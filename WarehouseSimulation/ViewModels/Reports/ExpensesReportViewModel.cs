using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseSimulation.Core;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Data;
using WarehouseSimulation.Models.ViewModels;

namespace WarehouseSimulation.ViewModels.Reports
{
    public class ExpensesReportViewModel : ViewModelBase
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

        public int SelectedYear { get; set; }
        public List<int> Years { get; set; }

        public RelayCommand NavigateToPreviousViewCommand { get; set; }


        public ExpensesReportViewModel(INavigationServices navService, IDateService dateService)
        {
            Navigation = navService;
            DateService = dateService;

            Years = ReportDataWorker.GetYearsWithActions().ToList();

            NavigateToPreviousViewCommand = new RelayCommand(o =>
            {
                Navigation.ToBack();
            }, canExecute: o => true);
        }

        public void UpdateChart()
        {
            if (SelectedYear == 0)
            {
                SelectedYear = DateService.CurrentDate.Year;
            }

            var result = ReportDataWorker.GetExpensesDetailsByYear(SelectedYear).ToList();

            var data = new List<ExpensesReportDto>();

            foreach (var month in DateService.Months)
            {
                data.Add(new ExpensesReportDto
                {
                    Month = month.Key,
                    Expenses = result.FirstOrDefault(d => d.Month == month.Value.ToString())?.Expenses ?? 0
                });
            }

            ChartValues<double> values = new ChartValues<double>();
            values.AddRange(data.Select(d => d.Expenses).ToList());

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Expenses",
                    Values = values
                }
            };

            Labels = data.Select(d => d.Month).ToArray();
            Formatter = value => value.ToString("N");
        }

        public void UpdateData()
        {
            Years = ReportDataWorker.GetYearsWithActions().ToList();
            UpdateChart();
        }
    }
}
