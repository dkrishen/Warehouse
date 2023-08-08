using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation.Core
{
    public static class GlobalVariables
    {
        public static Guid? SelectedDeliveryId { get; set; }
        public static Guid? SelectedDispatchId { get; set; }
    }
}
