using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Billiard.Common.WebToolkit
{
    public static class ClassToJson<T>
    {
        public static string ConverToJson(T cls)
        {
            var json = JsonConvert.SerializeObject(cls, Formatting.Indented);
            return json;
        }
    }
}