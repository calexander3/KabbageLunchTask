using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using LunchTask.Models;
using Newtonsoft.Json;
using pebble_api_dotnet;
using pebble_api_dotnet.Layouts;

namespace LunchTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiKey = ConfigurationManager.AppSettings["pebbleTimelineKey"];

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

            var timeline = new Timeline(apiKey);
            foreach (var lunch in lunches)
            {
                if (lunch.Date >= DateTime.Today.AddDays(-1))
                {
                    Console.WriteLine(lunch.Menu);

                    var lunchSections = lunch.Menu.Split(';');
                    var title = lunchSections.First() + ";";
                    var body = new StringBuilder();
                    if (lunchSections.Length > 1)
                    {
                        for (var i = 1; i < lunchSections.Length; i++)
                        {
                            body.Append(lunchSections[i] + ";");
                        }
                    }

                    var result = timeline.sendSharedPin(new List<string> {"KabbageLunch"}, new Pin
                    {
                        Id = "KabbageLunch" + lunch.Date.ToString("yyyyMMdd"),
                        Time = lunch.Date.AddHours(11).AddMinutes(45).ToUniversalTime(),
                        Duration = new TimeSpan(1, 0, 0),
                        Layout = new GenericLayout
                        {
                            Body = body.ToString(),
                            Title = title,
                            TinyIcon = "system://images/DINNER_RESERVATION"
                        }
                    }).Result;

                    if (!result.Success)
                    {
                        Console.WriteLine(result.ErrorDescription);
                    }
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
