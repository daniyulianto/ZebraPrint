using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zebra.Sdk.Comm;
using EmbedIO;
using EmbedIO.WebApi;
using Swan.Logging;


namespace PrintZebra
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            var url = "http://localhost:9696/";
            using (var server = StartWebServer(url))
            {
                server.RunAsync();
            }

        }
        private static WebServer StartWebServer(string url)
        {
            var server = new WebServer(o => o
                    .WithUrlPrefix(url)
                    .WithMode(HttpListenerMode.EmbedIO))
                    .WithCors(
                        "https://saloka.arkana.app",
                        "content-type, accept",
                        "post")
                    .WithWebApi("/api", m => m
                    .WithController<DataController>());

            // Listen for state changes.
            server.StateChanged += (s, e) => $"WebServer New State - {e.NewState}".Info();

            return server;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //this.ShowInTaskbar = false;
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.Visible = true;
            
        }

        private void sendAndWaitDate(string prnData)
        {
            Connection conn = null;
            try
            {
                conn = new DriverPrinterConnection("ZDesigner GT800 (EPL)");
                conn.Open();

                byte[] buffer1 = ASCIIEncoding.ASCII.GetBytes(prnData);
                conn.SendAndWaitForResponse(buffer1,500,500,null);
                conn.Close();
            }
            catch (ConnectionException e)
            {
                //DemoDialog.showErrorDialog(SendFileDemo.this, e.getMessage(), "Connection Error!");
                MessageBox.Show(e.Message);
            }
            finally
            {
                try
                {
                    if (conn != null)
                        conn.Close();
                }
                catch (ConnectionException)
                {
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i=0;i<2;i++)
            {
                string kodebar = "1234567890" + i;
                //DateTime Waktu = DateTime.Now;
                string command1 = "^XA" +
                    "^FO50,280 ^BY4 ^BQN,2,8^FD" + kodebar + "^FS" +
                    "^FO50,470 ^ADN,12,12^FD" + kodebar + "^FS" +
                    "^FO50,500 ^ADN,12,12^FD" + DateTime.Now + "^FS" +
                    "^FO50,520 ^ADN,12,12^FDCSR038-20190906-00002^FS" +
                    "^FO50,540 ^ADN,12,12^FDNORMAL TIKET Rp. 120.000,-^FS" +
                    "^FO50,560 ^ADN,12,12^FDDATE IN : 06-09-2019^FS" +
                    "^FO50,590 ^ADN,12,12^FDCaeria Tiada Habisnya^FS" +
                    "^XZ";
                sendAndWaitDate(command1); 
            
            }
            
        }
    }
}
