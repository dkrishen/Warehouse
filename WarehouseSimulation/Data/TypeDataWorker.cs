using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseSimulation.Models.DatabaseModels;
using WarehouseSimulation.Models.ViewModels;

namespace WarehouseSimulation.Data
{
    public static class TypeDataWorker
    {
        public static IEnumerable<string> GetTypeNames()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.ProductTypes
                    .Select(pt => pt.TypeName)
                    .ToList();
            }
        }

        public static bool AddType(string typeName)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                try
                {
                    var type = context.ProductTypes
                        .Where(pr => pr.TypeName == typeName)
                        .FirstOrDefault();

                    if(type == null)
                    {
                        context.ProductTypes
                            .Add(new ProductType
                            {
                                Id = Guid.NewGuid(),
                                TypeName = typeName
                            });
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                } catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static bool RemoveType(string typeName)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                try
                {
                    var type = context.ProductTypes.Single(pt =>  pt.TypeName == typeName);
                    context.ProductTypes.Remove(type);
                    context.SaveChanges();

                    return true;
                } catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
