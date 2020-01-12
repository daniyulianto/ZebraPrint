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
using System.Text;

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
            //ZebraClass zebraPrinterHelper = new ZebraClass();
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
            /*
            StringBuilder label = new StringBuilder();
            label.AppendLine("N");
            label.AppendLine("ZT");
            label.AppendLine("D10");
            label.AppendLine("Q680,B24");
            label.AppendLine("q440");
            label.AppendLine("b30,300,P,380,800,x2,y11,l100,r100,f0,s5,\"12345678900000\"");
            label.AppendLine("A70,430,0,1,2,2,N,\"12345678900000\"");
            label.AppendLine("A30,470,0,1,1,1,N,\"Line Pertama\"");
            label.AppendLine("A30,490,0,1,1,1,N,\"Line Kedua\"");
            label.AppendLine("A30,510,0,1,1,1,N,\"Line Ketiga\"");
            label.AppendLine("A30,530,0,1,1,1,N,\"Line Keempat \"");
            label.AppendLine("A30,550,0,1,1,1,N,\"Line Kelima \"");
            label.AppendLine("A30,570,0,1,1,1,N,\"Line Keenam \"");
            label.AppendLine("A100,620,0,1,1,1,N,\"Ceria Tiada Habisnya\"");
            label.AppendLine("ZT");
            label.AppendLine("P1");
            zebraPrinterHelper.CetakZebra(label.ToString(), selectedPrinter);
            */
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
