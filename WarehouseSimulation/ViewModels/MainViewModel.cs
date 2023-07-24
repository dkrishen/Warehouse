using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSimulation.Core;
using WarehouseSimulation.Services;

namespace WarehouseSimulation.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private INavigationServices _Navigation { get; set; }

        public INavigationServices Navigation
        {
            get { return _Navigation; }
            set
            {
                _Navigation = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NavigateToDeliveriesViewCommand { get; set; }
        public RelayCommand NavigateToProductsViewCommand { get; set; }

        public MainViewModel(INavigationServices navService)
        {
            Navigation = navService;
            NavigateToDeliveriesViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<DeliveriesViewModel>();
            }, canExecute: o => true);
            NavigateToProductsViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<ProductViewModel>();
            }, canExecute: o => true);
        }
    }
}
