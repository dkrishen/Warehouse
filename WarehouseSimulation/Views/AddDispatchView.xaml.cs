using System.Windows;
using System.Windows.Controls;
using WarehouseSimulation.ViewModels;

namespace WarehouseSimulation.Views
{
    /// <summary>
    /// Interaction logic for AddDispatchView.xaml
    /// </summary>
    public partial class AddDispatchView : UserControl
    {
        public AddDispatchView()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddDispatchViewModel addDispatchViewModel)
            {
                addDispatchViewModel.UpdateData();
            }
        }
    }
}
