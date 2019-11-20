
namespace PrintZebra
{
    public class Ticket
    {
        public int id { get; set; }
        public string barcode { get; set; }
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string line3 { get; set; }
        public string line4 { get; set; }
        public string line5 { get; set; }
        public string line6 { get; set; }
    }
    public class Data
    {
        public string success { get; set; }
        public string message { get; set; }
    }

    public class PrintResult
    {
        public int code { get; set; }
        public Data data { get; set; }
    }
}
