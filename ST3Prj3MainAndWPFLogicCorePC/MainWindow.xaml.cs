using System.Linq;
using System.Windows;
using DataAccessLogicCore.Data;
using InterfacesCore;

namespace MainAndWPFLogicCorePC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// This tutorial using DI container and WPF has been used https://executecommands.com/dependency-injection-in-wpf-net-core-csharp/
    /// </summary>
   
    public partial class MainWindow : Window
    {
        EmployeeDbContext dbContext; //Using an explicitly defined instance
        iBusinessLogic currentBl; //Using an inteface defines implementation

        public MainWindow(EmployeeDbContext dbContext,iBusinessLogic bl ) //Injected by DI Container
        {
            this.dbContext = dbContext;
            this.currentBl = bl;
            InitializeComponent();//Standard WPF
            GetEmployees(); //Specific data initialization
        }

        private void GetEmployees()
        {
            var employees = dbContext.Employee.ToList(); //Get some data from DB
            EmployeeDG.ItemsSource = employees; //Put data into a DataGrid
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int val = currentBl.DoAnAlogrithm();
        }
    }
}
