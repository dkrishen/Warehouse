using WarehouseSimulation.Core;
using WarehouseSimulation.Core.Services;

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

        public RelayCommand NavigateToWarehouseViewCommand { get; set; }

        public MainViewModel(INavigationServices navService)
        {
            Navigation = navService;

            NavigateToWarehouseViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<WarehouseViewModel>();
            }, canExecute: o => true);

            NavigateToWarehouseViewCommand.Execute(true);
        }
    }
}
