using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation.Models.DatabaseModels
{
    public class DispatchProduct
    {
        public Guid DispatchId { get; set; }
        public Guid ProductId { get; set; }
        public int ProductCount { get; set; }

        public Dispatch Dispatch { get; set; }
        public Product Product { get; set; }
    }
}
