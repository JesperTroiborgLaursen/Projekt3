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
using AppMainWPFPC.Data;

namespace AppMainWPFPC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EmployeeDbContext dbContext;

        public MainWindow(EmployeeDbContext dbContext)
        {
            this.dbContext = dbContext;
            InitializeComponent();
            GetEmployees();
        }

        private void GetEmployees()
        {
            var employees = dbContext.Employee.ToList();
            EmployeeDG.ItemsSource = employees;
        }
    }
}
