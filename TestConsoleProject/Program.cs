using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using BusinessLogicCore.Controller;
using DataAccessLogicCore.Boundaries;
using DataAccessLogicCore.Data;
using InterfacesCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestConsoleProject
{
    class Program
    {
        //private const int PORT = 11000;
        //private const string ip = "192.168.43.255"; //Standard networking broadcast IP  

        //static void Main(string[] args)
        //{
        //    //BroadcastLogic broadcastLogic = new BroadcastLogic();

        //    //BPLogic bpLogic = new BPLogic();

        //    //broadcastLogic.BroadcastSamplePack(bpLogic.ReadAdc());
        //    while (true)
        //    {
        //        Console.WriteLine("Ready to broadcast");
        //        BroadcastMessage(Console.ReadLine());
        //    }

        //    void BroadcastMessage(String message)
        //    {
        //        IPAddress broadcast = IPAddress.Parse(ip);
        //        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        //        byte[] sendbuf = Encoding.ASCII.GetBytes(message);
        //        //byte[] sendbufhundred = new byte[100];
        //        //sendbuf.CopyTo(sendbufhundred,0);
        //        IPEndPoint ep = new IPEndPoint(broadcast, PORT);

        //        s.SendTo(sendbuf, ep);

        //        Console.WriteLine("Message sent to the broadcast address");
        //    }



        //}

        private static ServiceProvider serviceProvider;
        public static SamplePackDbContext dbContext; //Using an explicitly defined instance
        IBusinessLogic currentBl; //Using an inteface defines implementation

        //public Program(SamplePackDbContext dbContext, IBusinessLogic bl)
        //{
        //    this.dbContext = dbContext;
        //    this.currentBl = bl;
        //    GetSamplePacks();
        //}
        static void Main(string[] args)
        {
            ServiceCollection services = new ServiceCollection();//Start a DI Container (Dependency Injection Container)
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();

         


            

            //_ = new Program();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IDataAccessLogic, CtrlDataAccessLogic>(); //Add an instance a iDataAccesLogic implementaion to the DI Container
            services.AddSingleton<IBusinessLogic, BPLogic>(); //Add anusing ST3Prj3DataAccessLogicCore.Data; instance a iBusinessLogiciBusinessLogic implementaion to the DI Container
            services.AddDbContext<SamplePackDbContext>(options =>
            {
                options.UseSqlite("Data Source = SamplePack.db");//This options may be smarter to set in DbContext
            });

            //services.AddSingleton<MainWindow>();
        }
        private void OnStartup()
        {
            var program = serviceProvider.GetService<Program>();
        }

        public void GetSamplePacks()
        {
            var samplePack = dbContext.SamplePack.ToList(); //Get some data from DB
            //EmployeeDG.ItemsSource = employees; //Put data into a DataGrid
            Console.WriteLine(samplePack.ToString());
        }
    }
}
