using System;
using System.Collections.Generic;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Core;
using WarehouseSimulation.Data;
using WarehouseSimulation.Models.ViewModels;
using WarehouseSimulation.Models.DatabaseModels;
using System.Linq;
using System.Windows;

namespace WarehouseSimulation.ViewModels
{
    public class DispatchInfoViewModel : ViewModelBase
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

        private List<ProductViewDto> _AllProducts { get; set; }
        public List<ProductViewDto> AllProducts
        {
            get { return _AllProducts; }
            set { _AllProducts = value; OnPropertyChanged("AllProducts"); }
        }

        public RelayCommand RemoveDispatchCommand { get; set; }
        public RelayCommand ApproveDispatchCommand { get; set; }
        public RelayCommand NavigateToPreviousViewCommand { get; set; }
        public Guid? DispatchId { get; set; }

        public DispatchInfoViewModel(INavigationServices navService, IDateService dateService)
        {
            DispatchId = GlobalVariables.SelectedDispatchId;
            Navigation = navService;
            DateService = dateService;
            NavigateToPreviousViewCommand = new RelayCommand(o =>
            {
                Navigation.ToBack();
            }, canExecute: o => true);
            ApproveDispatchCommand = new RelayCommand(o =>
            {
                var result = DispatchDataWorker.ApproveDispatch(DispatchId ?? Guid.Empty, DateService.CurrentDate);
                DateService.NextDay();

                if (result.IsSuccessfully)
                {
                    result = RackDataWorker.CheckSump(result);
                    result.Show();

                    NavigateToPreviousViewCommand.Execute(true);
                }
            }, canExecute: o => true);
            RemoveDispatchCommand = new RelayCommand(o =>
            {
                DispatchDataWorker.RemoveDispatch(DispatchId ?? Guid.Empty);
                NavigateToPreviousViewCommand.Execute(true);
            }, canExecute: o => true);

            if (DispatchId == null)
            {
                NavigateToPreviousViewCommand.Execute(true);
            }
            else
            {
                AllProducts = DispatchDataWorker.GetProductsByDispatchId(DispatchId ?? Guid.Empty).ToList();
            }
        }

        public void UpdateData()
        {
            DispatchId = GlobalVariables.SelectedDispatchId;
            AllProducts = DispatchDataWorker.GetProductsByDispatchId(DispatchId ?? Guid.Empty).ToList();
            GlobalVariables.SelectedDispatchId = null;
        }
    }
}
