# 🚚 Planowanie Trasy Kuriera z Google Maps API

Aplikacja konsolowa w języku C#, która umożliwia planowanie trasy dostaw kuriera na podstawie:
- wybranych punktów dostawy,
- dostępnego czasu pracy kuriera,
- pojemności pojazdu,
- rzeczywistych danych o odległości i czasie przejazdu pobranych z Google Maps API.

---

## 📦 Funkcjonalności

✅ Pobieranie rzeczywistej odległości i czasu przejazdu z Google Maps  
✅ Uwzględnianie limitu czasu pracy kuriera  
✅ Kontrola maksymalnej pojemności pojazdu  
✅ Kalkulacja kosztu paliwa na podstawie dystansu i spalania  
✅ Wstępne sortowanie punktów dostawy według odległości  
✅ Prosta interaktywna obsługa w konsoli  

---

## 🛠️ Technologie

- C# (.NET Core lub .NET Framework)
- [Newtonsoft.Json](https://www.newtonsoft.com/json)
- Google Maps Distance Matrix API
- Google Maps Directions API

---

## ⚙️ Wymagania

- .NET Core SDK lub Visual Studio
- Aktywny klucz Google Maps API z dostępem do:
  - Distance Matrix API
  - Directions API
- Połączenie internetowe

---

## 💻 Uruchomienie

1. **Sklonuj repozytorium:**

```bash
git clone https://github.com/twoj-uzytkownik/planowanie-trasy-kuriera.git
cd planowanie-trasy-kuriera
```

2. **Podmień klucz API:**

W pliku `Program.cs` w metodach `PobierzOdleglosc` i `PobierzCzasPodrozy` zastąp wartość zmiennej `kluczApi` swoim kluczem Google Maps API.

3. **Zbuduj i uruchom aplikację:**

Jeśli korzystasz z .NET Core:

```bash
dotnet build
dotnet run
```

---

## 📝 Sposób działania

1. Po uruchomieniu program wyświetli listę dostępnych punktów dostawy i wagę paczek.
2. Użytkownik wybiera numery punktów, które chce odwiedzić (oddzielone przecinkami).
3. Program w czasie rzeczywistym sprawdza możliwość dostarczenia paczek, uwzględniając czas pracy, pojemność pojazdu i odległość.
4. Po zakończeniu wyświetlana jest:
   - całkowita odległość trasy,
   - całkowity czas podróży,
   - szacowany koszt paliwa.

---

## 📊 Przykład działania

```
Dostępne punkty dostawy:
1. Armii Krajowej 101, 81-824 Sopot, Waga paczki: 1 kg
2. Brzozowa 30, 84-240 Reda, Waga paczki: 10 kg
3. Starowiejska 4, 84-230 Rumia, Waga paczki: 3 kg
4. Morska 81/87, 81-225 Gdynia, Waga paczki: 40 kg
5. plac Mariacki 5, 31-042 Kraków, Waga paczki: 5 kg

Wybierz punkty dostawy, wpisując ich numery oddzielone przecinkami:
1,3,4

Możemy dostarczyć paczkę o pojemności 1 kg do Armii Krajowej 101, 81-824 Sopot
Możemy dostarczyć paczkę o pojemności 3 kg do Starowiejska 4, 84-230 Rumia
Pojemność nie pozwala na dostarczenie paczki o wadze 40 kg do Morska 81/87, 81-225 Gdynia

Całkowita odległość trasy: 25.7 km
Całkowity czas podróży: 0.65 godzin
Całkowity koszt trasy: 13.98 zł
```

---

## ⚠️ Ważne

> Korzystanie z Google Maps API może generować koszty na Twoim koncie Google Cloud w zależności od ilości zapytań. Sprawdź szczegóły cennika na stronie [Google Cloud](https://cloud.google.com/maps-platform/pricing).

---

## 🛣️ Możliwości rozbudowy

- Automatyczna optymalizacja kolejności punktów dostawy (np. algorytmem najkrótszej trasy)
- Graficzna wizualizacja trasy
- Uwzględnianie okien czasowych w punktach dostawy
- Obsługa wielu kurierów i pojazdów

---

## 📄 Licencja

Projekt stworzony w celach edukacyjnych. Możesz modyfikować i rozwijać według własnych potrzeb.
