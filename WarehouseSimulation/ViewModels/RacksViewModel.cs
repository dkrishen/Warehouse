using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Core;

namespace WarehouseSimulation.ViewModels
{
    public class RacksViewModel : ViewModelBase
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
        public RelayCommand NavigateToTypesViewCommand { get; set; }
        public RelayCommand AddRackCommand { get; set; }

        public RacksViewModel(INavigationServices navService)
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
            AddRackCommand = new RelayCommand(o =>
            {
                // TODO:
            }, canExecute: o => true);
        }

    }
}
