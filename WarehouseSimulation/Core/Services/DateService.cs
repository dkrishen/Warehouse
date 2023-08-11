using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseSimulation.Data;

namespace WarehouseSimulation.Core.Services
{
    public class DateService : ObservableObject, IDateService
    {
        public DateTime CurrentDate
        {
            get { return GlobalVariables.CurrentDate; }
            set { 
                GlobalVariables.CurrentDate = value;
                OnPropertyChanged();
            }
        }

        public DateService()
        {
            DateTime maxDate = (new List<DateTime> 
            {
                DeliveryDataWorker.GetAllDeliveries().ToList()
                    .SelectMany(d 
                        => new[] { d.CreationDate, d.ApprovalDate ?? DateTime.MinValue })
                    .Max(), 
                DispatchDataWorker.GetAllDispatches().ToList()
                    .SelectMany(d 
                        => new[] { d.CreationDate, d.ApprovalDate ?? DateTime.MinValue })
                    .Max() 
            }).Max();

            if(maxDate == DateTime.MinValue)
            {
                CurrentDate = DateTime.Today;
            }
            else
            {
                CurrentDate = maxDate.Date;
            }
        }

        public void NextDay()
        {
            CurrentDate = CurrentDate.AddDays(1);
        }
    }
}
