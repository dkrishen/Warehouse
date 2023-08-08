using System.Windows;
using System.Windows.Controls;
using WarehouseSimulation.ViewModels;

namespace WarehouseSimulation.Views
{
    /// <summary>
    /// Interaction logic for DeliveriesPage.xaml
    /// </summary>
    public partial class DeliveriesView : UserControl
    {
        public DeliveriesView()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is DeliveriesViewModel DeliveriesViewModel)
            {
                DeliveriesViewModel.UpdateData();
            }
        }
    }
}
