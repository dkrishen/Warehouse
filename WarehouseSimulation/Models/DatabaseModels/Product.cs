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
        public IEnumerable<RackProduct> RackProducts { get; set; }
        //public IEnumerable<Dispatch> Dispatches { get; set; }
        //public IEnumerable<Delivery> Deliveries { get; set; }
    }
}
