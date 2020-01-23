using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintZebra
{
    public class TicketData
    {
        public int id { get; set; }
        public string ticket { get; set; }
    }
    public class PrintTicket
    {
        public List<TicketData> tickets { get; set; }
    }
}
