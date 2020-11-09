﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDPListenerTestProject
{
    class Program
    {
        private const int PORT = 11000;
        private const string ip = "192.168.43.30";     //Se logbog for RPI ip adresser fra 4/11. "192.168.43.255"

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
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
        }
    }
}