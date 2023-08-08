using System;
using System.Windows;
using System.Windows.Controls;
using WarehouseSimulation.ViewModels;

namespace WarehouseSimulation.Views
{
    /// <summary>
    /// Interaction logic for AddDeliveryView.xaml
    /// </summary>
    public partial class AddDeliveryView : UserControl
    {
        public AddDeliveryView()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddDeliveryViewModel addDeliveryViewModel)
            {
                addDeliveryViewModel.UpdateData();
            }
        }
    }
}
