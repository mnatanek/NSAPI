# NS-API
Projekt na potrzeby szkoleń z przedmiotu PAI

**UWAGI do dokumentacji!**
* znak __*__ w tabeli parametrów oznacza wymagalność danego parametru

### Przykład użycia funkcji LOGIN

```cs
NameValueCollection Parameters = new NameValueCollection {
    { "username", "jan" },
    { "password", "pass1234" }
};

dynamic d = API.Query("LOGIN", Parameters);
```


### Przykład zwracanego obiektu dla funkcji LOGIN

```json
{
  "Data": [
    {
      "Id": "2",
      "Username": "jan",
      "Password": "pass1234"
    }
  ],
  "Info": "",
  "Status": "OK"
}
```


### Opis obiektu zwracanego przez funkcje API

Nazwa parametru | Typ danych | Opis
--------------- | ---------- | ----
Data | dynamic | Dane zwracane przez funkcję API, różne w zależności od wykonywanej funkcji
Info | string | Informacja na temat wykonania danej funkcji
Status | enum | **SUCCESS** - w przypadku sukcesu<br> **ERROR** - w przypadku występienia błędu, dodatkowa informacja o błędzie przechowywana jest w zmiennej Info


### Dostępne funkcje

<details>
  <summary>VERSION</summary>
  
  Zwraca informacje o aktualnie używanej wersji API  

</details>

<details>
  <summary>LOGIN</summary>
  
  weryfikacja danych logowania
  
  *Obiekt oczekiwany:*
  
  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  username * | string | Nazwa użytkownika
  password * | string | Hasło użytkownika
  
  *Obiekt zwracany:*
  
  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  userid | int | Identyfikator użytkownika
  username | string | Nazwa użytkownika
  password | string | Hasło użytkownika
  firstname | string | Imię
  lastname | string | Nazwisko
  type | int | Typ użytkownika

</details>

<details>
  <summary>USER.ADD</summary>
  
  Dodaje nowego użytkownika do bazy
  
  *Obiekt oczekiwany:*

  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  username * | string | Nazwa użytkownika
  password * | string | Hasło użytkownika
  firstname | string | Imię
  lastname | string | Nazwisko
  type | int | Typ użytkownika

  *Obiekt zwracany:*

  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  userid | int | Identyfikator użytkownika
  username | string | Nazwa użytkownika
  password | string | Hasło użytkownika
  firstname | string | Imię
  lastname | string | Nazwisko
  type | int | Typ użytkownika

</details>

<details>
  <summary>USER.EDIT</summary>
  
  Edytuje istniejącego użytkownika
  
  *Obiekt oczekiwany:*

  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  userid * | int | Identyfikator użytkownika wymagany
  username | string | Nazwa użytkownika
  password | string | Hasło użytkownika
  firstname | string | Imię
  lastname | string | Nazwisko
  type | int | Typ użytkownika

  *Obiekt zwracany:*

  Nazwa parametru | Typ danych | Opis
  --------------- | ---------- | ----
  userid | int | Identyfikator użytkownika
  username | string | Nazwa użytkownika
  password | string | Hasło użytkownika
  firstname | string | Imię
  lastname | string | Nazwisko
  type | int | Typ użytkownika

</details>

