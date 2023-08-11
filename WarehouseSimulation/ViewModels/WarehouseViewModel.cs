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

        private ProductViewDto _SelectedProduct { get; set; }
        public ProductViewDto SelectedProduct
        {
            get { return _SelectedProduct; }
            set { _SelectedProduct = value; GlobalVariables.SelectedProductSku = _SelectedProduct?.SKU; }
        }

        public RelayCommand NavigateToDeliveriesViewCommand { get; set; }
        public RelayCommand NavigateToDispatchesViewCommand { get; set; }
        public RelayCommand NavigateToRacksViewCommand { get; set; }
        public RelayCommand NavigateToProductsViewCommand { get; set; }
        public RelayCommand NavigateToLocationsViewCommand { get; set; }
        public RelayCommand NextDayCommand { get; set; }
        public RelayCommand TransferReportCommand { get; set; }
        public RelayCommand ExpensesReportCommand { get; set; }

        public WarehouseViewModel(INavigationServices navService, IDateService dateService)
        {
            Navigation = navService;
            DateService = dateService;

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
            NextDayCommand = new RelayCommand(o =>
            {
                DateService.NextDay();
            }, canExecute: o => true);
            TransferReportCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<TransferReportViewModel>();
            }, canExecute: o => true);
            ExpensesReportCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<ExpensesReportViewModel>();
            }, canExecute: o => true);
            NavigateToLocationsViewCommand = new RelayCommand(o =>
            {
                if (SelectedProduct != null)
                {
                    GlobalVariables.SelectedProductSku = SelectedProduct.SKU;
                    Navigation.NavigateTo<ProductLocaionInfoViewModel>();
                }
            }, canExecute: o => true);
        }


        private List<ProductViewDto> _AllProducts = ProductDataWorker.GetProductsCountInfo().ToList();
        public List<ProductViewDto> AllProducts
        {
            get { return _AllProducts; }
            set { _AllProducts = value; OnPropertyChanged("AllProducts"); }
        }

        public void UpdateData()
        {
            AllProducts = ProductDataWorker.GetProductsCountInfo().ToList();
        }

        public void ViewLocations()
        {
            NavigateToLocationsViewCommand.Execute(this);
        }
    }
}
