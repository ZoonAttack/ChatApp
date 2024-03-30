using System.Net.Sockets;
namespace Communicator
{
    public class Utility
    {
        public static bool SocketConnected(Socket s)
        {
            bool poll = s.Poll(0, SelectMode.SelectRead);
            bool available = (s.Available == 0);
            return (poll && available) ? false : true;
        }
        public static void Send(Socket socket, ClientMessage message)
        {
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            int size = 0;
            //Write initial size in bytes(4)
            bw.Write(size);
            //Write Message type in bytes(2)
            bw.Write((short)message.Type);
            //Write actual message in bytes(1 * message.Length)
            bw.Write(message.Message);
            //Go back to the size
            bw.Seek(0, SeekOrigin.Begin);
            //Write actual size
            bw.Write((int)bw.BaseStream.Length - 4);
            socket.Send(ms.ToArray());
        }
    }
}
