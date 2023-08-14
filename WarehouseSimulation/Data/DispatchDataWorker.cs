using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using WarehouseSimulation.Models.CoreModels;
using WarehouseSimulation.Models.DatabaseModels;
using WarehouseSimulation.Models.ViewModels;

namespace WarehouseSimulation.Data
{
    public static class DispatchDataWorker
    {
        public static bool CreateDispatch(List<ProductViewDto> products, DateTime creationDate)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                try
                {
                    List<ProductViewDto> missingProducts = new List<ProductViewDto>();

                    var dispatchId = context.Dispatches.Add(new Dispatch
                    {
                        Id = Guid.NewGuid(),
                        ApprovalDate = null,
                        CreationDate = creationDate,
                        IsActive = true,
                    }).Entity.Id;

                    foreach (var product in products)
                    {
                        var availableQuantity = ProductDataWorker.GetProductCountByProductId(product.Id);
                        if(availableQuantity < product.Count)
                        {
                            missingProducts.Add(new ProductViewDto
                            {
                                SKU = product.SKU,
                                Id = product.Id,
                                Cost = product.Cost,
                                Type = product.Type,
                                Count = product.Count - availableQuantity
                            });
                        }

                        context.DispatchesProducts.Add(new DispatchesProduct
                        {
                            DispatchId = dispatchId,
                            ProductId = product.Id,
                            ProductCount = product.Count,
                        });
                    }

                    if(missingProducts.Count > 0)
                    {
                        DeliveryDataWorker.CreateDelivery(missingProducts, creationDate);
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

        public static bool RemoveDispatch(Guid dispatchId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                try
                {
                    var dispatchesProducts = context.DispatchesProducts
                        .Where(dp => dp.DispatchId == dispatchId)
                        .ToList();
                    dispatchesProducts
                        .ForEach(dp
                            => context.DispatchesProducts
                                .Remove(dp));

                    var dispatch = context.Dispatches
                        .Single(d => d.Id == dispatchId);
                    context.Dispatches.Remove(dispatch);

                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static IEnumerable<DispatchViewDto> GetShortDispatches()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Dispatches
                    .Where(d => d.IsActive)
                    .Include(d => d.DispatchesProducts)
                    .ThenInclude(dp => dp.Product)
                    .Select(d => new DispatchViewDto
                    {
                        DispatchId = d.Id,
                        CreationDate = d.CreationDate,
                        TotalCost = d.DispatchesProducts
                            .Select(dp => dp.Product.Cost * dp.ProductCount)
                            .Sum(),
                    })
                    .ToList();
            }
        }

        public static IEnumerable<ProductViewDto> GetProductsByDispatchId(Guid id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.DispatchesProducts
                    .Where(dp => dp.DispatchId == id)
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

        public static OperationDto ApproveDispatch(Guid dispatchId, DateTime approvalDate, OperationDto result = null)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                if (result == null)
                {
                    result = new OperationDto();
                }

                try
                {
                    var products = ProductDataWorker.GetProductsCountInfoByDispatchId(dispatchId).ToList();

                    bool isProductsEnought = true;
                    products.ForEach(product =>
                    {
                        if(product.Count > ProductDataWorker.GetProductCountByProductId(product.Id))
                        {
                            isProductsEnought = false;
                            return;
                        }
                    });

                    if(isProductsEnought)
                    {
                        products.ForEach(product =>
                        {
                            var isEnoughBefore = ProductDataWorker.GetProductCountByProductId(product.Id) > product.RecommendedAmount;

                            var racks = RackDataWorker.GetRacksByProduct(product.SKU).ToList();
                            
                            racks.ForEach(rack =>
                            {
                                var count = RackDataWorker.GetProductsCountInRack(rack.Number, product.SKU);
                                var delta = Math.Min(count, product.Count);

                                RackDataWorker.TakeProductFromRack(product.SKU, rack.Number, delta);
                                product.Count -= delta;

                                if (product.Count <= 0)
                                {
                                    return;
                                }
                            });

                            if(product.Count > 0)
                            {
                                RackDataWorker.TakeProductFromSump(product.SKU, product.Count);
                            }

                            var isEnoughAfter = ProductDataWorker.GetProductCountByProductId(product.Id) > product.RecommendedAmount;

                            if (isEnoughBefore != isEnoughAfter)
                            {
                                result.Tags.Add($"{product.SKU} fell below the recommended amount;");
                                result.IsRequiredNotification = true;
                            }
                        });

                        var dispatch = context.Dispatches.Single(d => d.Id == dispatchId);
                        dispatch.ApprovalDate = approvalDate;
                        dispatch.IsActive = false;
                        context.SaveChanges();
                        result.IsSuccessfully = true;
                    }
                    else
                    {
                        result.IsSuccessfully = false;
                    }

                }
                catch (Exception ex)
                {
                    result.IsSuccessfully = false;
                }

                return result;
            }
        }

        public static IEnumerable<Dispatch> GetAllDispatches()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Dispatches.ToList();
            }
        }
    }
}
