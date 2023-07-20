using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation.Models.DatabaseModels
{
    public class Delivery
    {
        public Guid Id { get; set; }
        public DateOnly? ApprovalDate { get; set; }
        public DateOnly CreationDate { get; set; }
        public bool IsActive { get; set; }

        public IEnumerable<DeliveryProduct> DeliveryProducts { get; set; }
    }
}
