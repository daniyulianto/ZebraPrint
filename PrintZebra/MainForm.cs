using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using EmbedIO;
using EmbedIO.WebApi;
using Swan.Logging;
using IniParser;
using IniParser.Model;


namespace PrintZebra
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            var parser = new FileIniDataParser();
            if (File.Exists("Configuration.ini") == false)
            {
                IniData data = new IniData();
                data["server"]["url"] = "https://saloka.arkana.app";
                data["server"]["printer"] = "ZDesigner GT800 (EPL)";
                data["server"]["api"] = "654321";
                parser.WriteFile("Configuration.ini", data);

            }
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

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
