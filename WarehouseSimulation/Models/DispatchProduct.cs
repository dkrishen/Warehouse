using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseSimulation.Models
{
    [PrimaryKey(nameof(DispatchId), nameof(ProductId))]
    public class DispatchProduct
    {
        public Guid DispatchId { get; set; }
        public Guid ProductId { get; set; }
        public int ProductCount { get; set; }

        public Dispatch Dispatch { get; set; }
        public Product Product { get; set; }
    }
}
