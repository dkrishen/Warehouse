using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation.Models.DatabaseModels
{
    public class Dispatch
    {
        public Guid Id { get; set; }
        public DateOnly? ApprovalDate { get; set; }
        public DateOnly CreationDate { get; set; }
        public bool IsActive { get; set; }

        public List<DispatchProduct> DispatchProducts { get; set; }
    }
}
