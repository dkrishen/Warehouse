using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation.Models.DatabaseModels
{
    public class DeliveryProduct
    {
        public Guid DeliveryId { get; set; }
        public Guid ProductId { get; set; }
        public int ProductCount { get; set; }

        public Delivery Delivery { get; set; }
        public Product Product { get; set; }
    }
}
