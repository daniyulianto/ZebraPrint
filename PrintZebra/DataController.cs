using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Swan.Formatters;
using Zebra.Sdk.Comm;


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
                string id = data[i].id;
                string barcode = data[i].date;
                string price = data[i].price;
                string product = data[i].product;
                string date = data[i].date;
                string date_plan = data[i].date_plan;
                string announcement = data[i].announcement;

                string command1 = "^XA" +
                    "^FO50,280 ^BY4 ^BQN,2,8^FD" + barcode + "^FS" +
                    "^FO50,470 ^ADN,12,12^FD" + barcode + "^FS" +
                    "^FO50,500 ^ADN,12,12^FD" + date + "^FS" +
                    "^FO50,520 ^ADN,12,12^FDCSR038-20190906-00002^FS" +
                    "^FO50,540 ^ADN,12,12^FD"+ price +"^FS" +
                    "^FO50,560 ^ADN,12,12^FDDATE IN : "+ date_plan +
                    "^FO50,590 ^ADN,12,12^FD"+ announcement + "^FS" +
                    "^XZ";
                sendAndWaitDate(command1);
            }


            //handle disini buat ambil data yang mau dicetak
        }
        private void sendAndWaitDate(string prnData)
        {
            Connection conn = null;
            try
            {
                conn = new DriverPrinterConnection("ZDesigner GT800 (EPL)");
                conn.Open();

                byte[] buffer1 = ASCIIEncoding.ASCII.GetBytes(prnData);
                conn.SendAndWaitForResponse(buffer1, 500, 500, null);
                conn.Close();
            }
            catch (ConnectionException e)
            {
                //DemoDialog.showErrorDialog(SendFileDemo.this, e.getMessage(), "Connection Error!");
                //MessageBox.Show(e.Message);
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
