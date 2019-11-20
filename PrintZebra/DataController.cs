using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Swan.Formatters;


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

        private void CetakTicket(List<Ticket> data)
        {
            int jml = data.Count;
            Console.WriteLine(jml);
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
                label.AppendLine("^FO140,300^BY4^BQN,2,8^FDAM,A"+barcode+"^FS ");//qrcode
                label.AppendLine("^FO105,485^ADN,12,12^FD"+barcode+"^FS");
                label.AppendLine("^FO140,510^ADN,12,12^FD"+line1+"^FS");//line1
                label.AppendLine("^FO140,535^ADN,12,12^FD"+line2+"^FS");//line2
                label.AppendLine("^FO140,560^ADN,12,12^FD"+line3+"^FS");//line3
                label.AppendLine("^FO420,450^ABB,5,10^FD"+line4+"^FS");//line4
                label.AppendLine("^FO10,480^ABB,5,10^FD"+line5+"^FS");//line5
                label.AppendLine("^FO30,615^ADN,12,8^FD"+line6+"^FS");//linE6
                label.AppendLine("^XZ");

                PrinterController.SendStringToPrinter("ZDesigner GT800 (EPL)", label.ToString());
            }
        }
        public async Task UpdateStatus(int ticket_id, string api_key)
        {
            var client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, "http://localhost/saloka/ticket/print_proxy/" + ticket_id);
            requestMessage.Headers.Add("X-Api-Key", api_key);
            requestMessage.Headers.Add("Content-Type", "application/json");
            HttpResponseMessage response = await client.SendAsync(requestMessage);
            var result = Json.Deserialize<PrintResult>(await response.Content.ReadAsStringAsync());
            GetResult(result);
        }

        private void GetResult(PrintResult result)
        {
            //handle disini hasil response dari odoo
        }
    }
}
