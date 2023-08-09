using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseSimulation.Models.CoreModels;
using WarehouseSimulation.Models.DatabaseModels;
using WarehouseSimulation.Models.ViewModels;

namespace WarehouseSimulation.Data
{
    public static class RackDataWorker
    {
        public static IEnumerable<RackViewDto> GetRacks()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Racks
                    .Include(r => r.Type)
                    .Select(r => new RackViewDto
                    {
                        Number = r.Number,
                        Type = r.Type.TypeName,
                        Size = r.Size
                    })
                    .ToList();
            }
        }

        public static bool AddRack(RackViewDto newRack)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                try
                {
                    var rack = context.Racks
                        .FirstOrDefault(r => r.Number == newRack.Number);
                    var typeId = context.ProductTypes
                        .Single(pt => pt.TypeName == newRack.Type).Id;

                    if (rack == null)
                    {
                        context.Racks.Add(new Rack
                        {
                            Id = Guid.NewGuid(),
                            TypeId = typeId,
                            Size = newRack.Size,
                            Number = newRack.Number
                        });
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static bool RemoveRack(int rackNumber)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                try
                {
                    var rack = context.Racks.Single(r => r.Number == rackNumber);

                    context.Racks.Remove(rack);
                    context.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static IEnumerable<IncompleteRackDto> GetIncompleteRacksByTypes(ICollection<string> typeNames)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var racks = context.Racks
                    .Include(r => r.Type)
                    .Where(r => typeNames.Contains(r.Type.TypeName))
                    .Select(r => new IncompleteRackDto
                    {
                        Number = r.Number,
                        Type = r.Type.TypeName,
                        Size = r.Size,
                    }).ToList();

                racks.ForEach(rack =>
                    {
                        rack.FreeSpace = GetFreeSpaceAmountInRack(rack.Number);
                    });

                return racks.Where(r => r.FreeSpace > 0)
                    .ToList();
            }
        }

        public static IEnumerable<RackViewDto> GetRacksByProduct(string productSKU)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.RacksProducts
                    .Include(rp => rp.Product)
                    .Where(rp => rp.Product.Sku == productSKU && rp.RackId != null)
                    .Include(rp => rp.Rack)
                    .ThenInclude(r => r.Type)
                    .Select(rp => new RackViewDto
                    {
                        Number = rp.Rack.Number,
                        Type = rp.Rack.Type.TypeName,
                        Size = rp.Rack.Size
                    })
                    .ToList();
            }
        }

        public static int GetProductsCountInRack(int rackNumber, string productSku)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.RacksProducts
                    .Include(rp => rp.Rack)
                    .Include(rp => rp.Product)
                    .First(rp => rp.Rack.Number == rackNumber && rp.Product.Sku == productSku)
                    .ProductCount;
            }
        }

        public static int GetProductsCountInSump(string productSku)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var sump = context.RacksProducts
                    .Include(rp => rp.Product)
                    .FirstOrDefault(rp => rp.RackId == null && rp.Product.Sku == productSku);

                if(sump == null)
                {
                    return 0;
                }
                else
                {
                    return sump.ProductCount;
                }
            }
        }

        public static bool TakeProductFromRack(string productSKU, int rackNumber, int count)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                try
                {
                    var rackProduct = context.RacksProducts
                        .Where(rp => rp.RackId != null)
                        .Include(rp => rp.Rack)
                        .Include(rp => rp.Product)
                        .FirstOrDefault(rp => rp.Rack.Number == rackNumber && rp.Product.Sku == productSKU);

                    if (rackProduct == null || rackProduct.ProductCount < count)
                    {
                        return false;
                    }

                    rackProduct.ProductCount -= count;

                    if(rackProduct.ProductCount == 0)
                    {
                        context.RacksProducts.Remove(rackProduct);
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

        public static bool TakeProductFromSump(string productSKU, int count)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                try
                {
                    var sump = context.RacksProducts
                        .Include(rp => rp.Product)
                        .FirstOrDefault(rp => rp.RackId == null && rp.Product.Sku == productSKU);

                    if (sump == null || sump.ProductCount < count)
                    {
                        return false;
                    }

                    sump.ProductCount -= count;
                    context.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static int GetFreeSpaceAmountInRack(int rackNumber)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return GetFreeSpaceAmountInRack(context.Racks.Single(r => r.Number == rackNumber).Id);
            }
        }
        
        public static int GetFreeSpaceAmountInRack(Guid rackId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var racksProducts = context.RacksProducts
                    .Where(rp => rp.RackId == rackId)
                    .Include(rp => rp.Rack)
                    .ToList();

                if (racksProducts.Count == 0)
                {
                    return context.Racks.Single(r => r.Id == rackId).Size;
                }
                else
                {
                    return racksProducts
                        .First().Rack.Size - racksProducts.Sum(rp => rp.ProductCount);
                }
            }
        }

        public static bool PutProductOnRack(string productSKU, int rackNumber, int count)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                try
                {
                    var product = context.Products.Single(p => p.Sku == productSKU);
                    var rack = context.Racks.Single(r => r.Number == rackNumber);

                    var rackProduct = context.RacksProducts
                        .FirstOrDefault(rp => rp.RackId != null 
                            && rp.RackId == rack.Id 
                            && rp.ProductId == product.Id);

                    if (rackProduct == null) {
                        context.RacksProducts.Add(new RacksProduct
                        {
                            RackId = rack.Id,
                            ProductId = product.Id,
                            ProductCount = count,
                            Id = Guid.NewGuid()
                        });
                    }
                    else
                    {
                        rackProduct.ProductCount += count;
                    }

                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex) { }
                return false;
            }
        }

        public static bool PutProductOnSump(string productSKU, int count)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                try
                {
                    var productSump = context.RacksProducts
                        .Include(rp => rp.Product)
                        .FirstOrDefault(rp => rp.Product.Sku == productSKU && rp.RackId == null);

                    if (productSump != null)
                    {
                        productSump.ProductCount += count;
                    }
                    else
                    {
                        var product = context.Products.Single(p => p.Sku == productSKU);

                        context.RacksProducts.Add(new RacksProduct
                        {
                            RackId = null,
                            ProductId = product.Id,
                            ProductCount = count,
                            Id = Guid.NewGuid()
                        });
                    }
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex) { }
                return false;
            }
        }

        public static OperationDto CheckSump()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var result = new OperationDto();

                var sumps = context.RacksProducts
                    .Where(rp => rp.RackId == null)
                    .Include(rp => rp.Product)
                    .ThenInclude(p => p.Type)
                    .ToList();

                sumps.ForEach(sump =>
                {
                    var racks = GetIncompleteRacksByTypes(new HashSet<string> { sump.Product.Type.TypeName });

                    racks.Where(rack => rack.Type == sump.Product.Type.TypeName)
                        .ToList()
                        .ForEach(rack =>
                        {
                            if (sump.ProductCount > 0)
                            {
                                var freeSpace = GetFreeSpaceAmountInRack(rack.Number);
                                var delta = Math.Min(sump.ProductCount, freeSpace);

                                sump.ProductCount -= delta;
                                PutProductOnRack(sump.Product.Sku, rack.Number, delta);

                                result.Tags.Add($"{delta} {sump.Product.Sku} moved to {rack.Number} Rack;");
                                result.IsSuccessfully = true;
                            }
                        });
                });

                context.SaveChanges();
                return result;
            }
        }
    }
}
