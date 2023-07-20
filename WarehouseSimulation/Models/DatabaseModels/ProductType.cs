using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation.Models.DatabaseModels
{
    public class ProductType
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; }

        public IEnumerable<Product> Products { get; set;}
        public IEnumerable<Rack> Racks { get; set;}
    }
}
