using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Billiard.Common.WebToolkit
{
    public static class JsonToClass<T>
    {
        public static T ConverToClass(string json)
        {
            var cls = JsonConvert.DeserializeObject<T>(json );
            return cls;
        }
    }
}