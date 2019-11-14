using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Swan.Formatters;
using System;


namespace PrintZebra
{
    public sealed class DataController : WebApiController
    {
        [Route(HttpVerbs.Options, "/print")]
        public async Task Ticket()
        {
            var data = await HttpContext.GetRequestDataAsync<List<Ticket>>();
            CetakTicket(data);

        }

        private void CetakTicket(List<Ticket> data)
        {
            int jml = data.Count;
            Console.WriteLine(jml);
            for (int i = 0; i < jml; i++)
            {

                string tanggal = data[i].date;
                string barcode = data[i].barcode;

                string command1 = "^XA" +
                    "^FO50,280 ^BY4 ^BQN,2,8^FD" + barcode + "^FS" +
                    "^FO50,470 ^ADN,12,12^FD" + barcode + "^FS" +
                    "^FO50,500 ^ADN,12,12^FD" + tanggal + "^FS" +
                    "^FO50,520 ^ADN,12,12^FDCSR038-20190906-00002^FS" +
                    "^FO50,540 ^ADN,12,12^FDNORMAL TIKET Rp. 120.000,-^FS" +
                    "^FO50,560 ^ADN,12,12^FDDATE IN : 06-09-2019^FS" +
                    "^FO50,590 ^ADN,12,12^FDCaeria Tiada Habisnya^FS" +
                    "^XZ"
            }


            //handle disini buat ambil data yang mau dicetak
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
