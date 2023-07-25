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

        public RelayCommand NavigateToProductsViewCommand { get; set; }

        public MainViewModel(INavigationServices navService)
        {
            Navigation = navService;

            NavigateToProductsViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<ProductViewModel>();
            }, canExecute: o => true);

            NavigateToProductsViewCommand.Execute(true);
        }
    }
}
