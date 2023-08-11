using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Core;
using System.Collections.Generic;
using WarehouseSimulation.Data;
using WarehouseSimulation.Models.ViewModels;
using System.Linq;

namespace WarehouseSimulation.ViewModels
{
    public class DispatchesViewModel : ViewModelBase
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

        private List<DispatchViewDto> _AllDispatches = DispatchDataWorker.GetShortDispatches().ToList();
        public List<DispatchViewDto> AllDispatches
        {
            get { return _AllDispatches; }
            set
            {
                _AllDispatches = value;
                OnPropertyChanged("AllDispatches");
            }
        }

        private DispatchViewDto _SelectedDispatch { get; set; }
        public DispatchViewDto SelectedDispatch
        {
            get { return _SelectedDispatch; }
            set { _SelectedDispatch = value; GlobalVariables.SelectedDispatchId = _SelectedDispatch?.DispatchId; }
        }

        public RelayCommand NavigateToAddDispatchViewCommand { get; set; }
        public RelayCommand NavigateToDispatchInfoViewCommand { get; set; }
        public RelayCommand NavigateToPreviousViewCommand { get; set; }

        public DispatchesViewModel(INavigationServices navService)
        {
            Navigation = navService;
            NavigateToAddDispatchViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<AddDispatchViewModel>();
            }, canExecute: o => true);
            NavigateToDispatchInfoViewCommand = new RelayCommand(o =>
            {
                if (SelectedDispatch != null)
                {
                    GlobalVariables.SelectedDispatchId = SelectedDispatch.DispatchId;
                    Navigation.NavigateTo<DispatchInfoViewModel>();
                }
            }, canExecute: o => true);
            NavigateToPreviousViewCommand = new RelayCommand(o =>
            {
                Navigation.ToBack();
            }, canExecute: o => true);
        }

        public void UpdateData()
        {
            SelectedDispatch = null;
            AllDispatches = DispatchDataWorker.GetShortDispatches().ToList();
        }

        public void ViewDetails()
        {
            NavigateToDispatchInfoViewCommand.Execute(this);
        }
    }
}
