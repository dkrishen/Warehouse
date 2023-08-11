using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WarehouseSimulation.ViewModels;

namespace WarehouseSimulation.Views
{
    /// <summary>
    /// Interaction logic for TransferReportView.xaml
    /// </summary>
    public partial class TransferReportView : UserControl
    {
        public TransferReportView()
        {
            InitializeComponent();
        }

        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is TransferReportViewModel transferReportViewModel)
            {
                transferReportViewModel.UpdateData();
            }
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is TransferReportViewModel transferReportViewModel)
            {
                transferReportViewModel.UpdateData();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is TransferReportViewModel transferReportViewModel)
            {
                transferReportViewModel.UpdateData();
            }
        }
    }
}
