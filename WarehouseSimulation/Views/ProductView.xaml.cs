using System.Windows;
using System.Windows.Controls;
using WarehouseSimulation.ViewModels;

namespace WarehouseSimulation.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ProductView : UserControl
    {
        public ProductView()
        {
            InitializeComponent();
            //DataContext = new ProductViewModel();
        }
    }
}
