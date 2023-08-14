using System;

namespace WarehouseSimulation.Models.ViewModels
{
    public class DeliveryViewDto
    {
        public Guid DeliveryId { get; set; }
        public DateTime CreationDate { get; set; }
        public float TotalCost { get; set; }
    }
}
