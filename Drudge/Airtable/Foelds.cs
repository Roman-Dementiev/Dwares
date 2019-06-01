using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace Dwares.Drudge.Airtable
{
    public class Fields
    {
        [JsonProperty("fields")]
        public Dictionary<string, object> FieldsCollection { get; set; } = new Dictionary<string, object>();

        public void AddField(string fieldName, object fieldValue)
        {
            FieldsCollection.Add(fieldName, fieldValue);
        }
    }

}
