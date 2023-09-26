using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Discord.Rest;

namespace JsonClass
{
    public class Json
    {

        public string ReturnValue(string ValueWanted, string fileLocation )
        {
            JObject JsonObject = JObject.Parse(File.ReadAllText(fileLocation));

            var ValueReturned = JsonObject.GetValue(ValueWanted).ToString();
            return ValueReturned;
        }
    }
}
