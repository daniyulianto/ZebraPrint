using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;

namespace PrintZebra
{
    public class ZebraClass
    {
        public bool CetakZebra(string printData, string printerName)
        {
            Connection conn = null;
            try
            {
                conn = new DriverPrinterConnection(printerName);
                conn.Open();
                ZebraPrinter zebraPrinter = ZebraPrinterFactory.GetInstance(conn);
                PrinterStatus printStatus = zebraPrinter.GetCurrentStatus();
                if (printStatus.isReadyToPrint)
                {
                    byte[] buffer1 = ASCIIEncoding.ASCII.GetBytes(printData);
                    conn.SendAndWaitForResponse(buffer1, 1000, 1000, null);
                    conn.Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("Printer is not ready!");
                    return false;
                }
                
            }
            catch (ConnectionException e)
            {
                MessageBox.Show(e.Message);
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
    }
}
