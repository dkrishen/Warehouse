using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using WarehouseSimulation.Core;
using WarehouseSimulation.Models.Data;
using WarehouseSimulation.Models.ViewModels;
using WarehouseSimulation.Services;
using WarehouseSimulation.Views;

namespace WarehouseSimulation.ViewModels
{
    internal class ProductViewModel : ViewModelBase
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

        public RelayCommand NavigateToDeliveriesViewCommand { get; set; }

        public ProductViewModel(INavigationServices navService)
        {
            Navigation = navService;
            NavigateToDeliveriesViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<DeliveriesViewModel>();
            }, canExecute: o => true);
        }

        // Data Methods
        //private List<ProductViewDto> allProducts = DataWorker.GetProductsCountInfo().ToList();
        //public List<ProductViewDto> AllProducts
        //{
        //    get { return allProducts; }
        //    set { allProducts = value; NotifyPropertyChanged("allProducts"); }
        //}

        // Navigation Methods
        //private RelayCommand openDeliveriesWindow;
        //public RelayCommand OpenDeliveriesWindow
        //{
        //    //get
        //    //{
        //    //    //return openDeliveriesWindow ?? new RelayCommand(obj =>
        //    //    //{
        //    //    //    openDeliveriesWindowMethod();
        //    //    //});
        //    //}
        //}

        //private void openDeliveriesWindowMethod()
        //{
        //    DeliveriesWindow deliveriesWindow = new DeliveriesWindow();
        //    SetCenterPositionAndOpen(deliveriesWindow);
        //}

        //private void SetCenterPositionAndOpen(Window window)
        //{
        //    window.Owner = Application.Current.MainWindow;
        //    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        //    window.ShowDialog();
        //}

        // 
        //private void NotifyPropertyChanged(string propertyName)
        //{
        //    if(PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        //public event PropertyChangedEventHandler? PropertyChanged;

    }
}
