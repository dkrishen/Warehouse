using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSimulation.Models.CoreModels;
using WarehouseSimulation.Models.DatabaseModels;
using WarehouseSimulation.Models.ViewModels;

namespace WarehouseSimulation.Data
{
    public static class DeliveryDataWorker
    {
        public static bool CreateDelivery(List<ProductViewDto> products, DateTime creationDate)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                try
                {
                    var deliveryId = context.Deliveries.Add(new Delivery
                    {
                        Id = Guid.NewGuid(),
                        ApprovalDate = null,
                        CreationDate = creationDate,
                        IsActive = true,
                    }).Entity.Id;

                    foreach (var product in products)
                    {
                        context.DeliveriesProducts.Add(new DeliveriesProduct
                        {
                            DeliveryId = deliveryId,
                            ProductId = product.Id,
                            ProductCount = product.Count,
                        });
                    }

                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static bool RemoveDelivery(Guid deliveryId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                try
                {
                    var deliveryProducts = context.DeliveriesProducts
                        .Where(dp => dp.DeliveryId == deliveryId)
                        .ToList();
                    deliveryProducts
                        .ForEach(dp 
                            => context.DeliveriesProducts
                                .Remove(dp));

                    var delivery = context.Deliveries
                        .Single(d => d.Id == deliveryId);
                    context.Deliveries.Remove(delivery);

                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static IEnumerable<DeliveryViewDto> GetShortDeliveries()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Deliveries
                    .Where(d => d.IsActive)
                    .Include(d => d.DeliveriesProducts)
                    .ThenInclude(dp => dp.Product)
                    .Select(d => new DeliveryViewDto
                    {
                        DeliveryId = d.Id,
                        CreationDate = d.CreationDate,
                        TotalCost = d.DeliveriesProducts
                            .Select(dp => dp.Product.Cost * dp.ProductCount)
                            .Sum(),
                    })
                    .ToList();
            }
        }

        public static IEnumerable<ProductViewDto> GetProductsByDeliveryId(Guid id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.DeliveriesProducts
                    .Where(dp => dp.DeliveryId == id)
                    .Include(dp => dp.Product)
                    .Select(d => new ProductViewDto
                    {
                        SKU = d.Product.Sku,
                        Count = d.ProductCount,
                        Cost = d.Product.Cost
                    })
                    .ToList();
            }
        }

        public static OperationDto ApproveDelivery(Guid deliveryId, DateTime approvalDate, OperationDto result = null)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                if (result == null)
                {
                    result = new OperationDto();
                }

                try
                {
                    var products = ProductDataWorker.GetProductsCountInfoByDeliveryId(deliveryId).ToList();
                    var racks = RackDataWorker.GetIncompleteRacksByTypes(products.Select(p => p.Type).ToHashSet()).ToList();

                    products.ForEach(product =>
                    {
                        var isEnoughBefore = ProductDataWorker.GetProductCountByProductId(product.Id) > product.RecommendedAmount;
                        do
                        {
                            var rack = racks.FirstOrDefault(r => r.Type == product.Type);
                            if (rack == null)
                            {
                                RackDataWorker.PutProductOnSump(product.SKU, product.Count);
                                break;
                            }
                            else
                            {
                                var delta = Math.Min(rack.FreeSpace, product.Count);

                                product.Count -= delta;

                                if (rack.FreeSpace - delta > 0)
                                {
                                    racks.Single(r => r.Number == rack.Number).FreeSpace -= delta;
                                }
                                else
                                {
                                    racks.Remove(rack);
                                }

                                RackDataWorker.PutProductOnRack(product.SKU, rack.Number, delta);
                            }
                        } while (product.Count > 0);

                        var isEnoughAfter = ProductDataWorker.GetProductCountByProductId(product.Id) > product.RecommendedAmount;
                        if (isEnoughBefore != isEnoughAfter)
                        {
                            result.Tags.Add($"{product.SKU} has reached the recommended amount;");
                            result.IsRequiredNotification = true;
                        }
                    });

                    var delivery = context.Deliveries.Single(d => d.Id == deliveryId);
                    delivery.ApprovalDate = approvalDate;
                    delivery.IsActive = false;
                    context.SaveChanges();

                    result.IsSuccessfully = true;
                } catch (Exception ex)
                {
                    result.IsSuccessfully = false;
                }

                return result;
            }
        }

        public static IEnumerable<Delivery> GetAllDeliveries()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Deliveries.ToList();
            }
        }
    }
}
