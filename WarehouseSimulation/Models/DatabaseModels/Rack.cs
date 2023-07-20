using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation.Models.DatabaseModels
{
    public class Rack
    {
        public Guid Id { get; set; }
        public Guid TypeId { get; set; }
        public int Size { get; set; }
        public int Number { get; set; }

        public Type Type { get; set; }
        public List<RackProduct> RackProducts { get; set; }
    }
}
