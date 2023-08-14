namespace WarehouseSimulation.Models.ViewModels
{
    public class TransferReportDto
    {
        public string ProductSku { get; set; }
        public int CountDelivered { get; set; }
        public int CountDispatched { get; set; }
    }
}
