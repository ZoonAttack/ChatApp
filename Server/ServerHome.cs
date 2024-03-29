using System.Net;
using System.Net.Sockets;
using Communicator;
namespace Server
{
    public partial class ServerHome : Form
    {
        Socket serverSocket;


        bool flag = false;

        List<Client> clients = new List<Client>();
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
            flag = true;
            Thread serverThread = new Thread(Listening);
            serverThread.Start();
        }
        private void Listening()
        {
            while(flag)
            {
                serverSocket.Listen(0);
                Socket clientSocket = serverSocket.Accept();
                Client client = new Client(clientSocket, $"Guest_{Random.Shared.Next()}");
                clients.Add(client);
                Thread clientThread = new Thread(() => ClientConnection(client));
                clientThread.Start();
            }
        }
        private void ClientConnection(Client client)
        {
            byte[] buffer;
            int bytesRead = 1;
            do
            {
                using (var ms = new MemoryStream())
                {
                    using (var br = new BinaryReader(ms))
                    {
                        if (Utility.SocketConnected(client.Socket))
                        {
                            try
                            {
                                buffer = new byte[sizeof(int)];
                                bytesRead = client.Socket.Receive(buffer);
                                if (bytesRead != buffer.Length) throw new InvalidDataException($"Got {bytesRead} Expected {buffer.Length}");

                                int size = BitConverter.ToInt32(buffer, 0);
                                buffer = new byte[size];
                                bytesRead = client.Socket.Receive(buffer);
                                if (bytesRead != buffer.Length) throw new InvalidDataException($"Got {bytesRead} Expected {buffer.Length}");

                                ActionType type = (ActionType)br.ReadInt16();
                                switch (type)
                                {
                                    case ActionType.MESSAGE:
                                        string messageReceived = br.ReadString();
                                        TB_Log.AppendText($"({client.Name}) sent: {messageReceived}{Environment.NewLine}");
                                        break;
                                    case ActionType.USERNAME:
                                        messageReceived = br.ReadString();
                                        client.Name = messageReceived;
                                        TB_Log.AppendText($"Set Client: {client.Name}'s name to {messageReceived}");
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                        }
                        else
                        {
                            TB_Log.AppendText($"Client({client.Name}) has disconnected from the server");
                            clients.Remove(client);

                        }
                    }
                }


            } while (bytesRead > 0);

        }
    }
}