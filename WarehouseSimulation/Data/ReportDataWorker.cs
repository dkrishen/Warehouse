using Microsoft.EntityFrameworkCore;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSimulation.Core;
using WarehouseSimulation.Models.ViewModels;

namespace WarehouseSimulation.Data
{
    public static class ReportDataWorker
    {
        public static IEnumerable<TransferReportViewDto> GetTransferDetailsByMonth(DateTime startDate, DateTime endDate)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var products = context.Products
                    .Include(p => p.DispatchesProducts)
                    .ThenInclude(dp => dp.Dispatch)
                    .Include(p => p.DeliveriesProducts)
                    .ThenInclude(dp => dp.Delivery)
                    .Where(p =>
                        p.DeliveriesProducts.Where(dp => dp.Delivery.ApprovalDate != null
                            && (DateTime)dp.Delivery.ApprovalDate >= startDate
                            && (DateTime)dp.Delivery.ApprovalDate <= endDate).Count() != 0
                        || p.DispatchesProducts.Where(dp => dp.Dispatch.ApprovalDate != null
                            && (DateTime)dp.Dispatch.ApprovalDate >= startDate
                            && (DateTime)dp.Dispatch.ApprovalDate <= endDate).Count() != 0);

                return products.Select(p => new TransferReportViewDto
                {
                    ProductSku = p.Sku,
                    CountDelivered = p.DeliveriesProducts.Where(dp => dp.Delivery.ApprovalDate != null
                            && (DateTime)dp.Delivery.ApprovalDate >= startDate
                            && (DateTime)dp.Delivery.ApprovalDate <= endDate).Sum(dp => dp.ProductCount),
                    CountDispatched = p.DispatchesProducts.Where(dp => dp.Dispatch.ApprovalDate != null
                            && (DateTime)dp.Dispatch.ApprovalDate >= startDate
                            && (DateTime)dp.Dispatch.ApprovalDate <= endDate).Sum(dp => dp.ProductCount),
                }).ToList();
            }
        }


        public static IEnumerable<int> GetYearsWithActions()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                int maxDate = (new List<DateTime>
                {
                    DeliveryDataWorker.GetAllDeliveries().ToList()
                        .SelectMany(d
                            => new[] { d.CreationDate, d.ApprovalDate ?? DateTime.MinValue })
                        .Max(),
                    DispatchDataWorker.GetAllDispatches().ToList()
                        .SelectMany(d
                            => new[] { d.CreationDate, d.ApprovalDate ?? DateTime.MinValue })
                        .Max()
                }).Max().Year;

                int minDate = (new List<DateTime>
                {
                    DeliveryDataWorker.GetAllDeliveries().ToList()
                        .SelectMany(d
                            => new[] { d.CreationDate, d.ApprovalDate ?? DateTime.MaxValue })
                        .Min(),
                    DispatchDataWorker.GetAllDispatches().ToList()
                        .SelectMany(d
                            => new[] { d.CreationDate, d.ApprovalDate ?? DateTime.MaxValue })
                        .Min()
                }).Min().Year;

                if(minDate <= maxDate)
                {
                    return Enumerable.Range(minDate, maxDate - minDate + 1).ToList();
                }

                return new List<int>() { GlobalVariables.CurrentDate.Year };
            }
        }
    }
}
