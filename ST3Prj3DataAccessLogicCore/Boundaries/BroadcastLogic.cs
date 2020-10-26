using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomaineCore.Models;
using InterfacesCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

/// <summary>
/// Metoder implemeteret som eksempler der overholder iDataAccessLogic intefacet
/// Ændringer signaturer og til tilføjelse af metoder (nye signature) skal ske
/// først skegennem ændringer i iDataAccessLogic interfacet
/// </summary>
namespace DataAccessLogicCore.Boundaries
{
    public class BroadcastLogic : IDataBroadcastLogic
    {
        private const int PORT = 11000;
        private const string ip = "192.168.1.255"; //Standard networking broadcast IP

        public void BroadcastSamplePack(SamplePack samplePack)
        {
            IPAddress broadcast = IPAddress.Parse(ip);
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            byte[] sendbuf = Encoding.ASCII.GetBytes(samplePack.ToString());
            IPEndPoint ep = new IPEndPoint(broadcast, PORT);

            s.SendTo(sendbuf, ep);

            Console.WriteLine("Message sent to the broadcast address");
        }
    }
}
