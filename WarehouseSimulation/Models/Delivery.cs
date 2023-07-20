using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseSimulation.Models
{
    public class Delivery
    {
        [Key]
        public Guid Id { get; set; }
        public DateOnly? ApprovalDate { get; set; }
        public DateOnly CreationDate { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey(nameof(DeliveryProduct.DeliveryId))]
        public List<DeliveryProduct> DeliveryProducts { get; set; }
    }
}
