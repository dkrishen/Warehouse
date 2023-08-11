using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WarehouseSimulation.Core;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Data;
using WarehouseSimulation.Models.DatabaseModels;
using System.Windows.Controls;
using Microsoft.IdentityModel.Tokens;

namespace WarehouseSimulation.ViewModels
{
    public class TransferReportViewModel : ViewModelBase
    {
        private INavigationServices _Navigation;
        public INavigationServices Navigation
        {
            get => _Navigation;
            set
            {
                _Navigation = value;
                OnPropertyChanged();
            }
        }

        private IDateService _DateService;
        public IDateService DateService
        {
            get => _DateService;
            set
            {
                _DateService = value;
                OnPropertyChanged();
            }
        }


        private SeriesCollection _SeriesCollection { get; set; }
        public SeriesCollection SeriesCollection
        {
            get => _SeriesCollection; set
            {
                _SeriesCollection = value;
                OnPropertyChanged();
            }
        }

        private string[] _Labels { get; set; }
        public string[] Labels
        { 
            get => _Labels;
            set
            {
                _Labels = value;
                OnPropertyChanged();
            }
        }


        private Func<double, string> _Formatter { get; set; }
        public Func<double, string> Formatter
        {
            get => _Formatter;
            set
            {
                _Formatter = value; 
                OnPropertyChanged();
            }
        }

        public string SelectedMonth { get; set; }
        public int SelectedYear { get; set; }
        public List<int> Years { get; set; }
        public string[] Monthes { get; set; }
        
        public RelayCommand NavigateToPreviousViewCommand { get; set; }


        public TransferReportViewModel(INavigationServices navService, IDateService dateService)
        {
            Navigation = navService;
            DateService = dateService;

            Years = ReportDataWorker.GetYearsWithActions().ToList();
            Monthes = DateService.Months.Keys.ToArray();

            UpdateChart();

            NavigateToPreviousViewCommand = new RelayCommand(o =>
            {
                Navigation.ToBack();
            }, canExecute: o => true);
        }

        public void UpdateChart()
        {
            if(SelectedMonth.IsNullOrEmpty())
            {
                SelectedMonth = DateService.GetMonthName(DateService.CurrentDate.Month);
            }
            if(SelectedYear == 0)
            {
                SelectedYear = DateService.CurrentDate.Year;
            }

            var startDate = new DateTime(SelectedYear, DateService.GetMonthNumber(SelectedMonth), 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var data = ReportDataWorker.GetTransferDetailsByMonth(startDate, endDate).ToList();

            ChartValues<int> deliveredValues = new ChartValues<int>();
            deliveredValues.AddRange(data.Select(d => d.CountDelivered).ToList());

            ChartValues<int> dispatchedValues = new ChartValues<int>();
            dispatchedValues.AddRange(data.Select(d => d.CountDispatched).ToList());

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Delivered",
                    Values = deliveredValues
                },
                new ColumnSeries
                {
                    Title = "Dispatched",
                    Values = dispatchedValues
                }
            };

            Labels = data.Select(d => d.ProductSku).ToArray();
            Formatter = value => value.ToString("N");
        }

        public void UpdateData()
        {
            Years = ReportDataWorker.GetYearsWithActions().ToList();
            UpdateChart();
        }

    }
}
