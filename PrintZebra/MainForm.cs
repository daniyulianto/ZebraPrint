using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using EmbedIO;
using EmbedIO.WebApi;
using Swan.Logging;
using IniParser;
using IniParser.Model;
using System.Collections.Generic;
using System.Printing;
using System.Diagnostics;

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
            IniData iniData = parser.ReadFile("Configuration.ini");
            var url = "http://localhost:9696/";
            using (var server = StartWebServer(url, iniData["server"]["url"]))
            {
                server.RunAsync();
            }

        }
        private static WebServer StartWebServer(string url, string cors)
        {
            var server = new WebServer(o => o
                    .WithUrlPrefix(url)
                    .WithMode(HttpListenerMode.EmbedIO))
                    .WithCors(
                        cors,
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
            this.ShowInTaskbar = false;
            var parser = new FileIniDataParser();
            List<string> PrinterResult = new List<string>();
            foreach (Printer printerResult in GetPrinter())
            {
                PrinterResult.Add(printerResult.Name);
            }
            IniData iniData = parser.ReadFile("Configuration.ini");
            string serverUrl = iniData["server"]["url"];
            string selectedPrinter = iniData["server"]["printer"];
            printer.DataSource = PrinterResult;
            printer.SelectedItem = selectedPrinter;
            server_url.Text = serverUrl;
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.Visible = true;
            saveBtn.Enabled = false;
            server_url.Enabled = false;
            printer.Enabled = false;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData iniData = parser.ReadFile("Configuration.ini");
            iniData["server"]["url"] = server_url.Text;
            iniData["server"]["printer"] = printer.SelectedItem.ToString();
            parser.WriteFile("Configuration.ini", iniData);

            editButton.Enabled = true;
            saveBtn.Enabled = false;
            server_url.Enabled = false;
            printer.Enabled = false;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            saveBtn.Enabled = true;
            editButton.Enabled = false;
            server_url.Enabled = true;
            printer.Enabled = true;
        }
        public IEnumerable<Printer> GetPrinter()
        {
            List<Printer> printers = new List<Printer>();
            LocalPrintServer printServer = new LocalPrintServer();
            PrintQueueCollection printQueuesOnLocalServer = printServer.GetPrintQueues(new[] {
                EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections
            });
            foreach (PrintQueue printer in printQueuesOnLocalServer)
            {
                printers.Add(new Printer { Name = printer.Name });
                Debug.WriteLine("\tThe shared printer : " + printer.Name);
            }
            return printers;
        }
        public class Printer
        {
            public string Name { get; set; }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            if ( this.WindowState == FormWindowState.Normal)
            {
                this.ShowInTaskbar = true;
                notifyIcon.Visible = false;
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                notifyIcon.Visible = true;
            }
        }
    }
}
