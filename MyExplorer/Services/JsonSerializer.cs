using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MyExplorer.Services
{
    class JsonSerializer<T> where T : class
    {
        DataContractJsonSerializer serializer;

        public JsonSerializer()
        {
            serializer = new DataContractJsonSerializer(typeof(T));
        }

        public T ReadFromFile(string fileName)
        {
            T result;
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                result = (T)serializer.ReadObject(fs);
            }
            return result;
        }

        public void WriteToFile(T jItems, string fileName)
        {
            if (!File.Exists(fileName))
            {
                File.Open(fileName, FileMode.Create).Close();
            }
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, jItems);
                File.WriteAllText(fileName, FormatJson(Encoding.UTF8.GetString(ms.ToArray())));
            }
        }

        private const string INDENT_STRING = "    ";
        public string FormatJson(string json)
        {

            int indentation = 0;
            int quoteCount = 0;
            var result =
                from ch in json
                let quotes = ch == '"' ? quoteCount++ : quoteCount
                let lineBreak = ch == ',' && quotes % 2 == 0 ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, indentation)) : null
                let openChar = ch == '{' || ch == '[' ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, ++indentation)) : ch.ToString()
                let closeChar = ch == '}' || ch == ']' ? Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, --indentation)) + ch : ch.ToString()
                select lineBreak == null
                            ? openChar.Length > 1
                                ? openChar
                                : closeChar
                            : lineBreak;

            return String.Concat(result);
        }
    }
}
