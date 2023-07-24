using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSimulation.Core;
using WarehouseSimulation.Services;

namespace WarehouseSimulation.ViewModels
{
    public class DeliveriesViewModel : ViewModelBase
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

        public RelayCommand NavigateToProductsViewCommand { get; set; }

        public DeliveriesViewModel(INavigationServices navService)
        {
            Navigation = navService;
            NavigateToProductsViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<ProductViewModel>();
            }, canExecute: o => true);
        }
    }
}
