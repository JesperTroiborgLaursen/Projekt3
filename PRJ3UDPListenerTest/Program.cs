using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;


namespace PRJ3UDPListenerTest
{
    class Program
    {
        private const int PORT = 11000;
        private const string ip = "192.168.43.30";     //Se logbog for RPI ip adresser fra 4/11. "192.168.43.255"
        static void Main(string[] args)
        {
            #region Send

            //while (true)
            //{
            //    Console.WriteLine("Ready to broadcast");

            //    List<short> testList = new List<short>();

            //    for (int i = 0; i < 51; i++)
            //    {
            //        short nb = (short)i;
            //        testList.Add(nb);
            //    }
            //    IPAddress broadcast = IPAddress.Parse(ip);
            //    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);


            //    byte[] sendbuf = Encoding.ASCII.GetBytes(testList.ToString());
            //    IPEndPoint ep = new IPEndPoint(broadcast, PORT);


            //    while (!Console.KeyAvailable)
            //    {
            //        s.SendTo(sendbuf, ep);
            //    }

            //    Console.WriteLine("Sendt pakke");

            //}
            #endregion


            #region Listen

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 11000);
            sock.Bind(iep);
            EndPoint ep = (EndPoint)iep;
            Console.WriteLine("Ready to receive...");

            while (true)
            {
                byte[] data = new byte[1024];
                int recv = sock.ReceiveFrom(data, ref ep);
                string stringData = Encoding.ASCII.GetString(data, 0, recv);
                Console.WriteLine("received: {0}  from: {1}", stringData, ep.ToString());
            }

            #endregion

            //while((bool)Console.ReadKey())
            //    sock.Close();
        }
    }
}
