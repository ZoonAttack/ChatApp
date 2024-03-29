
using System.Net.Sockets;


namespace Communicator
{
    internal class Utility
    {
        public static bool SocketConnected(Socket s)
        {
            bool poll = s.Poll(0, SelectMode.SelectRead);
            bool available = (s.Available == 0);
            return (poll && available) ? false : true;
        }
    }
}
