using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using IniParser;
using IniParser.Model;
using RestSharp;
using Newtonsoft.Json;

namespace PrintZebra
{
    public sealed class DataController : WebApiController
    {
        [Route(HttpVerbs.Post, "/print")]
        public async Task<bool> Ticket()
        {
            var data = await HttpContext.GetRequestDataAsync<List<Ticket>>();
            CetakTicket(data);
            return true;

        }

        private async void CetakTicket(List<Ticket> data)
        {
            var parser = new FileIniDataParser();
            IniData iniData = parser.ReadFile("Configuration.ini");
            string serverUrl = iniData["server"]["url"];
            string merkPrinter = iniData["server"]["printer"];
            string serverApi = iniData["server"]["api"];
            ZebraClass zebraPrinterHelper = new ZebraClass();
            List<int> ticket_ids = new List<int>();
            int jml = data.Count;
            foreach (Ticket ticket in data)
            {
                ticket_ids.Add(ticket.id);
            }
            await SendPrintingStatus(ticket_ids, serverApi, serverUrl, "printing");
            for (int i = 0; i < jml; i++)
            {
                int ticket_id = data[i].id;
                string barcode = data[i].barcode;
                string line1 = data[i].line1;
                string line2 = data[i].line2;
                string line3 = data[i].line3;
                string line4 = data[i].line4;
                string line5 = data[i].line5;
                string line6 = data[i].line6;
                Console.WriteLine(ticket_id);

                StringBuilder label = new StringBuilder();
                label.AppendLine("^XA");
                label.AppendLine("^POI");
                label.AppendLine("^FO15,300^BY4^BQN,2,8^FDAM,A" + barcode + "^FS ");
                label.AppendLine("^FO25,490^ADN,12,12^FD" + barcode + "^FS");
                label.AppendLine("^FO200,430^ADN,12,12^FD" + line1 + "^FS");
                label.AppendLine("^FB250,3,0,L,0^FO200,340^ADN,12,12^FD" + line2 + "^FS");
                label.AppendLine("^FO200,400^ADN,12,12^FD" + line3 + "^FS");
                label.AppendLine("^FO200,460^ADN,12,12^FD" + line4 + "^FS");
                label.AppendLine("^FO200,310^ADN,12,12^FD" + line5 + "^FS");
                label.AppendLine("^FB430,2,0,C,0^FO8,540^ADN,5,10^FD" + line6 + "^FS");
                label.AppendLine("^FB430,2,0,C,0^FO8,600^ADN,5,10^FDCeria Tiada Habisnya!^FS");
                label.AppendLine("^XZ");
                zebraPrinterHelper.CetakZebra(label.ToString(), merkPrinter);
                /*
                if (PrinterController.SendStringToPrinter(merkPrinter, label.ToString()) == true)
                {
                    await UpdateStatus(ticket_id, serverApi, serverUrl, "printed");
                }
                else
                {
                    await UpdateStatus(ticket_id, serverApi, serverUrl, "draft");
                }
                */
                    
            }
        }
        public async Task UpdateStatus(int ticket_id, string api_key, string server_url, string status)
        {
            var client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, server_url+ "/saloka/ticket/print_proxy/"+status+"/" + ticket_id);
            requestMessage.Headers.Add("X-Api-Key", api_key);
            requestMessage.Content = new StringContent("{}",
                                    Encoding.UTF8,
                                    "application/json");
            var result = await client.SendAsync(requestMessage);

        }
        public async Task SendPrintingStatus(List<int> ticket_ids, string api_key, string server_url, string status)
        {
            var request = new RestRequest(Method.POST);
            RestClient url = new RestClient(server_url + "/saloka/ticket/print_proxy/" + status);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("X-Api-Key", api_key);
            request.AddParameter("application/json", "{\"ticket_ids\":" + JsonConvert.SerializeObject(ticket_ids) + "}", ParameterType.RequestBody);
            var result = await url.ExecuteTaskAsync(request);
        }
    }
}
