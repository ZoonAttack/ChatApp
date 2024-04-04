using System.Net;
using System.Net.Sockets;
using Communicator;
namespace Client
{
    public partial class ClientHome : Form
    {
        Socket clientSocket;

        bool isReading = true;

        ClientMessage message;
        List<string> onlineClients = new List<string>();
        public ClientHome()
        {
            InitializeComponent();
        }

        private void BTN_Connect_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocolType: ProtocolType.Tcp);
            IPEndPoint EndPoint = new IPEndPoint(IPAddress.Parse(TB_Address.Text), 8888);
            if (TB_Username.Text.Length > 0)
            {
                //try
                //{
                clientSocket.Connect(EndPoint);
                if (clientSocket.Connected)
                {
                    //Send USERCONNECTED
                    message = new ClientMessage(TB_Username.Text, ActionType.USERCONNECTED);
                    Utility.Send(clientSocket, message);
                    Thread incomingDataThread = new Thread(ReadingData);
                    incomingDataThread.Start();
                }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}
            }
        }
        private void BTN_Send_Click(object sender, EventArgs e)
        {
            message = new ClientMessage(TB_Message.Text, ActionType.MESSAGE);
            Utility.Send(clientSocket, message);
        }
        private void ReadingData()
        {
            byte[] buffer;
            int bytesRead = 0;
            while (isReading)
            {
                if (!clientSocket.Connected)
                {
                    clientSocket.Close();
                    isReading = false;
                    return;
                }
                //try
                //{
                buffer = new byte[sizeof(int)];
                bytesRead = clientSocket.Receive(buffer);
                if (bytesRead == 0) return;
                if (bytesRead != buffer.Length) throw new InvalidDataException($"Got {bytesRead} Expected {buffer.Length}");

                int size = BitConverter.ToInt32(buffer, 0);
                buffer = new byte[size];
                bytesRead = clientSocket.Receive(buffer);
                if (bytesRead != buffer.Length) throw new InvalidDataException($"Got {bytesRead} Expected {buffer.Length}");
                using var ms = new MemoryStream(buffer);
                using var br = new BinaryReader(ms);
                ActionType type = (ActionType)br.ReadInt16();
                string messageReceived;
                switch (type)
                {
                    case ActionType.MESSAGE:
                        messageReceived = br.ReadString();
                        UpdateUI(TB_ChatBox, $"{messageReceived}{Environment.NewLine}");
                        break;
                    case ActionType.SERVERDISCONNECTED:
                        messageReceived = br.ReadString();
                        isReading = false;
                        UpdateUI(TB_ChatBox, messageReceived);
                        clientSocket.Disconnect(true);
                        return;
                    case ActionType.UPDATELIST:
                        int clientsCount = br.ReadInt32();
                        for(int i = 0; i < clientsCount; i++)
                        {
                            string message = br.ReadString();
                            if (message == TB_Username.Text) message = "me";
                            //if (message == TB_Username.Text)
                            //{
                            //    onlineClients.Add("me");
                            //    updateclientsTB("me", false);
                            //}
                            //else
                            //{
                            //    onlineClients.Add(message);
                            //    updateclientsTB(message, false);
                            //}
                            onlineClients.Add(message);
                            updateclientsTB(message, false);
                        }
                        break;
                    case ActionType.USERCONNECTED:
                        string username = br.ReadString();
                        onlineClients.Add(username);
                        updateclientsTB(username, false);
                        break;
                    case ActionType.USERDISCONNECTED:
                        string disconnectedUsername = br.ReadString();
                        onlineClients.Remove(disconnectedUsername);
                        updateclientsTB(disconnectedUsername, true);
                        break;
                }
                //}
                //catch (SocketException soex)
                //{
                //    clientSocket.Dispose();
                //    MessageBox.Show(soex.Message);
                //}
            }
        }
        private void UpdateUI(TextBox tb, string text, bool connected = true)
        {
            if (this.Disposing || this.IsDisposed)
            {
                MessageBox.Show("Inside disposing");
                return;
            }


            if (tb.InvokeRequired)
            {
                tb.Invoke(new Action(() => UpdateUI(tb, text, connected)));
            }
            else
            {
                if (!connected && tb == TB_OnlineClients) tb.Text = tb.Text.Replace(text, " ");
                tb.AppendText($"{text.Trim()}");
                tb.AppendText(Environment.NewLine);
            }
        }
        private void updateclientsTB(string text, bool remove)
        {
            if (this.Disposing || this.IsDisposed)
            {
                MessageBox.Show("Inside disposing");
                return;
            }
            if(TB_OnlineClients.InvokeRequired)
            {
                TB_OnlineClients.Invoke(new Action(() => updateclientsTB(text, remove)));
            }
            else
            {
                if (!remove)
                {
                    TB_OnlineClients.AppendText(text.Trim());
                    TB_OnlineClients.AppendText(Environment.NewLine);
                }
                else
                {
                    TB_OnlineClients.Text = TB_OnlineClients.Text.Replace(text, " ");
                }
            }
        }

        private void ClientHome_FormClosing(object sender, FormClosingEventArgs e)
        {
            isReading = false;
            if (clientSocket?.Connected != true) return;
            message = new ClientMessage("Disconnected..", ActionType.USERDISCONNECTED);
            Utility.Send(clientSocket, message);
            try
            {
                clientSocket.Shutdown(SocketShutdown.Both);

            }
            finally
            {
                clientSocket.Close();
            }
        }

        private void ClientHome_Load(object sender, EventArgs e)
        {

        }
    }
}