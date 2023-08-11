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

        public string GetMonthName(int number)
        {
            return Months.Single(m => m.Value == number).Key;
        }

        public int GetMonthNumber(string name)
        {
            return Months[name];
        }

        public Dictionary<string, int> Months => new Dictionary<string, int>
        {
            { "January", 1 },
            { "February", 2 },
            { "March", 3 },
            { "April", 4 },
            { "May", 5 },
            { "June", 6 },
            { "July", 7 },
            { "August", 8 },
            { "September", 9 },
            { "October", 10 },
            { "November", 11 },
            { "December", 12 }
        };

    }
}
