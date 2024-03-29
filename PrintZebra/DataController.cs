﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
            PrintTicket printTicket = new PrintTicket();
            printTicket.tickets = new List<TicketData>();
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
                label.AppendLine("N");
                label.AppendLine("ZT");
                label.AppendLine("D10");
                label.AppendLine("Q680,B24");
                label.AppendLine("q440");
                //test on 10*10 label 
                //label.AppendLine("Q800,24");
                //label.AppendLine("q800");
                label.AppendLine("b30,300,P,380,800,x2,y11,l100,r100,f0,s5,\"" + barcode + "\"");
                label.AppendLine("A70,430,0,1,2,2,N,\"" + barcode + "\"");
                label.AppendLine("A30,470,0,1,1,1,N,\"" + line1 + "\"");
                label.AppendLine("A30,490,0,1,1,1,N,\"" + line2 + "\"");
                label.AppendLine("A30,510,0,1,1,1,N,\"" + line3 + "\"");
                label.AppendLine("A30,530,0,1,1,1,N,\"" + line4 + " \"");
                label.AppendLine("A30,550,0,1,1,1,N,\"" + line5 + " \"");
                label.AppendLine("A30,570,0,1,1,1,N,\"" + line6 + " \"");
                label.AppendLine("A100,620,0,1,1,1,N,\"Ceria Tiada Habisnya!\"");
                label.AppendLine("ZT");
                label.AppendLine("P1");
                printTicket.tickets.Add(new TicketData { id = ticket_id, ticket = label.ToString() });
                /*
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
                */
            }
            //await zebraPrinterHelper.CetakZebraAsync(printTicket, merkPrinter);
            await zebraPrinterHelper.CetakWindowsAsync(printTicket, merkPrinter);
        }
        public async Task SendPrintingStatus(List<int> ticket_ids, string api_key, string server_url, string status)
        {
            var request = new RestRequest(Method.POST);
            RestClient url = new RestClient(server_url + "/saloka/ticket/print_proxy/" + status);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("X-Api-Key", api_key);
            request.AddParameter("application/json", "{\"ticket_ids\":" + JsonConvert.SerializeObject(ticket_ids) + "}", ParameterType.RequestBody);
            _ = await url.ExecuteTaskAsync(request);
        }
    }
}
