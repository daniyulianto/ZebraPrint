using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
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
    }
}
