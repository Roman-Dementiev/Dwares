using System;
using Newtonsoft.Json;


namespace Dwares.Drudge.Airtable
{
    public enum SortDirection
    {
        Asc,
        Desc
    }

    public class Sort
    {
        [JsonProperty("fields")]
        public string Field { get; set; }

        [JsonProperty("direction")]
        public SortDirection Direction { get; set; }
    }


}
