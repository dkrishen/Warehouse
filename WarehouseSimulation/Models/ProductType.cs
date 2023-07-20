using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseSimulation.Models
{
    public class ProductType
    {
        [Key]
        public Guid Id { get; set; }
        public string TypeName { get; set; }

        [ForeignKey(nameof(Product.Id))]
        public List<Product> Products { get; set; }
        [ForeignKey(nameof(Rack.Id))]
        public List<Rack> Racks { get; set; }
    }
}
