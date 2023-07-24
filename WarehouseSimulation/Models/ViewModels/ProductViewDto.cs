using System;

namespace WarehouseSimulation.Models.ViewModels
{
    public class ProductViewDto
    {
        public Guid Id { get; set; }
        public string SKU { get; set; }
        public string Type { get; set; }
        public float Cost { get; set; }
        public int Count { get; set; }
    }
}
