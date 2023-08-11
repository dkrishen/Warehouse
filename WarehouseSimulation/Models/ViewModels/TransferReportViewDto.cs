namespace WarehouseSimulation.Models.ViewModels
{
    public class TransferReportViewDto
    {
        public string ProductSku { get; set; }
        public int CountDelivered { get; set; }
        public int CountDispatched { get; set; }
    }
}
