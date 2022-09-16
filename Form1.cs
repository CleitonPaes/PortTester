using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using System.Text.RegularExpressions;

namespace PortTester
{
    public partial class Form1 : Form
    {
        private string MYIP = "";
        private string LocalIP = "";
        private int PortNumber = 0;
        private bool TestResult = false;
        
        public Form1()
        {
            InitializeComponent();
            GetLocalIP();
            Protocol.Text = "TCP";
        }

        private void GetLocalIP()
        {
            try
            {
                UdpClient u = new("8.8.8.8", 1);
                IPAddress localAddr = (u.Client.LocalEndPoint as IPEndPoint).Address;
                LocalIP = localAddr.ToString();
            }
            catch
            {
            }
        }

        private void Lock()
        {
            foreach (Control c in Controls)
            {
                c.Enabled = false;
            }
        }

        private void Unlock()
        {
            foreach (Control c in Controls)
            {
                c.Enabled = true;
            }
        }

        private void Test_Click(object sender, EventArgs e)
        {
            if (Port.TextLength >= 1 && Convert.ToInt32(Port.Text) >= 1 && Convert.ToInt32(Port.Text) <= 65535)
            {
                TestResult = false;
                PortNumber = Convert.ToInt32(Port.Text);
                Status.Text = " Wait";
                Lock();

                Application.DoEvents();

                if(MYIP == "")
                {
                    try
                    {
                        WebRequest request = WebRequest.Create("https://api.ipify.org");
                        request.Timeout = 1000;
                        using WebResponse response = request.GetResponse();
                        using StreamReader stream = new(response.GetResponseStream());
                        {
                            MYIP = stream.ReadToEnd();
                        }
                    }
                    catch { }

                    if (MYIP == "")
                    {
                        try
                        {
                            WebRequest request = WebRequest.Create("https://ip4.seeip.org");
                            request.Timeout = 1000;
                            using WebResponse response = request.GetResponse();
                            using StreamReader stream = new(response.GetResponseStream());
                            {
                                MYIP = stream.ReadToEnd();
                            }
                        }
                        catch { }
                    }
                }

                if (Protocol.Text == "TCP")
                {
                    TCPServer.RunWorkerAsync();

                    Thread.Sleep(1000);

                    using TcpClient tcpClient = new();
                    {
                        try
                        {
                            IAsyncResult result = tcpClient.BeginConnect(MYIP, PortNumber, null, null);
                            if (!result.AsyncWaitHandle.WaitOne(1000, false))
                            {
                                tcpClient.Close();
                            }
                            tcpClient.EndConnect(result);
                        }
                        catch (Exception)
                        {
                            tcpClient.Close();
                        }
                    }
                }

                if (Protocol.Text == "UDP")
                {
                    UDPServer.RunWorkerAsync();

                    Thread.Sleep(1000);

                    using UdpClient udpClient = new();
                    {
                        try
                        {
                            udpClient.Client.ReceiveTimeout = 1000;

                            udpClient.Connect(MYIP, PortNumber);

                            byte[] sendBytes = new byte[1];
                            udpClient.Send(sendBytes, sendBytes.Length);
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
            try
            {
                TcpListener listener = new(IPAddress.Parse(LocalIP), PortNumber);
                listener.Start();
                if (listener.AcceptTcpClientAsync().Wait(2000))
                {
                    TestResult = true;
                }
                listener.Stop();
            }
            catch (Exception)
            {
            }
        }

        private void UDPServer_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                UdpClient listener = new(PortNumber);

                if (listener.ReceiveAsync().Wait(2000))
                {
                    TestResult = true;
                }
                listener.Close();
            }
            catch (Exception)
            {
            }
        }

        private void TCPServer_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Unlock();
            if (TestResult == true)
            {
                Status.Text = " Open";
            }
            else
            {
                Status.Text = "Closed";
            }
        }

        private void UDPServer_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Unlock();
            if (TestResult == true)
            {
                Status.Text = " Open";
            }
            else
            {
                Status.Text = "Closed";
            }
        }

        private void Listen_Click(object sender, EventArgs e)
        {
            if (Port.TextLength >= 1 && Convert.ToInt32(Port.Text) >= 1 && Convert.ToInt32(Port.Text) <= 65535)
            {
                TestResult = false;
                PortNumber = Convert.ToInt32(Port.Text);
                Status.Text = " Listening";
                Lock();
                Application.DoEvents();

                if (Protocol.Text == "TCP")
                {
                    TCPListener.RunWorkerAsync();
                }
                if (Protocol.Text == "TCP")
                {
                    UDPListener.RunWorkerAsync();
                }
            }
        }

        private void TCPListener_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                TcpListener listener = new(IPAddress.Parse(LocalIP), PortNumber);
                listener.Start();
                if (listener.AcceptTcpClientAsync().Wait(30000))
                {
                    TestResult = true;
                }
                listener.Stop();
            }
            catch (Exception)
            {
            }
        }

        private void UDPListener_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                UdpClient listener = new(PortNumber);

                if (listener.ReceiveAsync().Wait(30000))
                {
                    TestResult = true;
                }
                listener.Close();
            }
            catch (Exception)
            {
            }
        }


        private void TCPListener_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Unlock();
            if (TestResult == true)
            {
                Status.Text = " Open";
            }
            else
            {
                Status.Text = "Closed";
            }
        }

        private void UDPListener_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Unlock();
            if (TestResult == true)
            {
                Status.Text = " Open";
            }
            else
            {
                Status.Text = "Closed";
            }
        }

        private void Port_KeyPress(object sender, KeyPressEventArgs e)
        {
            Port.MaxLength = 5;
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void Port_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(Port.Text, "[^0-9]"))
            {
                Port.Text = "";
                Port.MaxLength = 5;
            }
        }
    }
}
