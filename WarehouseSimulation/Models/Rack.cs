using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseSimulation.Models
{
    public class Rack
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TypeId { get; set; }
        public int Size { get; set; }
        public int Number { get; set; }

        [ForeignKey(nameof(RackProduct.RackId))]
        public List<RackProduct> RackProducts { get; set; }
    }
}
