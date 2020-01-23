using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;
using System.Net.Http;
using IniParser;
using IniParser.Model;

namespace PrintZebra
{
    public class ZebraClass
    {
        public async Task<bool> CetakZebraAsync(PrintTicket printData, string printerName)
        {
            var parser = new FileIniDataParser();
            IniData iniData = parser.ReadFile("Configuration.ini");
            string serverUrl = iniData["server"]["url"];
            string serverApi = iniData["server"]["api"];
            Connection conn = null;
            try
            {
                conn = new DriverPrinterConnection(printerName);
                conn.Open();
                ZebraPrinter zebraPrinter = ZebraPrinterFactory.GetInstance(conn);
                PrinterStatus printStatus = zebraPrinter.GetCurrentStatus();
                if (printStatus.isReadyToPrint)
                {
                    foreach (TicketData data in printData.tickets)
                    {
                        byte[] buffer1 = ASCIIEncoding.ASCII.GetBytes(data.ticket);
                        conn.SendAndWaitForResponse(buffer1, 1000, 1000, null);
                        await UpdateStatus(data.id, serverApi, serverUrl, "printed");
                    }
                    conn.Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("Printer is not ready!");
                    foreach (TicketData data in printData.tickets)
                    {
                        await UpdateStatus(data.id, serverApi, serverUrl, "draft");
                    }
                    return false;
                }
                
            }
            catch (ConnectionException e)
            {
                MessageBox.Show(e.Message);
                foreach (TicketData data in printData.tickets)
                {
                    await UpdateStatus(data.id, serverApi, serverUrl, "draft");
                }
                return false;
            }
            finally
            {
                try
                {
                    if (conn != null)
                        conn.Close();
                }
                catch (ConnectionException e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        public async Task<bool> CetakWindowsAsync(PrintTicket printData, string printerName)
        {
            bool result = false;
            var parser = new FileIniDataParser();
            var printerController = new PrinterController();
            IniData iniData = parser.ReadFile("Configuration.ini");
            string serverUrl = iniData["server"]["url"];
            string serverApi = iniData["server"]["api"];
            try
            {
                foreach (TicketData data in printData.tickets)
                {
                    result = printerController.SendStringToPrinter(printerName, data.ticket);
                    await UpdateStatus(data.id, serverApi, serverUrl, "printed");
                }
                return result;
            }
            catch (ConnectionException e)
            {
                MessageBox.Show(e.Message);
                foreach (TicketData data in printData.tickets)
                {
                    await UpdateStatus(data.id, serverApi, serverUrl, "draft");
                }
                return result;
            }
        }
        public async Task UpdateStatus(int ticket_id, string api_key, string server_url, string status)
        {
            var client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, server_url + "/saloka/ticket/print_proxy/" + status + "/" + ticket_id);
            requestMessage.Headers.Add("X-Api-Key", api_key);
            requestMessage.Content = new StringContent("{}",
                                    Encoding.UTF8,
                                    "application/json");
            var result = await client.SendAsync(requestMessage);

        }
    }
}
