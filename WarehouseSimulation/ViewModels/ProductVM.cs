using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using WarehouseSimulation.Models;
using WarehouseSimulation.Models.Data;
using WarehouseSimulation.Models.ViewModels;
using WarehouseSimulation.Views;

namespace WarehouseSimulation.ViewModels
{
    internal class ProductVM : INotifyPropertyChanged
    {
        // Data Methods
        private List<ProductViewDto> allProducts = DataWorker.GetProductsCountInfo().ToList();
        public List<ProductViewDto> AllProducts
        {
            get { return allProducts; }
            set { allProducts = value; NotifyPropertyChanged("allProducts"); }
        }

        // Navigation Methods
        private RelayCommand openDeliveriesWindow;
        public RelayCommand OpenDeliveriesWindow
        {
            get
            {
                return openDeliveriesWindow ?? new RelayCommand(obj =>
                {
                    openDeliveriesWindowMethod();
                });
            }
        }

        private void openDeliveriesWindowMethod()
        {
            DeliveriesWindow deliveriesWindow = new DeliveriesWindow();
            SetCenterPositionAndOpen(deliveriesWindow);
        }

        private void SetCenterPositionAndOpen(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        // 
        private void NotifyPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
