using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintZebra
{
    class ProxyPrint
    {
        static void Main(string[] args)
        {
            var url = "http://localhost:9696/";
            if (args.Length > 0)
                url = args[0];

            using (var server = StartWebServer(url))
            {
                server.RunAsync();
                var browser = new System.Diagnostics.Process()
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true }
                };
                Console.ReadKey(true);
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

    }
}
