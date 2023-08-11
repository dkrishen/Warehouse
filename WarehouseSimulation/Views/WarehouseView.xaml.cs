using System.Windows;
using System.Windows.Controls;
using WarehouseSimulation.ViewModels;

namespace WarehouseSimulation.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WarehouseView : UserControl
    {
        public WarehouseView()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is WarehouseViewModel warehouseViewModel)
            {
                warehouseViewModel.UpdateData();
            }
        }

        private void ProductDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataContext is WarehouseViewModel warehouseViewModel)
            {
                warehouseViewModel.ViewLocations();
            }
        }
    }
}
