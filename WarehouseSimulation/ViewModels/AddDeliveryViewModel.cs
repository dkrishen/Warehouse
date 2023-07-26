using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Core;

namespace WarehouseSimulation.ViewModels
{
    public class AddDeliveryViewModel : ViewModelBase
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

        public RelayCommand NavigateToPreviousViewCommand { get; set; }
        public RelayCommand AddProductCommand { get; set; }
        public RelayCommand NavigateToProductsViewCommand { get; set; }
        public RelayCommand AddDeliveryCommand { get; set; }

        public AddDeliveryViewModel(INavigationServices navService)
        {
            Navigation = navService;
            NavigateToPreviousViewCommand = new RelayCommand(o =>
            {
                Navigation.ToBack();
            }, canExecute: o => true);
            NavigateToProductsViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<ProductsViewModel>();
            }, canExecute: o => true);
            AddProductCommand = new RelayCommand(o =>
            {
                // TODO:

            }, canExecute: o => true);
            AddDeliveryCommand = new RelayCommand(o =>
            {
                // TODO:

                NavigateToPreviousViewCommand.Execute(true);
            }, canExecute: o => true);
        }

    }
}
