using System.Collections.Generic;

namespace WarehouseSimulation.Models.CoreModels
{
    public class OperationDto
    {
        public OperationDto()
        {
            IsSuccessfully = false;
            Tags = new List<string>();
        }

        public bool IsSuccessfully { get; set; }
        public List<string> Tags { get; set; }
    }
}
