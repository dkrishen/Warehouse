using System.Collections.Generic;
using System.Linq;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Core;
using WarehouseSimulation.Models.ViewModels;
using WarehouseSimulation.Data;

namespace WarehouseSimulation.ViewModels
{
    public class ProductsViewModel : ViewModelBase
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
            set { _AllProducts = value; OnPropertyChanged("AllProducts"); }
        }

        private List<string> _AllTypes = TypeDataWorker.GetTypeNames().ToList();
        public List<string> AllTypes
        {
            get { return _AllTypes; }
            set { _AllTypes = value; OnPropertyChanged("AllTypes"); }
        }

        public ProductViewDto SelectedProduct { get; set; }
        public string SelectedType { get; set; }
        public string NewProductSku { get; set; }
        public string NewProductCost { get; set; }

        public RelayCommand NavigateToPreviousViewCommand { get; set; }
        public RelayCommand NavigateToTypesViewCommand { get; set; }
        public RelayCommand AddProductCommand { get; set; }
        public RelayCommand RemoveProductCommand { get; set; }

        public ProductsViewModel(INavigationServices navService)
        {
            Navigation = navService;
            NavigateToPreviousViewCommand = new RelayCommand(o =>
            {
                Navigation.ToBack();
            }, canExecute: o => true);
            NavigateToTypesViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<TypesViewModel>();
            }, canExecute: o => true);
            AddProductCommand = new RelayCommand(o =>
            {
                try
                {
                    var newCost = int.Parse(NewProductCost);

                    if (SelectedType != null
                        && NewProductSku != null
                        && NewProductSku.Replace(" ", "").Length != 0
                        && newCost > 0
                        && ProductDataWorker.AddProduct(new ProductViewDto
                        {
                            SKU = NewProductSku,
                            Cost = newCost,
                            Type = SelectedType
                        }))
                    {
                        AllProducts = ProductDataWorker.GetShortProducts().ToList();
                    }
                }
                catch { }
            }, canExecute: o => true);
            RemoveProductCommand = new RelayCommand(o =>
            {
                if (SelectedProduct != null
                    && ProductDataWorker.RemoveProduct(SelectedProduct.SKU))
                {
                    AllProducts = ProductDataWorker.GetShortProducts().ToList();
                }
            }, canExecute: o => true);
        }

        public void UpdateData()
        {
            AllProducts = ProductDataWorker.GetShortProducts().ToList();
            AllTypes = TypeDataWorker.GetTypeNames().ToList();
        }

    }
}
