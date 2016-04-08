using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LunchTask.Models;
using Newtonsoft.Json;

namespace LunchTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var lunchRequest = WebRequest.CreateHttp("https://lunch.kabbage.com/api/v2/lunches");
            var lunches = new Lunch[0];
            var response = lunchRequest.GetResponse();
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        lunches = JsonConvert.DeserializeObject<Lunch[]>(reader.ReadToEnd());
                    }
                }
            }

            foreach (var lunch in lunches)
            {
                if (lunch.Date > DateTime.Today)
                {
                    Console.WriteLine(lunch.Menu);
                }
            }

            if (Environment.UserInteractive)
            {
                Console.Write("\nPress any key to continue...");
                Console.ReadKey(true);
                Console.WriteLine("\nExiting application...");
                Environment.Exit(0);
            }
        }
    }
}
