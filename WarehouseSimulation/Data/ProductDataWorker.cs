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

        public static IEnumerable<ProductViewDto> GetShortProducts()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Products
                    .Include(p => p.Type)
                    .Select(p =>
                        new ProductViewDto
                        {
                            Id = p.Id,
                            Cost = p.Cost,
                            SKU = p.Sku,
                            Type = p.Type.TypeName,
                            Count = 0
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

        public static bool AddProduct(ProductViewDto newProduct)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                try
                {
                    var product = context.Products
                        .FirstOrDefault(r => r.Sku == newProduct.SKU);
                    var typeId = context.ProductTypes
                        .Single(pt => pt.TypeName == newProduct.Type).Id;

                    if (product == null)
                    {
                        context.Products.Add(new Product
                        {
                            Id = Guid.NewGuid(),
                            TypeId = typeId,
                            Cost = newProduct.Cost,
                            Sku = newProduct.SKU
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

        public static bool RemoveProduct(string productSku)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                try
                {
                    var product = context.Products.Single(r => r.Sku == productSku);

                    context.Products.Remove(product);
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
