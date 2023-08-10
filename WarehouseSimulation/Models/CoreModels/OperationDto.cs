using System.Collections.Generic;
using System.Windows;

namespace WarehouseSimulation.Models.CoreModels
{
    public class OperationDto
    {
        public OperationDto()
        {
            IsSuccessfully = false;
            IsRequiredNotification = false;
            Tags = new List<string>();
        }

        public void Show()
        {
            if (IsRequiredNotification)
            {
                MessageBox.Show(string.Join("\n", Tags));
            }
        }

        public bool IsRequiredNotification { get; set; }
        public bool IsSuccessfully { get; set; }
        public List<string> Tags { get; set; }
    }
}
