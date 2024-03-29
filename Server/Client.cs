using System.Net.Sockets;

namespace Server
{
    internal class Client
    {
        public string Name { get; set; }
        public Socket Socket { get; set; }
    }
}
