using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseSimulation.Models
{
    [PrimaryKey(nameof(DeliveryId), nameof(ProductId))]
    public class DeliveryProduct
    {
        public Guid DeliveryId { get; set; }
        public Guid ProductId { get; set; }
        public int ProductCount { get; set; }

        public Delivery Delivery { get; set; }
        public Product Product { get; set; }
    }
}
