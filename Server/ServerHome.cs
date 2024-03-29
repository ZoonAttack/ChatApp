using System.Net;
using System.Net.Sockets;

namespace Server
{
    public partial class ServerHome : Form
    {
        Socket serverSocket;
        public ServerHome()
        {
            InitializeComponent();
        }

        private void BTN_Start_Click(object sender, EventArgs e)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);
            serverSocket.Bind(EndPoint);
            TB_Log.AppendText($"({DateTime.Now.Date}) Server has started with address: {EndPoint.Address}{Environment.NewLine}");
        }
    }
}