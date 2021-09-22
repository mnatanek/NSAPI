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

### Obsługa użytkowników


+ <details><summary>USER.put</summary>

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

  *Obiekt zwracany:*

  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  id | int | Identyfikator użytkownika
  username | string | Nazwa użytkownika
  password | string | Hasło użytkownika
  firstname | string | Imię
  lastname | string | Nazwisko
  type | int | Typ użytkownika

</details>

+ <details><summary>USER.del</summary>

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

+ <details><summary>USER.get</summary>

  Zwraca istniejącego użytkownika
  
  *Obiekt oczekiwany:*
  
  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  id * | int | Identyfikator użytkownika wymagany
  
  *Obiekt zwracany:*
  
  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  id | int | Identyfikator użytkownika
  username | string | Nazwa użytkownika
  password | string | Hasło użytkownika
  firstname | string | Imię
  lastname | string | Nazwisko
  type | int | Typ użytkownika
  
</details>

+ <details><summary>USER.login</summary>
  
  weryfikacja danych logowania
  
  *Obiekt oczekiwany:*
  
  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  username * | string | Nazwa użytkownika
  password * | string | Hasło użytkownika
  
  *Obiekt zwracany:*
  
  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  id | int | Identyfikator użytkownika
  username | string | Nazwa użytkownika
  password | string | Hasło użytkownika
  firstname | string | Imię
  lastname | string | Nazwisko
  type | int | Typ użytkownika

</details>
