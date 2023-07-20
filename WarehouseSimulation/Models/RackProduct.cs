using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WarehouseSimulation.Models
{
    [PrimaryKey(nameof(RackId), nameof(ProductId))]
    public class RackProduct
    {
        public Guid RackId { get; set; }
        public Guid ProductId { get; set; }
        public int ProductCount { get; set; }

        public Rack Rack { get; set; }
        public Product Product { get; set; }
    }
}
