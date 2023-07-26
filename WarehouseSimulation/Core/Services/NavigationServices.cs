using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSimulation.Core;
using WarehouseSimulation.ViewModels;
using WarehouseSimulation.Views;

namespace WarehouseSimulation.Core.Services
{
    public class NavigationServices : ObservableObject, INavigationServices
    {
        private Stack<ViewModelBase> _ViewHistory;
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
            _ViewHistory = new Stack<ViewModelBase>();
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            var viewModel = _ViewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = viewModel;
            _ViewHistory.Push(CurrentView);
        }

        public void NavigateBack()
        {
            _ViewHistory.Pop();
            CurrentView = _ViewHistory.Peek();
        }
    }
}
