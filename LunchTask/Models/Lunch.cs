using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LunchTask.Models
{
    public class Lunch
    {

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("menu")]
        public string Menu { get; set; }

        [JsonProperty("ratings")]
        public IList<object> Ratings { get; set; }

        [JsonProperty("comments")]
        public IList<object> Comments { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }
    }

}