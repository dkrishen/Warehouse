using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseSimulation.Core;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Data;
using WarehouseSimulation.Models.ViewModels;

namespace WarehouseSimulation.ViewModels
{
    internal class WarehouseViewModel : ViewModelBase
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
        public RelayCommand NavigateToDispatchesViewCommand { get; set; }
        public RelayCommand NavigateToRacksViewCommand { get; set; }
        public RelayCommand NavigateToProductsViewCommand { get; set; }

        public WarehouseViewModel(INavigationServices navService)
        {
            Navigation = navService;
            NavigateToDeliveriesViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<DeliveriesViewModel>();
            }, canExecute: o => true);
            NavigateToDispatchesViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<DispatchesViewModel>();
            }, canExecute: o => true);
            NavigateToRacksViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<RacksViewModel>();
            }, canExecute: o => true);
            NavigateToProductsViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<ProductsViewModel>();
            }, canExecute: o => true);
        }


        private List<ProductViewDto> _AllProducts = ProductDataWorker.GetProductsCountInfo().ToList();
        public List<ProductViewDto> AllProducts
        {
            get { return _AllProducts; }
            set { _AllProducts = value; OnPropertyChanged("AllProducts"); }
        }

        internal void UpdateData()
        {
            AllProducts = ProductDataWorker.GetProductsCountInfo().ToList();
        }
    }
}
