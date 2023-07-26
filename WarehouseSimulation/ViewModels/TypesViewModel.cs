using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Core;
using System.Reflection;

namespace WarehouseSimulation.ViewModels
{
    public class TypesViewModel : ViewModelBase
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
        public RelayCommand AddTypeCommand { get; set; }

        public TypesViewModel(INavigationServices navService)
        {
            Navigation = navService;

            NavigateToPreviousViewCommand = new RelayCommand(o =>
            {
                Navigation.ToBack();
            }, canExecute: o => true);
            AddTypeCommand = new RelayCommand(o =>
            {
                // TODO:
            }, canExecute: o => true);
        }
    }
}
