using System.Windows;
using System.Windows.Controls;
using WarehouseSimulation.ViewModels;

namespace WarehouseSimulation.Views
{
    /// <summary>
    /// Interaction logic for DispatchesView.xaml
    /// </summary>
    public partial class DispatchesView : UserControl
    {
        public DispatchesView()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is DispatchesViewModel DispatchesViewModel)
            {
                DispatchesViewModel.UpdateData();
            }
        }
    }
}
