using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;

namespace PortTester
{
    public partial class Form1 : Form
    {
        private bool Loading = false;
        private int PortNumber = 0;
        private string MYIP = "";
        private bool UDPResult = false;

        public Form1()
        {
            InitializeComponent();
            Protocol.Text = "TCP";
        }

        private void Test_Click(object sender, EventArgs e)
        {
            if (Loading == false && Port.TextLength >= 1 && Convert.ToInt32(Port.Text) >= 1 && Convert.ToInt32(Port.Text) <= 65535)
            {
                Loading = true;
                PortNumber = Convert.ToInt32(Port.Text);
                Test.Enabled = false;
                Status.Text = " Wait";

                Application.DoEvents();

                if (MYIP == "")
                {
                    try
                    {
                        WebRequest request = WebRequest.Create("https://api.ipify.org");
                        using WebResponse response = request.GetResponse();
                        using StreamReader stream = new(response.GetResponseStream());
                        {
                            MYIP = stream.ReadToEnd();
                        }
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        Test.Enabled = true;

                        Status.Text = "Verify the connection";

                        Loading = false;
                        return;
                    }
                }

                if (Protocol.Text == "TCP")
                {
                    TCPServer.RunWorkerAsync();

                    Thread.Sleep(1000);

                    using TcpClient tcpClient = new();
                    {
                        IAsyncResult result = tcpClient.BeginConnect(MYIP, PortNumber, null, null);
                        WaitHandle timeoutHandler = result.AsyncWaitHandle;
                        try
                        {
                            if (!result.AsyncWaitHandle.WaitOne(1000, false))
                            {
                                tcpClient.Close();
                                Status.Text = "Closed";
                            }

                            tcpClient.EndConnect(result);
                            Status.Text = " Open";
                        }
                        catch (Exception)
                        {
                            tcpClient.Close();
                            Status.Text = "Closed";
                        }
                        finally
                        {
                            timeoutHandler.Close();
                        }
                    }
                }

                if (Protocol.Text == "UDP")
                {
                    UDPServer.RunWorkerAsync();

                    Thread.Sleep(1000);

                    using UdpClient udpClient = new();
                    {

                        UDPResult = false;

                        try
                        {
                            udpClient.Client.ReceiveTimeout = 1000;

                            udpClient.Connect(MYIP, PortNumber);

                            byte[] sendBytes = new byte[1];
                            new Random().NextBytes(sendBytes);
                            udpClient.Send(sendBytes, sendBytes.Length);

                            IPEndPoint remoteIpEndPoint = new(IPAddress.Any, 0);
                            byte[] result = udpClient.Receive(ref remoteIpEndPoint);

                            if (result != null)
                            {
                                UDPResult = true;
                            }
                            udpClient.Close();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        private void TCPServer_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            IPAddress ipAddress = IPAddress.Any;
            Socket listenSocket = new(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEndPoint = new(ipAddress, PortNumber);

            listenSocket.Bind(localEndPoint);
            listenSocket.Listen(1);
            SocketAsyncEventArgs ex = new();
            ex.Completed += AcceptCallback;
            if (!listenSocket.AcceptAsync(ex))
            {
                AcceptCallback(listenSocket, ex);
            }

            static void AcceptCallback(object sender, SocketAsyncEventArgs e)
            {
                Socket listenSocket = (Socket)sender;
                do
                {
                    try
                    {
                        Socket newSocket = e.AcceptSocket;
                        Debug.Assert(newSocket != null);
                        newSocket.Send(Encoding.ASCII.GetBytes("Hello socket!"));
                        newSocket.Disconnect(false);
                        newSocket.Close();
                    }
                    catch
                    {
                    }
                    finally
                    {
                        e.AcceptSocket = null;
                    }
                } while (!listenSocket.AcceptAsync(e));
            }
        }

        private void UDPServer_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            UdpClient udpServer = new(PortNumber);

            udpServer.Client.ReceiveTimeout = 2000;

            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, PortNumber);
                _ = udpServer.Receive(ref remoteEP);
                udpServer.Send(new byte[] { 1 }, 1, remoteEP);
                udpServer.Close();
            }
        }

        private void TCPServer_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Loading = false;
            Test.Enabled = true;
        }

        private void UDPServer_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Loading = false;
            Test.Enabled = true;

            if (UDPResult == true)
            {
                Status.Text = " Open";
            }
            else
            {
                Status.Text = "Closed";
            }
        }
    }
}
