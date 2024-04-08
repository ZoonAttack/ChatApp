using System.Diagnostics;
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
            serverSocket.ReceiveBufferSize = 128;
            serverSocket.SendBufferSize = 128;
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
                catch
                {
                    //Ignore
                }
                Client client = new Client(clientSocket, $"Guest_{Random.Shared.Next()}");
                client.Socket.ReceiveBufferSize = 128;
                client.Socket.SendBufferSize = 128;
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
                int dataRemaining = buffer.Length;
                int offset = 0;
                while(dataRemaining > 0)
                {
                     bytesRead = client.Socket.Receive(buffer, offset, dataRemaining, SocketFlags.None);
                     dataRemaining -= bytesRead;
                     offset += bytesRead;
                }
                if (bytesRead != buffer.Length) throw new InvalidDataException($"Got {bytesRead} Expected {buffer.Length}");
                using var ms = new MemoryStream(buffer);
                using var br = new BinaryReader(ms);
                ActionType type = (ActionType)br.ReadInt16();
                switch (type)
                {
                    case ActionType.MESSAGE:
                        string messageReceived = br.ReadString();
                        UpdateUI(TB_Log, $"({client.Name}) sent: {messageReceived}{Environment.NewLine}");
                        Broadcast(client, $"({client.Name}) sent: {messageReceived}");
                        break;
                    //case ActionType.USERNAME:
                    //    messageReceived = br.ReadString();
                    //    client.Name = messageReceived;
                    //    UpdateUI(TB_Log, $"Set Client: {client.Name}'s name to {messageReceived}");
                    //    Broadcast(client, $"({client.Name}) Has entered the chat! Say HI");
                    //    break;
                    case ActionType.USERCONNECTED:
                        UpdateUI(TB_Log, $"({client.Name}) has connected");
                        messageReceived = br.ReadString();
                        client.Name = messageReceived;
                        UpdateUI(TB_Log, $"Set Client: {client.Name}'s name to {messageReceived}");
                        SendList(client);

                        //Tell other clients a user has just joined!
                        Broadcast(client, client.Name, ActionType.USERCONNECTED);

                        Broadcast(client, $"({client.Name}) Has entered the chat! Say HI");
                        break;
                    case ActionType.USERDISCONNECTED:
                        UpdateUI(TB_Log, $"({client.Name}) has disconnected");

                        //Tell other clients a user has just left!
                        Broadcast(client, client.Name, ActionType.USERDISCONNECTED);
                        clients.Remove(client);

                        //client.Socket.Dispose();
                        //Broadcast(client, $"({client.Name}) has disconnected");
                        return;
                    default:
                        Debug.WriteLine("Something hit");
                        break;
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
            ClientMessage message = new ClientMessage("Server has shutdown", ActionType.SERVERDISCONNECTED);
            foreach (Client client in clients)
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
                tb.AppendText($"{text.Trim()}");
                tb.AppendText(Environment.NewLine);
            }

        }

        private void Broadcast(Client sender, string message, ActionType type = ActionType.MESSAGE)
        {
            foreach (Client client in clients)
            {
                if (client.Socket?.Connected != true) return;

                if (type == ActionType.USERCONNECTED && client.Name == sender.Name) continue;

                if (client.Name == sender.Name) 
                    Utility.Send(client.Socket, new ClientMessage(message.Replace(sender.Name, "me"), type));
                else
                    Utility.Send(client.Socket, new ClientMessage(message, type));
            }
        }
        //private void BroadcastClientData(Client sender)
        //{
        //    foreach (Client client in clients)
        //    {
        //        foreach (var pair in onlineCLients)
        //        {
        //            if (client == sender)
        //                Utility.Send(client.Socket, new ClientMessage("Me", ActionType.UPDATELIST));
        //            else
        //                Utility.Send(client.Socket, new ClientMessage(pair.Value, ActionType.UPDATELIST));

        //        }
        //    }
        //}
        //private void SendClientData(Client client, Guid id, string username)
        //{
        //    using var ms = new MemoryStream();
        //    using var bw = new BinaryWriter(ms);
        //    int size = 0;
        //    bw.Write(size);
        //    bw.Write((short)ActionType.USERCONNECTED);
        //    bw.Write(id.ToString());
        //    bw.Write(username);
        //    bw.Seek(0, SeekOrigin.Begin);
        //    bw.Write((int)bw.BaseStream.Length - 4);
        //    client.Socket.Send(ms.ToArray());

        //}

        private void SendList(Client sender)
        {
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            int size = 0;
            bw.Write(size);
            bw.Write((short)ActionType.UPDATELIST);
            //Writing the number of clients 
            bw.Write(clients.Count);
            foreach(Client client in clients)
            {
                //if (client.Name == sender.Name) continue;
                //Writing each name
                bw.Write(client.Name);
            }
            bw.Seek(0, SeekOrigin.Begin);
            bw.Write((int)bw.BaseStream.Length - 4);
            sender.Socket.Send(ms.ToArray());
        }
        private void ServerHome_Load(object sender, EventArgs e)
        {
            
        }
    }
}