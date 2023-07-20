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

        public List<Product> Products { get; set;}
        public List<Rack> Racks { get; set;}
    }
}
