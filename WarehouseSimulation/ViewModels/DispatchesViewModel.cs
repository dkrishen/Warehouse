using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Core;

namespace WarehouseSimulation.ViewModels
{
    public class DispatchesViewModel : ViewModelBase
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

        public RelayCommand NavigateToAddDispatchViewCommand { get; set; }
        public RelayCommand NavigateToPreviousViewCommand { get; set; }

        public DispatchesViewModel(INavigationServices navService)
        {
            Navigation = navService;
            NavigateToAddDispatchViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<AddDispatchViewModel>();
            }, canExecute: o => true);
            NavigateToPreviousViewCommand = new RelayCommand(o =>
            {
                Navigation.ToBack();
            }, canExecute: o => true);
        }

    }
}
