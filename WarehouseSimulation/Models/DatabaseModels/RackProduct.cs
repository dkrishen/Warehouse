using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation.Models.DatabaseModels
{
    public class RackProduct
    {
        public Guid RackId { get; set; }
        public Guid ProductId { get; set; }
        public int ProductCount { get; set; }

        public Rack Rack { get; set; }
        public Product Product { get; set; }
    }
}
