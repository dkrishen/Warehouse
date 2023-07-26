using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Core;

namespace WarehouseSimulation.ViewModels
{
    public class DeliveryInfoViewModel : ViewModelBase
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

        public RelayCommand RemoveDeliveryCommand { get; set; }
        public RelayCommand ApproveDeliveryCommand { get; set; }
        public RelayCommand NavigateToPreviousViewCommand { get; set; }

        public DeliveryInfoViewModel(INavigationServices navService)
        {
            Navigation = navService;
            NavigateToPreviousViewCommand = new RelayCommand(o =>
            {
                Navigation.ToBack();
            }, canExecute: o => true);
            ApproveDeliveryCommand = new RelayCommand(o =>
            {
                // TODO:

                NavigateToPreviousViewCommand.Execute(true);
            }, canExecute: o => true);
            RemoveDeliveryCommand = new RelayCommand(o =>
            {
                // TODO:

                NavigateToPreviousViewCommand.Execute(true);
            }, canExecute: o => true);
        }
    }
}
