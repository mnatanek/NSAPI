using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace NSAPI
{
    [Serializable()]
    public class Message
    {
        public Message(string v)
        {
            this.Body = v;
        }

        public string Body { get; set; }
    }

    public class MessageEventArgs : EventArgs
    {
        public Message Msg { get; set; }

        public MessageEventArgs(Message msg)
        {
            Msg = msg;
        }
    }

    public static class API
    {
        private static string url = "https://n-soft.pl/NSAPI/";

        public enum Methods { LOGIN, VERSION }

        private static string _rawResponse;
        public static string RawResponse
        {
            get { return _rawResponse; }
        }
        private static string JsonPrettify(string json)
        {
            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter())
            {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }

        public static dynamic Query(Methods method, NameValueCollection data)
        {
            _rawResponse = "";

            using (var wb = new WebClient())
            {
                Log("URL QUERYING: " + url + method);

                try
                {
                    var response = wb.UploadValues(url + method, "POST", data);
                    _rawResponse = JsonPrettify(Encoding.UTF8.GetString(response));

                    Log("RESPONSE DATA: " + Encoding.UTF8.GetString(response));
                }
                catch (Exception e)
                {
                    Log("ERROR: " + e.Message);
                }
            }

            return JsonConvert.DeserializeObject<dynamic>(_rawResponse);
        }

        private static void Log(string v)
        {
            Directory.CreateDirectory("C:\\NSAPI");

            using (StreamWriter sr = new StreamWriter("C:\\NSAPI\\Api.log", true))
            {
                Message m = new Message(DateTime.Now.ToString() + " :: " + v + "\r\n");
                sr.WriteLine(m.Body);

                OnLogChanged(new MessageEventArgs(m));
            }
        }

        public static event EventHandler<MessageEventArgs> LogChanged;

        public static void OnLogChanged(MessageEventArgs e)
        {
            LogChanged?.Invoke(new object(), e);
        }

        public static void ShowTestForm()
        {
            new TestForm().Show();
        }
    }
}
