using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSimulation.Models.DatabaseModels;
using WarehouseSimulation.Models.ViewModels;

namespace WarehouseSimulation.Data
{
    public static class ProductDataWorker
    {
        public static IEnumerable<ProductViewDto> GetProductsCountInfo()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Products
                    .Include(p => p.RacksProducts)
                    .Include(p => p.Type)
                    .GroupBy(p => new { p.Id, p.Sku, p.Cost, p.Type.TypeName })
                    .Select(p =>
                        new ProductViewDto
                        {
                            Id = p.Key.Id,
                            Cost = p.Key.Cost,
                            SKU = p.Key.Sku,
                            Type = p.Key.TypeName,
                            Count = GetProductCountByProductId(p.Key.Id)
                        })
                    .ToList();
            }
        }

        public static int GetProductCountByProductId(Guid id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.RacksProducts
                    .Where(rp => rp.ProductId == id)
                    .Sum(rp => rp.ProductCount);
            }
        }
    }
}
