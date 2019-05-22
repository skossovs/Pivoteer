using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.Common
{
    static class ApplicationCommands
    {
        public static void ExitApplication()
        {
            System.Environment.Exit(1);
        }

        public static Object ReadJsonObject(string fullPath)
        {
            JsonSerializer serializer = new JsonSerializer();
            Object result = null;

            using (var sr = new StreamReader(fullPath))
            using (var reader = new JsonTextReader(sr))
            {
                result = serializer.Deserialize(reader);
            }
            return result;
        }

        public static void SaveJsonObject(string fullPath, Object obj)
        {
            JsonSerializer serializer = new JsonSerializer();
//            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(fullPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, obj);
            }

        }
    }
}
