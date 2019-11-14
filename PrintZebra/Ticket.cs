
namespace PrintZebra
{
    public class Ticket
    {
        public string id { get; set; }
        public string barcode { get; set; }
        public string price { get; set; }
        public string product { get; set; }
        public string date { get; set; }
        public string date_plan { get; set; }
        public string announcement { get; set; }
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
