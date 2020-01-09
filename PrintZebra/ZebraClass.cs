using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zebra.Sdk.Comm;

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

                byte[] buffer1 = ASCIIEncoding.ASCII.GetBytes(printData);
                conn.SendAndWaitForResponse(buffer1, 500, 500, null);
                conn.Close();
                return true;
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
                catch (ConnectionException)
                {
                }
            }
        }
    }
}
