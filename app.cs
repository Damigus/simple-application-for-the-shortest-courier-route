using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        // Dane wejściowe
        string aktualnaLokalizacja = "Bukowa 2C, 84-200 Wejherowo"; // Startowa lokalizacja kuriera
        double maxGodziny = 4; // Ilość godzin pracy kuriera
        double maxPojemnosc = 40; // Pojemność pojazdu
        double srednieSpalanie = 8; // Średnie spalanie na 100km
        double cenaDiesla = 6.8; // Cena diesla

        // Lista punktów
        Dictionary<string, double> punktyDostawy = new Dictionary<string, double>
        {
            { "Armii Krajowej 101, 81-824 Sopot", 1 },
            { "Brzozowa 30, 84-240 Reda", 10 }, // W stringu adres, w doubleu waga paczki
            { "Starowiejska 4, 84-230 Rumia", 3 },
            { "Morska 81/87, 81-225 Gdynia", 40 },
            { "plac Mariacki 5, 31-042 Kraków", 5 }
        };

        // Lista punktów dostawy posortowana wg odległości
        List<KeyValuePair<string, double>> punktyDostawyPosortowane = punktyDostawy.OrderBy(x => PobierzOdleglosc(aktualnaLokalizacja, x.Key).Result).ToList();

        double calkowitaOdleglosc = 0;
        double calkowityCzas = 0;
        double calkowityKoszt = 0;
        double przebytyCzas = 0;
        double przebytaOdleglosc = 0;

        Console.WriteLine("Dostępne punkty dostawy:");

        for (int i = 0; i < punktyDostawyPosortowane.Count; i++)
        {
            var punkt = punktyDostawyPosortowane[i];
            Console.WriteLine($"{i + 1}. {punkt.Key}, Waga paczki: {punkt.Value} kg");
        }

        Console.WriteLine("Wybierz punkty dostawy, wpisując ich numery oddzielone przecinkami:");

        string[] wybranePunkty = Console.ReadLine().Split(',');

        foreach (var wybranyPunkt in wybranePunkty)
        {
            int index;
            if (int.TryParse(wybranyPunkt.Trim(), out index) && index >= 1 && index <= punktyDostawyPosortowane.Count)
            {
                var punkt = punktyDostawyPosortowane[index - 1];
                double odleglosc = PobierzOdleglosc(aktualnaLokalizacja, punkt.Key).Result;
                double czasPodrozy = PobierzCzasPodrozy(aktualnaLokalizacja, punkt.Key).Result;

                calkowitaOdleglosc += odleglosc;
                calkowityCzas += czasPodrozy;

                if (calkowityCzas <= maxGodziny)
                {
                    if (punkt.Value <= maxPojemnosc)
                    {
                        aktualnaLokalizacja = punkt.Key;
                        maxPojemnosc -= punkt.Value;
                        przebytyCzas += czasPodrozy;
                        przebytaOdleglosc += odleglosc;
                        Console.WriteLine($"Możemy dostarczyć paczkę o pojemności {punkt.Value} kg do {punkt.Key}");
                    }
                    else
                    {
                        Console.WriteLine($"Pojemność nie pozwala na dostarczenie paczki o wadze {punkt.Value} kg do {punkt.Key}");
                    }
                }
                else
                {
                    Console.WriteLine($"Nie wystarczy czasu na dostarczenie paczki do {punkt.Key}");
                    break;
                }
            }
            else
            {
                Console.WriteLine($"Niepoprawny numer punktu: {wybranyPunkt}. Pomijam.");
            }
        }

        calkowityKoszt = Math.Round((przebytaOdleglosc / 100) * srednieSpalanie * cenaDiesla, 2);
        Console.WriteLine($"Całkowita odległość trasy: {Math.Round(przebytaOdleglosc, 2)} km");
        Console.WriteLine($"Całkowity czas podróży: {Math.Round(przebytyCzas, 2)} godzin");
        Console.WriteLine($"Całkowity koszt trasy: {calkowityKoszt} zł");
    }

    static async Task<double> PobierzOdleglosc(string poczatek, string cel)
    {
        using (var klient = new HttpClient())
        {
            try
            {
                string kluczApi = "klucz_api"; // Klucz API Google Maps
                var odpowiedz = await klient.GetAsync($"https://maps.googleapis.com/maps/api/distancematrix/json?origins={poczatek}&destinations={cel}&departure_time=now&units=meters&key={kluczApi}");
                odpowiedz.EnsureSuccessStatusCode();

                var trescOdpowiedzi = await odpowiedz.Content.ReadAsStringAsync();
                dynamic dane = JsonConvert.DeserializeObject(trescOdpowiedzi);

                if (dane.status == "OK")
                {
                    double odlegloscWMetrach = (double)dane.rows[0].elements[0].distance.value;
                    double odlegloscWKilometrach = odlegloscWMetrach / 1000;

                    return odlegloscWKilometrach;
                }
                else
                {
                    Console.WriteLine("Błąd API.");
                    return -1; // Zwróć wartość ujemną, aby wskazać błąd
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Wystąpił błąd podczas wykonywania żądania: {e.Message}");
                return -1; // Błąd
            }
        }
    }

    static async Task<double> PobierzCzasPodrozy(string poczatek, string cel)
    {
        using (var klient = new HttpClient())
        {
            try
            {
                string kluczApi = "AIzaSyD_cVqE7-Hz_3-uVu2iMZO0yOHnDEqdI4E"; // Klucz API Google Maps
                var odpowiedz = await klient.GetAsync($"https://maps.googleapis.com/maps/api/directions/json?origin={poczatek}&destination={cel}&key={kluczApi}");
                odpowiedz.EnsureSuccessStatusCode();

                var trescOdpowiedzi = await odpowiedz.Content.ReadAsStringAsync();
                dynamic dane = JsonConvert.DeserializeObject(trescOdpowiedzi);

                if (dane.status == "OK")
                {
                    double czasPodrozyWSekundach = (double)dane.routes[0].legs[0].duration.value;
                    double czasPodrozyWGodzinach = czasPodrozyWSekundach / 3600;

                    return czasPodrozyWGodzinach;
                }
                else
                {
                    Console.WriteLine("Błąd API.");
                    return -1; // Zwróć wartość ujemną, aby wskazać błąd
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Wystąpił błąd: {e.Message}");
                return -1; // Błąd
            }
        }
    }
}