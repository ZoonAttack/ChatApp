using System.Net;
using System.Net.Sockets;
using Communicator;
namespace Client
{
    public partial class ClientHome : Form
    {
        Socket clientSocket;

        ClientMessage message;
        public ClientHome()
        {
            InitializeComponent();
        }

        private void BTN_Connect_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocolType: ProtocolType.Tcp);
            IPEndPoint EndPoint = new IPEndPoint(IPAddress.Parse(TB_Address.Text), 8888);
            if (TB_Username.Text.Length > 0 && TB_Address.Text.Equals("127.0.0.1"))
            {
                try
                {
                    clientSocket.Connect(EndPoint);
                    if (Utility.SocketConnected(clientSocket))
                    {
                        //Send username
                        message = new ClientMessage(TB_Username.Text, ActionType.USERNAME);
                        Send(message);
                        Thread incomingDataThread = new Thread(ReadingData);
                        incomingDataThread.Start();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void ReadingData()
        {
            byte[] buffer;
            int bytesRead = 1;
            do
            {
                using (var ms = new MemoryStream())
                {
                    using (var br = new BinaryReader(ms))
                    {
                        if (Utility.SocketConnected(clientSocket))
                        {
                            try
                            {
                                buffer = new byte[sizeof(int)];
                                bytesRead = clientSocket.Receive(buffer);
                                if (bytesRead != buffer.Length) throw new InvalidDataException($"Got {bytesRead} Expected {buffer.Length}");

                                int size = BitConverter.ToInt32(buffer, 0);
                                buffer = new byte[size];
                                bytesRead = clientSocket.Receive(buffer);
                                if (bytesRead != buffer.Length) throw new InvalidDataException($"Got {bytesRead} Expected {buffer.Length}");

                                ActionType type = (ActionType)br.ReadInt16();
                                switch (type)
                                {
                                    case ActionType.MESSAGE:
                                        string messageReceived = br.ReadString();
                                        TB_ChatBox.AppendText($"{messageReceived}{Environment.NewLine}");
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
                            clientSocket.Close();
                        }
                    }
                }


            } while (bytesRead > 0);
        }
        private void Send(Communicator.ClientMessage message)
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

        private void BTN_Send_Click(object sender, EventArgs e)
        {
            message = new ClientMessage(TB_Message.Text, ActionType.MESSAGE);
            Send(message);
        }
    }
}