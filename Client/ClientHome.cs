using System.Net;
using System.Net.Sockets;
using Communicator;
namespace Client
{
    public partial class ClientHome : Form
    {
        Socket clientSocket;
        public ClientHome()
        {
            InitializeComponent();
        }

        private void BTN_Connect_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocolType: ProtocolType.Tcp);
            IPEndPoint EndPoint = new IPEndPoint(IPAddress.Parse(TB_Address.Text), 8888);
            clientSocket.Connect(EndPoint);
            if(clientSocket.Connected)
            {
                ClientMessage message = new ClientMessage(TB_Username.Text, ActionType.USERNAME);
                Send(message);
            }
        }
        private void Send(ClientMessage message)
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
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
                    clientSocket.Send(ms.ToArray());
                }
            }
        }
    }
}