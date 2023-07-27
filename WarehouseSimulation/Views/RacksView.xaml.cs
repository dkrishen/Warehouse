using System.Windows;
using System.Windows.Controls;
using WarehouseSimulation.ViewModels;

namespace WarehouseSimulation.Views
{
    /// <summary>
    /// Interaction logic for AddRackView.xaml
    /// </summary>
    public partial class RacksView : UserControl
    {
        public RacksView()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is RacksViewModel racksViewModel)
            {
                racksViewModel.UpdateData();
            }
        }
    }
}
