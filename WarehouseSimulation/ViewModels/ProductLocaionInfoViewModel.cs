using System.Collections.Generic;
using System.Linq;
using WarehouseSimulation.Core;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Data;
using WarehouseSimulation.Models.ViewModels;

namespace WarehouseSimulation.ViewModels
{
    public class ProductLocaionInfoViewModel : ViewModelBase
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

        private List<ProductLocationViewDto> _AllLocations;
        public List<ProductLocationViewDto> AllLocations
        {
            get { return _AllLocations; }
            set { _AllLocations = value; OnPropertyChanged("AllLocations"); }
        }

        public RelayCommand NavigateToPreviousViewCommand { get; set; }
        public string? ProductSku { get; set; }

        public ProductLocaionInfoViewModel(INavigationServices navService)
        {
            ProductSku = GlobalVariables.SelectedProductSku;
            Navigation = navService;

            NavigateToPreviousViewCommand = new RelayCommand(o =>
            {
                Navigation.ToBack();
            }, canExecute: o => true);

            if (ProductSku == null)
            {
                NavigateToPreviousViewCommand.Execute(true);
            }
            else
            {
                UpdateLocations();
            }
        }

        public void UpdateLocations()
        {
            var locations = RackDataWorker.GetRacksByProduct(ProductSku)
                .Select(rack => new ProductLocationViewDto
                {
                    Title = rack.Number.ToString(),
                    Count = RackDataWorker.GetProductsCountInRack(rack.Number, ProductSku)
                }).ToList();
            var countInSump = RackDataWorker.GetProductsCountInSump(ProductSku);
            if (countInSump != 0)
            {
                locations.Add(new ProductLocationViewDto
                {
                    Title = GlobalVariables.SumpTitle,
                    Count = countInSump
                });
            }

            AllLocations = locations;
        }

        public void UpdateData()
        {
            ProductSku = GlobalVariables.SelectedProductSku;
            UpdateLocations();
            GlobalVariables.SelectedProductSku = null;
        }

    }
}
