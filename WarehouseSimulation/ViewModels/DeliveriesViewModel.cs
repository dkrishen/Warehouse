﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSimulation.Core;
using WarehouseSimulation.Core.Services;

namespace WarehouseSimulation.ViewModels
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

        public RelayCommand NavigateToAddDeliveryViewCommand { get; set; }
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
        }
    }
}
