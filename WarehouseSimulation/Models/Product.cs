using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseSimulation.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TypeId { get; set; }
        public string Sku { get; set; }
        public float Cost { get; set; }

        public ProductType ProductType { get; set; }
        [ForeignKey(nameof(RackProduct.ProductId))]
        public List<RackProduct> RackProducts { get; set; }
        [ForeignKey(nameof(DispatchProduct.ProductId))]
        public List<DispatchProduct> DispatchProducts { get; set; }
        [ForeignKey(nameof(DeliveryProduct.ProductId))]
        public List<DeliveryProduct> DeliveryProducts { get; set; }
    }
}
