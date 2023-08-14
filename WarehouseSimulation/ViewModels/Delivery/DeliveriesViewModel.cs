using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WarehouseSimulation.Core;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Data;
using WarehouseSimulation.Models.ViewModels;

namespace WarehouseSimulation.ViewModels.Delivery
{
    public class DeliveriesViewModel : ViewModelBase
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

        private List<DeliveryViewDto> _AllDeliveries = DeliveryDataWorker.GetShortDeliveries().ToList();
        public List<DeliveryViewDto> AllDeliveries
        {
            get { return _AllDeliveries; }
            set
            {
                _AllDeliveries = value;
                OnPropertyChanged("AllDeliveries");
            }
        }

        private DeliveryViewDto _SelectedDelivery { get; set; }
        public DeliveryViewDto SelectedDelivery
        {
            get { return _SelectedDelivery; }
            set { _SelectedDelivery = value; GlobalVariables.SelectedDeliveryId = _SelectedDelivery?.DeliveryId; }
        }

        public RelayCommand NavigateToAddDeliveryViewCommand { get; set; }
        public RelayCommand NavigateToDeliveryInfoViewCommand { get; set; }
        public RelayCommand NavigateToPreviousViewCommand { get; set; }

        public DeliveriesViewModel(INavigationServices navService)
        {
            Navigation = navService;
            NavigateToPreviousViewCommand = new RelayCommand(o =>
            {
                Navigation.ToBack();
            }, canExecute: o => true);
            NavigateToAddDeliveryViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<AddDeliveryViewModel>();
            }, canExecute: o => true);
            NavigateToDeliveryInfoViewCommand = new RelayCommand(o =>
            {
                if (SelectedDelivery != null)
                {
                    GlobalVariables.SelectedDeliveryId = SelectedDelivery.DeliveryId;
                    Navigation.NavigateTo<DeliveryInfoViewModel>();
                }
            }, canExecute: o => true);
        }

        public void UpdateData()
        {
            SelectedDelivery = null;
            AllDeliveries = DeliveryDataWorker.GetShortDeliveries().ToList();
        }

        public void ViewDetails()
        {
            NavigateToDeliveryInfoViewCommand.Execute(this);
        }
    }
}
