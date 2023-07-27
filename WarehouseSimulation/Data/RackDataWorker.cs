using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
