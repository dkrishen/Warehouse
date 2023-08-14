using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Core;
using WarehouseSimulation.Data;
using WarehouseSimulation.Models.ViewModels;

namespace WarehouseSimulation.ViewModels.Delivery
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

        private List<ProductViewDto> _AllProducts = ProductDataWorker.GetShortProducts().ToList();
        public List<ProductViewDto> AllProducts
        {
            get { return _AllProducts; }
            set
            {
                _AllProducts = value;
                OnPropertyChanged("AllProducts");
                OnPropertyChanged("AllProductsInDelivery");
                OnPropertyChanged("AllNotAddedProducts");
            }
        }

        private List<ProductViewDto> _AllProductsInDelivery = new List<ProductViewDto>();
        public List<ProductViewDto> AllProductsInDelivery
        {
            get { return _AllProductsInDelivery; }
            set
            {
                _AllProductsInDelivery = value;
                OnPropertyChanged("AllProductsInDelivery");
                OnPropertyChanged("AllNotAddedProducts");
            }
        }

        public List<string> AllNotAddedProducts
        {
            get
            {
                return _AllProducts
                    .Select(p => p.SKU)
                    .Except(AllProductsInDelivery
                        .Select(p => p.SKU))
                    .ToList();
            }
        }

        public ProductViewDto SelectedProductForRemove { get; set; }
        public string SelectedProductForAdd { get; set; }
        public string AddedProductCount { get; set; }

        public RelayCommand NavigateToPreviousViewCommand { get; set; }
        public RelayCommand AddProductCommand { get; set; }
        public RelayCommand RemoveProductCommand { get; set; }
        public RelayCommand NavigateToProductsViewCommand { get; set; }
        public RelayCommand AddDeliveryCommand { get; set; }

        public AddDeliveryViewModel(INavigationServices navService, IDateService dateService)
        {
            Navigation = navService;
            DateService = dateService;
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
                try
                {
                    var productCount = int.Parse(AddedProductCount);
                    if (SelectedProductForAdd != null
                        && SelectedProductForAdd.Replace(" ", "").Length != 0
                        && productCount > 0
                        && !AllProductsInDelivery.Select(p => p.SKU)
                            .Contains(SelectedProductForAdd))
                    {
                        var newProduct = AllProducts
                            .Single(p => p.SKU == SelectedProductForAdd);
                        newProduct.Count = productCount;
                        AllProductsInDelivery.Add(newProduct);

                        UpdateData();
                        InvokeListUpdate();
                    }
                }
                catch { }

            }, canExecute: o => true);
            RemoveProductCommand = new RelayCommand(o =>
            {
                try
                {
                    if (SelectedProductForRemove != null
                        && AllProductsInDelivery.Select(p => p.SKU)
                            .Contains(SelectedProductForRemove.SKU))
                    {
                        AllProductsInDelivery.Remove(SelectedProductForRemove);
                        UpdateData();
                        InvokeListUpdate();
                    }
                }
                catch { }

            }, canExecute: o => true);
            AddDeliveryCommand = new RelayCommand(o =>
            {
                if (AllProductsInDelivery.Count != 0
                    && DeliveryDataWorker.CreateDelivery(AllProductsInDelivery, DateService.CurrentDate))
                {
                    DateService.NextDay();
                    AllProductsInDelivery.Clear();
                    SelectedProductForAdd = null;
                    AddedProductCount = null;
                    NavigateToPreviousViewCommand.Execute(true);
                }
            }, canExecute: o => true);
        }

        public void UpdateData()
        {
            AllProducts = ProductDataWorker.GetShortProducts().ToList();

            AllProductsInDelivery
                .Select(p => p.SKU)
                .Except(AllProducts
                    .Select(p => p.SKU))
                .ToList()
                .ForEach(rp
                    => AllProductsInDelivery
                        .Remove(AllProductsInDelivery
                            .Single(p => p.SKU == rp)));
        }

        private void InvokeListUpdate()
        {
            var buffer = AllProductsInDelivery.ToArray();
            AllProductsInDelivery.Clear();
            AllProductsInDelivery = buffer.ToList();
        }
    }
}
