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
using WarehouseSimulation.ViewModels.Reports;

namespace WarehouseSimulation.Views
{
    /// <summary>
    /// Interaction logic for ExpensesReport.xaml
    /// </summary>
    public partial class ExpensesReportView : UserControl
    {
        public ExpensesReportView()
        {
            InitializeComponent();
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is ExpensesReportViewModel expensesReportViewModel)
            {
                expensesReportViewModel.UpdateData();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ExpensesReportViewModel expensesReportViewModel)
            {
                expensesReportViewModel.UpdateData();
            }
        }
    }
}
