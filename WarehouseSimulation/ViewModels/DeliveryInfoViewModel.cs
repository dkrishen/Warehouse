using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Core;
using WarehouseSimulation.Data;
using WarehouseSimulation.Models.ViewModels;

namespace WarehouseSimulation.ViewModels
{
    public class DeliveryInfoViewModel : ViewModelBase
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

        private List<ProductViewDto> _AllProducts { get; set; }
        public List<ProductViewDto> AllProducts
        {
            get { return _AllProducts; }
            set { _AllProducts = value; OnPropertyChanged("AllProducts"); }
        }

        public RelayCommand RemoveDeliveryCommand { get; set; }
        public RelayCommand ApproveDeliveryCommand { get; set; }
        public RelayCommand NavigateToPreviousViewCommand { get; set; }
        public Guid? DeliveryId { get; set; }

        public DeliveryInfoViewModel(INavigationServices navService)
        {
            DeliveryId = GlobalVariables.SelectedDeliveryId;
            Navigation = navService;

            NavigateToPreviousViewCommand = new RelayCommand(o =>
            {
                Navigation.ToBack();
            }, canExecute: o => true);
            ApproveDeliveryCommand = new RelayCommand(o =>
            {
                var result = DeliveryDataWorker.ApproveDelivery(DeliveryId ?? Guid.Empty, DateTime.UtcNow);
                if (result.IsSuccessfully)
                {
                    NavigateToPreviousViewCommand.Execute(true);
                    result.Show();
                }
            }, canExecute: o => true);
            RemoveDeliveryCommand = new RelayCommand(o =>
            {
                DeliveryDataWorker.RemoveDelivery(DeliveryId ?? Guid.Empty);
                NavigateToPreviousViewCommand.Execute(true);
            }, canExecute: o => true);

            if (DeliveryId == null)
            {
                NavigateToPreviousViewCommand.Execute(true);
            }
            else
            {
                AllProducts = DeliveryDataWorker.GetProductsByDeliveryId(DeliveryId ?? Guid.Empty).ToList();
            }
        }

        public void UpdateData()
        {
            DeliveryId = GlobalVariables.SelectedDeliveryId;
            AllProducts = DeliveryDataWorker.GetProductsByDeliveryId(DeliveryId ?? Guid.Empty).ToList();
            GlobalVariables.SelectedDeliveryId = null;
        }
    }
}