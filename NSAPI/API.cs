using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace NSAPI
{
    /// <summary>
    /// Klasa odpowiedzialna za komunikację aplikacji z serwerem danych
    /// </summary>
    public static class API
    {
        private static string url = "https://n-soft.pl/NSAPI/";

        private static string _rawResponse;

        private static object license = null;

        /// <summary>
        /// Przechowuje informacje odebrane z serwera w postaci JSON
        /// </summary>
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
        /// <param name="method">Nazwa funkcji do wykonania</param>
        /// <param name="data">kolekcja parametrów</param>
        /// <returns>Dynamiczny obiekt zawsze z własciwościami:
        /// Data (paczka z danymi), 
        /// Info (Informacja w postaci tekstowej, np opis błędu),
        /// Status (status wykonania: ERROR lub OK)</returns>
        public static dynamic Query(string method, Params data)
        {
            if (method != "API.auth" && license == null)
                method = "API.no_license";

            if (license != null)
                data.Add("ownertoken", Convert.ToString(((dynamic)license).Key));

            _rawResponse = "";

            using (var wb = new WebClient())
            {
                Log("URL QUERYING: " + url + method);

                try
                {
                    Log("SENDING DATA: " + JsonConvert.SerializeObject(data.AllKeys.ToDictionary(k => k, k => data[k])));

                    var response = wb.UploadValues(url + method, "POST", data);
                    _rawResponse = JsonPrettify(Encoding.UTF8.GetString(response));

                    Log("RESPONSE DATA: " + Encoding.UTF8.GetString(response));

                    if (method == "API.auth")
                    {
                        dynamic l = JsonConvert.DeserializeObject<dynamic>(_rawResponse);

                        license = (l.Status == "ERROR") ? null : l.Data;
                    }
                }
                catch (Exception e)
                {
                    Log("ERROR: " + e.Message);
                }
            }

            return JsonConvert.DeserializeObject<dynamic>(_rawResponse);
        }

        /// <summary>
        /// Odwołuje się do serwera danych i pobiera informacje poprzez plik JSON. Wykonuje funkcje bezparametrowe
        /// </summary>
        /// <param name="method">Nazwa funkcji do wykonania</param>
        /// <returns>Dynamiczny obiekt zawsze z własciwościami:
        /// Data (paczka z danymi), 
        /// Info (Informacja w postaci tekstowej, np opis błędu),
        /// Status (status wykonania: ERROR lub OK)</returns>
        public static dynamic Query(string method)
        {
            return Query(method, new Params());
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
                sr.WriteLine(DateTime.Now.ToString() + " :: " + v);

                OnLogChanged(new MessageEA(DateTime.Now.ToString() + " :: " + v));
            }
        }

        /// <summary>
        /// Zdarzenie wywoływane w momencie zapisywania informacji o logach
        /// </summary>
        public static event EventHandler<MessageEA> LogChanged;

        /// <summary>
        /// Wywołanie zdarzenia informującego o aktualnym postępie prac
        /// </summary>
        /// <param name="e"></param>
        private static void OnLogChanged(MessageEA e)
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
