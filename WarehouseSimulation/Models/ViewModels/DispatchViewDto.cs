using System;

namespace WarehouseSimulation.Models.ViewModels
{
    public class DispatchViewDto
    {
        public Guid DispatchId { get; set; }
        public DateTime CreationDate { get; set; }
        public float TotalCost { get; set; }
    }
}
