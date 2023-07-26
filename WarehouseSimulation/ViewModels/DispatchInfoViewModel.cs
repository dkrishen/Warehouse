using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Core;

namespace WarehouseSimulation.ViewModels
{
    public class DispatchInfoViewModel : ViewModelBase
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

        public RelayCommand RemoveDispatchCommand { get; set; }
        public RelayCommand ApproveDispatchCommand { get; set; }
        public RelayCommand NavigateToPreviousViewCommand { get; set; }

        public DispatchInfoViewModel(INavigationServices navService)
        {
            Navigation = navService;
            NavigateToPreviousViewCommand = new RelayCommand(o =>
            {
                Navigation.ToBack();
            }, canExecute: o => true);
            ApproveDispatchCommand = new RelayCommand(o =>
            {
                // TODO:

                NavigateToPreviousViewCommand.Execute(true);
            }, canExecute: o => true);
            RemoveDispatchCommand = new RelayCommand(o =>
            {
                // TODO:

                NavigateToPreviousViewCommand.Execute(true);
            }, canExecute: o => true);
        }
    }
}
