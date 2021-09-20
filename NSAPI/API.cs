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

    /// <summary>
    /// Klasa odpowiedzialna za komunikację aplikacji z serwerem danych
    /// </summary>
    public static class API
    {
        private static string url = "https://n-soft.pl/NSAPI/";

        public enum Methods { LOGIN, VERSION }

        private static string _rawResponse;
        public static string RawResponse
        {
            get { return _rawResponse; }
        }

        /// <summary>
        /// Formatuje teskt z danymi JSON na łątwy do odczytania przez człowieka
        /// </summary>
        /// <param name="json">zawartość w formacie JSON</param>
        /// <returns></returns>
        private static string JsonPrettify(string json)
        {
            using (var stringReader = new StringReader(json))
            {
                using (var stringWriter = new StringWriter())
                {
                    var jsonReader = new JsonTextReader(stringReader);
                    var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
                    jsonWriter.WriteToken(jsonReader);
                    return stringWriter.ToString();
                }
            }
        }

        /// <summary>
        /// Odwołuje się do serwera danych i pobiera informacje poprzez plik JSON wykorzystując metode POST
        /// </summary>
        /// <param name="method">Wybrana metoda przez użytkownika</param>
        /// <param name="data">kolekcja parametrów</param>
        /// <returns>Dynamiczny obiekt zawsze z własciwościami:
        /// Data (paczka z danymi), 
        /// Info (Informacja w postaci tekstowej, np opis błędu),
        /// Status (status wykonania: ERROR lub OK)</returns>
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

        /// <summary>
        /// Zapis danych do pliku tekstowego w ścieżce C:\NSAPI\API.log z włączoną opcją dopisywania
        /// Dane zostają zapisane uzupełnione o aktualny czas na początku każdego wiersza 
        /// Metoda generuje również zdarzenie zawierające informację o aktualnym zdarzeniu
        /// </summary>
        /// <param name="v">Informacja, która ma być zapisana</param>
        private static void Log(string v)
        {
            //Tworzymy katalog o ile nie istnieje
            Directory.CreateDirectory("C:\\NSAPI");

            //Otwarcie strumienia zapisu wraz z obowiązkowym jego zamknięciem po wykonaniu wszystkich operacji
            using (StreamWriter sr = new StreamWriter("C:\\NSAPI\\Api.log", true))
            {
                Message m = new Message(DateTime.Now.ToString() + " :: " + v + "\r\n");
                sr.WriteLine(m.Body);

                OnLogChanged(new MessageEventArgs(m));
            }
        }

        /// <summary>
        /// Zdarzenie wywoływane w momencie zapisywania informacji o logach
        /// </summary>
        public static event EventHandler<MessageEventArgs> LogChanged;

        /// <summary>
        /// Wywołanie zdarzenia informującego o aktualnym postępie prac
        /// </summary>
        /// <param name="e"></param>
        public static void OnLogChanged(MessageEventArgs e)
        {
            LogChanged?.Invoke(new object(), e);
        }

        /// <summary>
        /// Uruchomienie okna z formularzem testowym
        /// </summary>
        public static void ShowTestForm()
        {
            new TestForm().Show();
        }
    }
}
