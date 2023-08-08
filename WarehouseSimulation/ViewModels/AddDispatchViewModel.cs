using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Core;
using WarehouseSimulation.Models.ViewModels;
using WarehouseSimulation.Data;

namespace WarehouseSimulation.ViewModels
{
    public class AddDispatchViewModel : ViewModelBase
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

        private List<ProductViewDto> _AllProducts = ProductDataWorker.GetShortProducts().ToList();
        public List<ProductViewDto> AllProducts
        {
            get { return _AllProducts; }
            set
            {
                _AllProducts = value;
                OnPropertyChanged("AllProducts");
                OnPropertyChanged("AllProductsInDispatch");
                OnPropertyChanged("AllNotAddedProducts");
            }
        }

        private List<ProductViewDto> _AllProductsInDispatch = new List<ProductViewDto>();
        public List<ProductViewDto> AllProductsInDispatch
        {
            get { return _AllProductsInDispatch; }
            set
            {
                _AllProductsInDispatch = value;
                OnPropertyChanged("AllProductsInDispatch");
                OnPropertyChanged("AllNotAddedProducts");
            }
        }

        public List<string> AllNotAddedProducts
        {
            get
            {
                return _AllProducts
                    .Select(p => p.SKU)
                    .Except(AllProductsInDispatch
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
        public RelayCommand AddDispatchCommand { get; set; }

        public AddDispatchViewModel(INavigationServices navService)
        {
            Navigation = navService;
            NavigateToPreviousViewCommand = new RelayCommand(o =>
            {
                Navigation.ToBack();
            }, canExecute: o => true);
            AddProductCommand = new RelayCommand(o =>
            {
                try
                {
                    var productCount = int.Parse(AddedProductCount);
                    if (SelectedProductForAdd != null
                        && SelectedProductForAdd.Replace(" ", "").Length != 0
                        && productCount > 0
                        && !AllProductsInDispatch.Select(p => p.SKU)
                            .Contains(SelectedProductForAdd))
                    {
                        var newProduct = AllProducts
                            .Single(p => p.SKU == SelectedProductForAdd);
                        newProduct.Count = productCount;

                        AllProductsInDispatch.Add(newProduct);
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
                        && AllProductsInDispatch.Select(p => p.SKU)
                            .Contains(SelectedProductForRemove.SKU))
                    {
                        AllProductsInDispatch.Remove(SelectedProductForRemove);
                        UpdateData();
                        InvokeListUpdate();
                    }
                }
                catch { }
            }, canExecute: o => true);
            AddDispatchCommand = new RelayCommand(o =>
            {
                if (AllProductsInDispatch.Count != 0
                    && DispatchDataWorker.CreateDispatch(AllProductsInDispatch, DateTime.UtcNow))
                {
                    AllProductsInDispatch.Clear();
                    SelectedProductForAdd = null;
                    AddedProductCount = null;
                    NavigateToPreviousViewCommand.Execute(true);
                }
            }, canExecute: o => true);
        }

        public void UpdateData()
        {
            AllProducts = ProductDataWorker.GetShortProducts().ToList();

            AllProductsInDispatch
                .Select(p => p.SKU)
                .Except(AllProducts
                    .Select(p => p.SKU))
                .ToList()
                .ForEach(rp
                    => AllProductsInDispatch
                        .Remove(AllProductsInDispatch
                            .Single(p => p.SKU == rp)));
        }

        private void InvokeListUpdate()
        {
            var buffer = AllProductsInDispatch.ToArray();
            AllProductsInDispatch.Clear();
            AllProductsInDispatch = buffer.ToList();
        }
    }
}
