using System.Net.Sockets;

namespace Server
{
    internal class Client
    {
        public Client(Socket socket, string name)
        {
            Name = name;
            Socket = socket;
        }

        public string Name { get; set; }
        public Guid ID { get; init; } = Guid.NewGuid();
        public Socket Socket { get; set; }
    }
}
