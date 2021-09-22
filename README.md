# NS-API
Projekt na potrzeby szkoleń z przedmiotu PAI

**UWAGI do dokumentacji!**
* znak __*__ w tabeli parametrów oznacza wymagalność danego parametru

### Dostępne metody, zdarzenia i właściwości

  Nazwa           | Typ       | Opis
  --------------- | --------- | ----------------
  API.Query       | metoda    | Służy do połączenia z serwerem danych i pobrania danych lub wykonania operacji na danych 
  API.RawResponse | string    | Przechowuje dane w postaci sformatowanego obiektu JSON jakie zostały zwrócone przez metodę Query
  API.LogChanged  | zdarzenie | Uruchamiane podczas wystąpienia operacji związanych z wysyłaniem/pobieraniem danych z serwera

### Opis obiektu zwracanego przez funkcje API

Nazwa parametru | Typ danych | Opis
--------------- | ---------- | ----
Data | dynamic | Dane zwracane przez funkcję API, różne w zależności od wykonywanej funkcji
Info | string | Informacja na temat wykonania danej funkcji
Status | enum | **SUCCESS** - w przypadku sukcesu<br> **ERROR** - w przypadku występienia błędu, dodatkowa informacja o błędzie przechowywana jest w zmiennej Info

## Przykłady użycia

+ <details><summary>Przykład użycia funkcji API.version (bez parametrów)</summary>

  ```cs
  dynamic d = API.Query("API.version");
  ```
  
  Zwrócona zawartość
  
  ```json
  {
      "Data": "",
      "Info": "NS API v. 1.0.0",
      "Status": "OK"
  }
  ```

</details>
    
+ <details><summary>Przykład użycia funkcji USER.login (z parametrami)</summary>

  ```cs
  NameValueCollection Parameters = new NameValueCollection {
      { "username", "jan" },
      { "password", "pass1234" }
  };
  
  dynamic d = API.Query("LOGIN", Parameters);
  ```
  
  Zwrócona zawartość
  
  ```json
  {
    "Data": {
        "Id": "2",
        "Username": "jan",
        "Password": "pass1234"
    },
    "Info": "",
    "Status": "SUCCESS"
  }
  ```
    
</details>

## Dostępna funkcjonalność

### Obsługa API

+ <details>
  <summary>API.version</summary>
  
  Zwraca informacje o aktualnie używanej wersji API  

</details>

### Obsługa obiektów

Aktualnie dostępne są obiekty typu **USER**

Wszystkie wymienione powyżej obiekty obługiwane są w ten sam sposób i posiadają funkcje:
* get - pobieranie danych wg id
* put - dodawanie (gdy ide nie istnieje lub nie ma takiego w bazie) lub aktualizowanie danych (gdy istnieje wskazany id)
* del - usuwanie danych wg określonego id
* filter - Filtrowanie danych wg własnych parametrów

### Obsługa obiektów

+ <details><summary>OBJECT.put</summary>

  Dodaje nowego lub edytuje istniejącego użytkownika. Warunkiem edycji jest podanie parametru id > 0

  *Obiekt oczekiwany:*

  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  id | int | Identyfikator użytkownika
  username | string | Nazwa użytkownika
  password | string | Hasło użytkownika
  firstname | string | Imię
  lastname | string | Nazwisko
  type | int | Typ użytkownika

</details>

+ <details><summary>OBJECT.del</summary>

  Usuwa istniejącego użytkownika
  
  *Obiekt oczekiwany:*
  
  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  id * | int | Identyfikator użytkownika wymagany
  
  *Obiekt zwracany:*
  
  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  id | int | Identyfikator użytkownika
  
</details>

+ <details><summary>OBJECT.get</summary>

  Zwraca istniejącego użytkownika
  
  *Obiekt oczekiwany:*
  
  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  id * | int | Identyfikator użytkownika wymagany
    
</details>

+ <details><summary>OBJECT.filter</summary>
  
  Filtrowanie danych połączone z sortowaniem i limitowaniem.<br>
  Ogólnie zasady są takie same jak w MySQL
  
  *Obiekt oczekiwany:*
  
  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  where | NameValueCollection | Parametry wyszukiwania, gdzie <br> *Name* = "nazwa parametru" <br> *Value* = "operator wartość"<br>np: *{ "id", "> 3" }*
  limit | string | Ile rekodrów ma zwracać, od którego zacząć <br>np: *"10, 0"*
  order | string[] | Lista parametrów w kolejności sortowania łącznie z kierunkiem<br>np: *["id ASC", "firstname DESC"]*

</details>

### Opis dostępnych obiektów

+ <details><summary>API</summary>
    Dostępne funkcje: **version**
  </details
  
+ <details><summary>Obiekt USER</summary>

  ### Dostępne funkcje:
  **get, put, del, filter, login**
  
  
  + <details><summary>USER.login</summary>

    weryfikacja danych logowania

    *Obiekt oczekiwany:*

    Nazwa parametru | Typ danych | Opis
    --------------- | ---------- | ----
    username * | string | Nazwa użytkownika
    password * | string | Hasło użytkownika

  </details>
  
  ### Struktura
  
  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  id | int | Identyfikator użytkownika
  username | string | Nazwa użytkownika
  password | string | Hasło użytkownika
  firstname | string | Imię
  lastname | string | Nazwisko
  type | int | Typ użytkownika
  
  </details
