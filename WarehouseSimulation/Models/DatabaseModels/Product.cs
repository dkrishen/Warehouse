using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation.Models.DatabaseModels
{
    public class Product
    {
        public Guid Id { get; set; }
        public Guid TypeId { get; set; }
        public string Sku { get; set; }
        public float Cost { get; set; }

        public ProductType ProductType { get; set; }
        public List<RackProduct> RackProducts { get; set; }
        public List<DispatchProduct> DispatchProducts { get; set; }
        public List<DeliveryProduct> DeliveryProducts { get; set; }
    }
}
