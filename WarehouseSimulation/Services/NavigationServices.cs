﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSimulation.Core;
using WarehouseSimulation.ViewModels;
using WarehouseSimulation.Views;

namespace WarehouseSimulation.Services
{
    public class NavigationServices : ObservableObject, INavigationServices
    {
        private ViewModelBase _CurrentView;
        private Func<Type, ViewModelBase> _ViewModelFactory;

        public ViewModelBase CurrentView
        {
            get => _CurrentView;
            private set
            {
                _CurrentView = value;
                OnPropertyChanged();
            }
        }    

        public NavigationServices(Func<Type, ViewModelBase> viewModelFactory)
        {
            _ViewModelFactory = viewModelFactory;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            var viewModel = _ViewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = viewModel;
        }
    }
}
