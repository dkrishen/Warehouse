using System.Collections.Generic;
using System.Linq;
using WarehouseSimulation.Core;
using WarehouseSimulation.Models.Data;
using WarehouseSimulation.Models.ViewModels;
using WarehouseSimulation.Services;

namespace WarehouseSimulation.ViewModels
{
    internal class ProductViewModel : ViewModelBase
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

        public RelayCommand NavigateToDeliveriesViewCommand { get; set; }
        public ProductViewModel(INavigationServices navService)
        {
            Navigation = navService;
            NavigateToDeliveriesViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<DeliveriesViewModel>();
            }, canExecute: o => true);
        }

        private List<ProductViewDto> allProducts = DataWorker.GetProductsCountInfo().ToList();
        public List<ProductViewDto> AllProducts
        {
            get { return allProducts; }
            set { allProducts = value; OnPropertyChanged("allProducts"); }
        }
    }
}
