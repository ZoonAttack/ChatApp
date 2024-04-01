using System.Net;
using System.Net.Sockets;
using Communicator;
namespace Server
{
    public partial class ServerHome : Form
    {
        Socket serverSocket;


        bool flag = false;
        bool isReading = true;
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
            while (flag)
            {
                Socket clientSocket = null;
                //try
                //{
                serverSocket.Listen(0);
                try
                {
                    clientSocket = serverSocket.Accept();
                }
                catch(SocketException ex) { MessageBox.Show(ex.Message); }
                Client client = new Client(clientSocket, $"Guest_{Random.Shared.Next()}");
                clients.Add(client);
                Thread clientThread = new Thread(() => ClientConnection(client));
                clientThread.Start();
                //}
                //catch (SocketException soex)
                //{
                //    serverSocket.Dispose();
                //    clients.RemoveAll(clients.Remove);
                //    UpdateUI(TB_Log, soex.Message);
                //}
            }
            serverSocket.Close();
        }
        private void ClientConnection(Client client)
        {
            byte[] buffer;
            int bytesRead = 1;
            while (isReading)
            {
                if (!Utility.SocketConnected(client.Socket))
                {
                    isReading = false;
                    return;
                }
                //try
                //{
                    buffer = new byte[sizeof(int)];
                    bytesRead = client.Socket.Receive(buffer);
                    if (bytesRead == 0) return;
                    if (bytesRead != buffer.Length) throw new InvalidDataException($"Got {bytesRead} Expected {buffer.Length}");

                    int size = BitConverter.ToInt32(buffer, 0);
                    buffer = new byte[size];
                    bytesRead = client.Socket.Receive(buffer);
                    if (bytesRead != buffer.Length) throw new InvalidDataException($"Got {bytesRead} Expected {buffer.Length}");

                    using var ms = new MemoryStream(buffer);
                    using var br = new BinaryReader(ms);
                    ActionType type = (ActionType)br.ReadInt16();
                    switch (type)
                    {
                        case ActionType.MESSAGE:
                            string messageReceived = br.ReadString();
                            UpdateUI(TB_Log, $"({client.Name}) sent: {messageReceived}{Environment.NewLine}");
                            break;
                        case ActionType.USERNAME:
                            messageReceived = br.ReadString();
                            client.Name = messageReceived;
                            UpdateUI(TB_Log, $"Set Client: {client.Name}'s name to {messageReceived}");
                            break;
                        case ActionType.DISCONNECTED:
                            messageReceived = br.ReadString();
                            client.Socket.Dispose();
                            clients.Remove(client);
                            UpdateUI(TB_Log, $"{client.Name} {messageReceived}");
                            return;
                    }
                //}
                //catch (SocketException soex)
                //{
                //    serverSocket.Dispose();
                //    clients.RemoveAll(clients.Remove);
                //    UpdateUI(TB_Log, soex.Message);
                //}
            }
        }
        private void ServerHome_FormClosing(object sender, FormClosingEventArgs e)
        {
            isReading = false;
            flag = false;
                ClientMessage message = new ClientMessage("Server has shutdown", ActionType.DISCONNECTED);
                foreach(Client client in  clients)
                {
                if (client.Socket?.Connected != true) return;
                    Utility.Send(client.Socket, message);
                }
              serverSocket?.Dispose();
        }
        private void UpdateUI(TextBox tb, string text)
        {
            if (this.Disposing || this.IsDisposed) return;
            if (tb.InvokeRequired)
            {
                tb.Invoke(new Action(() => UpdateUI(tb, text)));
            }
            else
            {
                tb.AppendText($"{text}{Environment.NewLine}");
            }

        }

        private void BroadcastMessage(Client sender, ClientMessage message)
        {
            foreach(Client client in clients)
            {
                if(client.Socket?.Connected != true) return;

                if(client == sender)
                {
                    Utility.Send(client.Socket, new ClientMessage(message.Message.Replace(sender.Name, "me")));
                }
                else
                {
                    Utility.Send(client.Socket, message);
                }
            }
        }

    }
}