using System.Windows;
using System.Windows.Controls;
using WarehouseSimulation.ViewModels;

namespace WarehouseSimulation.Views
{
    /// <summary>
    /// Interaction logic for ProductLocationInfoView.xaml
    /// </summary>
    public partial class ProductLocationInfoView : UserControl
    {
        public ProductLocationInfoView()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProductLocaionInfoViewModel productLocaionInfoViewModel)
            {
                productLocaionInfoViewModel.UpdateData();
            }
        }
    }
}
