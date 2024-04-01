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
                    //Send username
                    message = new ClientMessage(TB_Username.Text, ActionType.USERNAME);
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
                        case ActionType.DISCONNECTED:
                            messageReceived = br.ReadString();
                            isReading = false;
                            UpdateUI(TB_ChatBox, messageReceived);
                        if (clientSocket?.Connected != true) return;
                            try
                            {
                                clientSocket.Shutdown(SocketShutdown.Both);
                            }
                            finally
                            {
                                clientSocket.Close();
                            }
                        return;
                    }
                //}
                //catch (SocketException soex)
                //{
                //    clientSocket.Dispose();
                //    MessageBox.Show(soex.Message);
                //}
            }
        }
        private void UpdateUI(TextBox tb, string text)
        {
            if (this.Disposing || this.IsDisposed)
            {
                MessageBox.Show("Inside disposing");
                return;
            }


            if (tb.InvokeRequired)
            {
                tb.Invoke(new Action(() => UpdateUI(tb, text)));
            }
            else
            { 
                tb.AppendText($"{text}{Environment.NewLine}");
            }
        }
        private void ClientHome_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (clientSocket?.Connected != true) return;

            message = new ClientMessage("Disconnected..", ActionType.DISCONNECTED);
            Utility.Send(clientSocket, message);
            isReading = false;
            //clientSocket.Close();
        }
    }
}