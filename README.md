# NSAPI [![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0) ![GitHub watchers](https://img.shields.io/github/watchers/mnatanek/nsapi?style=social)


Projekt na potrzeby szkoleń z przedmiotu PAI

**UWAGI do dokumentacji!**
* w dokumentacji założono dołączenie przestrzenie nazw NSAPI
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

## Dostępna funkcjonalność

### Obsługa obiektów

Aktualnie dostępne są obiekty typu **CONFIG, USER**

Wszystkie wymienione powyżej obiekty obługiwane są w ten sam sposób i posiadają funkcje:
* get - Pobieranie danych wg wskazanego parametru id
* put - Dodawanie danych (gdy parametr id nie istnieje lub nie ma takiego w bazie) / aktualizowanie danych (gdy istnieje rekord o wskazanym parametrze id)
* del - Usuwanie danych wg wskazanego parametru id
* filter - Filtrowanie danych wg wskazanych parametrów

Wywałanie funkcji dla danego obiektu polega na wpisaniu jego nazwy a następnie po kropce funkcji, która ma zostać wykonana. W przypadku podania nieobsługiwanego obiektu lub funkcji zostanie zwrócony stosowny komunikat.
<br>Przykład wywołania np:

  ```cs
  dynamic d = API.Query("API.version");
  ```
  
  ```cs
  dynamic d = API.Query("USER.del", new Params { {"id", 9} } );
  ```

### Uniwersalne funkcje obiektów

+ <details><summary>OBJECT.put</summary>

  Dodaje nowy lub edytuje istniejący obiekt o ścisle określonych parametrach. Parametry dla każdego obiektu są szczegółowo opisane w niniejszej dokumentacji w sekcji **Obiekty i funkcje**. Warunkiem edycji jest podanie parametru o nazwie *id*

</details>

+ <details><summary>OBJECT.del</summary>

  Usuwa istniejący obiekt o ścisle określonym parametrze o nazwie *id*.
  
</details>

+ <details><summary>OBJECT.get</summary>

  Zwraca istniejący obiekt o ścisle określonym parametrze o nazwie *id*.
    
</details>

+ <details><summary>OBJECT.filter</summary>
  
  Pobieranie danych z nałożonym filtrowaniem, sortowaniem i limitowaniem.<br>
  Ogólnie zasady są takie same jak w MySQL
  
  *Obiekt oczekiwany:*
  
  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  where | Params | Parametry wyszukiwania, gdzie <br> *Name* = "nazwa parametru" <br> *Value* = "operator wartość"<br>np: *{ "id", "> 3" }*
  limit | string | Ile rekodrów ma zwracać, od którego zacząć <br>np: *"10, 0"*
  order | string[] | Lista parametrów w kolejności sortowania łącznie z kierunkiem sortowania<br>np: *["id ASC", "firstname DESC"]*

</details>

### Przykłady użycia

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
  Params p = new Params {
      { "username", "jan" },
      { "password", "pass1234" }
  };
  
  dynamic d = API.Query("USER.login", p);
  ```
  
  Zwrócona zawartość
  
  ```json
  {
    "Data": [{
        "Id": "2",
        "Username": "jan",
        "Password": "pass1234"
    }],
    "Info": "",
    "Status": "SUCCESS"
  }
  ```
    
</details>

## Obiekty i funkcje

+ <details><summary>Obiekt API</summary>
  
  ### Dostępne funkcje: 
  
  <details><summary>API.version</summary>
    
    Zwraca informacje o aktualnie używanej wersji API  
    
  </details>
  
  <details><summary>API.token</summary>
    
    Ustawia token uzyskany w ramach licencji na użytkowanie aplikacji. Bez ustawienia tokena, dalsza praca będzie bezskuteczna.
    
    *Obiekt oczekiwany:*
    
    Nazwa parametru | Typ danych | Opis
    --------------- | ---------- | ----
    code | string | Nazwa użytkownika z dostępem do API
    token | string | Token przyznany w trakcie udzialania licencji dostępowej
    
  </details>
  
</details>

+ <details><summary>Obiekt CONFIG</summary>

  ### Struktura

  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  id | int | Identyfikator użytkownika
  type | string | Rodzaj informacji
  name | string | Nazwa ustawienia
  value | string | Wartość

</details>

+ <details><summary>Obiekt USER</summary>

  ### Struktura

  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  id | int | Identyfikator użytkownika
  type | int | Typ użytkownika <br> *0 - USER* <br> *1 - EMPLOYEE (wartość domyślna)* <br> *2 - SUPER ADMIN*
  username | string | Nazwa użytkownika
  password | string | Hasło użytkownika
  firstname | string | Imię
  lastname | string | Nazwisko
  
  ### Dostępne funkcje:
  
  + <details><summary>USER.login</summary>

    weryfikacja danych logowania

    *Obiekt oczekiwany:*

    Nazwa parametru | Typ danych | Opis
    --------------- | ---------- | ----
    username * | string | Nazwa użytkownika
    password * | string | Hasło użytkownika

  </details>

</details>
