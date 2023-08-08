using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    .Include(rp => rp.Rack)
                    .Where(rp => rp.RackId == rackId)
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

        public static bool PutProductOnTheRack(string productSKU, int rackNumber, int count)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                try
                {
                    var product = context.Products.Single(p => p.Sku == productSKU);
                    var rack = context.Racks.Single(r => r.Number == rackNumber);

                    context.RacksProducts.Add(new RacksProduct
                    {
                        RackId = rack.Id,
                        ProductId = product.Id,
                        ProductCount = count,
                        Id = Guid.NewGuid()
                    });

                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex) { }
                return false;
            }
        }

        public static bool PutProductOnTheSump(string productSKU, int count)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                try
                {
                    var product = context.Products.Single(p => p.Sku == productSKU);

                    context.RacksProducts.Add(new RacksProduct
                    {
                        RackId = null,
                        ProductId = product.Id,
                        ProductCount = count,
                        Id = Guid.NewGuid()
                    });

                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex) { }
                return false;
            }
        }
    }
}
