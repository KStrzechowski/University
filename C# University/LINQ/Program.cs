using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab14
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Database database = Database.GetInstance();

            /* WAŻNE:

                * Twoim zadaniem jest stworzenie sześciu zapytań LINQ dla bazy danych związanych z lotami.
                * Zapytania można tworzyć z dowolnej kolejności.
                * Wymagane jest korzystanie ze składni wyrażeń kwerendowych LINQ (składnia podobna do SQL).
                * Notacji kropkowej można używac jedynie gdy jest niezbędna.
                * Kiedy potrzeba można użyć metod agregujących takich jak Sum, Min, Count, Average oraz innych.
                * Nie można korzystać z pętli for, foreach lub innych w celu przetworzenia danych.
                * Kiedy wymagane ograniczone wyniki (pierwsze trzy wiersze) można użyć metody Take.
                * Powinieneś skorzystać z klas anonimowych w celu projekcji danych z zapytnia.

                * Licenses Table - Przechowuje informacje o licencjach wszystkich użytkowników: (Kategoria,WażnyOd,WażdyDo) - (Category,ValidSince,ExpirationDate).
                * People Table - Przechowuje informacje o członkach załogi: (Imię,Nazwisko,LicencjaID) - (Name,Surname,LicenseID).
                * Aircrafts Table - Przechowuje informacje o samolotach: (NumerRejestracyjny,Marka,Waga,Pojemność) - (RegistrationNumber,Brand,Weight,Capacity).
                * Airports Table - Przechowuje informacje o lotniskach: (Państwo,Miasto,KodIATA,KodICAO) - (Country,City,CodeIATA,CodeICAO).
                * Flights Table - Przechowuje informacje o lotach: (NumerLotu,SamolotID,LotniskoPoczątkoweID,LotniskoKońcodeID,CzasLotu) - (FlightNumber,AircraftID,AirportOriginID,AirportDestinationID,Duration).
                * Crews Table - Przechowuje informacje o załodze danego lotu: (LotID,OsobaID,Rola,Wynagrodzenie) - (FlightID,PersonID,Role,Salary).
            */

            /* ETAP_1 (0.5 Pts)
                * Wypisz Nazwisko oraz Imię (Surname oraz Name) wszystkich osób niebiorących udziału w żadnym locie.
                * Wskazówka:
                    * Użyj JOIN .. INTO oraz metody Count w celu pogrupowania wyników oraz wyznaczenia ich liczności.
            */
            {
                Console.WriteLine("--------------- ETAP_1 (0.5 Pts) ---------------");

                /* TUTAJ IMPLEMENTACJA */
                {
                    var seq1 = from person in database.People
                               join crew in database.Crews on person.ID equals crew.PersonID
                               into seq
                               let k = seq.Count()
                               where k == 0
                               select (person.Surname, person.Name);

                    foreach (var x in seq1)
                    {
                        Console.WriteLine($"{x.Surname} {x.Name} ");
                    }

                }

                Console.WriteLine();
            }

            /* ETAP_2 (1.0 Pts)
                * Dla każdego lotu posiadającego załogę wypisz NumerLotu oraz WypłatęCałkowitą (FlightNumber oraz TotalSalary) osób obsługujących dany lot. Gdzie:
                    * WypłataCałkowita danego lotu to suma wypłat wszystkich członków załogi.
                * Wyniki powinny być posortowane rosnąco po sumie wypłaty.
                * Wskazówka:
                    * Użyj GROUP oraz metody Sum w celu pogrupowania wyników oraz wyznaczenia sumy wynagrodzenia.
                    * Użyj LET w celu uniknięciu wielokrotnego wyliczania wypłaty całkowitej.
            */
            {
                Console.WriteLine("--------------- ETAP_2 (0.5 Pts) ---------------");

                /* TUTAJ IMPLEMENTACJA */
                {
                    var seq2 = from flight in database.Flights
                               join crew in database.Crews on flight.ID equals crew.FlightID
                               let k = new { flight.FlightNumber, crew.Salary }
                               group k by k.FlightNumber
                               into seq
                               let l = new { seq.Key, Count = seq.Sum(x => x.Salary) }
                               orderby l.Count
                               select l;

                    foreach (var x in seq2)
                    {
                        Console.WriteLine($" {x.Key} - {x.Count} PLN ");
                    }
                }

                Console.WriteLine();
            }


            /* ETAP_3 (1.0 Pts)
                * Wypisz KodIATA oraz liczbę startujących z danego lotniska samolotów (CodeIATA - N Airplanes).
                * Wskazówka:
                    * Użyj GROUP oraz metody Count w celu pogrupowania wyników oraz wyznaczenia liczby samolotów.
            */
            {
                Console.WriteLine("--------------- ETAP_3 (0.5 Pts) ---------------");

                /* TUTAJ IMPLEMENTACJA */
                {
                    var seq3 = from airport in database.Airports
                               join flight in database.Flights on airport.ID equals flight.AirportOriginID
                               let k = new { airport.CodeIATA, flight.AirportOriginID }
                               group k by k.CodeIATA
                               into seq
                               let l = new { seq.Key, Count = seq.Count() }
                               select l;

                    foreach (var x in seq3)
                    {
                        Console.WriteLine($" {x.Key} - {x.Count} PLN ");
                    }
                }

                Console.WriteLine();
            }

            /* ETAP_4 (1.0 Pts)
                * Wypisz LotniskoPoczątkowe,LotniskoKońcowe,NumerLotu,NumerRejestracyjny (OriginAirport,DestinationAirport,FlightNumber,Registration) dla wszystkich lotów samolotem o pojemności poniżej 170 osób. Gdzie:
                    * LotniskoPoczątkowe to kod IATA lotniska wylotu.
                    * LotniskoKońcowe to kod IATA lotniska przylotu.
                * Wyniki powinny być posortowane roznąco po numerze lotu.
            */
            {
                Console.WriteLine("--------------- ETAP_4 (1.0 Pts) ---------------");

                /* TUTAJ IMPLEMENTACJA */
                {
                    int maxCapacity = 170;

                    var seq4 = from flight in database.Flights
                               join aircraft in database.Aircrafts on flight.AircraftID equals aircraft.ID
                               where aircraft.Capacity < maxCapacity
                               join airport in database.Airports on flight.AirportOriginID equals airport.ID
                               join airport2 in database.Airports on flight.AirportDestinationID equals airport2.ID
                               let k = new { CodeIATA1 = airport.CodeIATA, CodeIATA2 = airport2.CodeIATA , Registration = aircraft.Registration, Number = flight.FlightNumber }
                               orderby k.Number
                               select k;

                    foreach (var x in seq4)
                    {
                        Console.WriteLine($" {x.CodeIATA1} -> {x.CodeIATA2} by PLANE {x.Registration} Flight Number {x.Number}");
                    }

                }

                Console.WriteLine();
            }


            /* ETAP_5 (1.0 Pts)
                * Wypisz NumerRejestracyjny oraz ŚredniCzasLotu (Registration,PlaneAverageTime) dla wszystkich lotów. Gdzie:
                    * ŚredniCzasLotu to średnia długość wszystkich lotów wykonana danym samolotem.
                * Wyniki powinny być posortowane malejąco po średnim czasie lotu.
            */
            {
                Console.WriteLine("--------------- ETAP_5 (1.0 Pts) ---------------");

                /* TUTAJ IMPLEMENTACJA */
                {
                    var seq5 = from flight in database.Flights
                               join aircraft in database.Aircrafts on flight.AircraftID equals aircraft.ID
                               let k = new { Registration = aircraft.Registration, flight.Duration }
                               group k by k.Registration
                               into seq
                               let l = new { Registration = seq.Key, Count = seq.Average(x => x.Duration.Hours * 60 + x.Duration.Minutes) }
                               orderby l.Count descending
                               select l;

                    foreach (var x in seq5)
                    {
                        Console.WriteLine($" {x.Registration} - {x.Count}  ");
                    }
                }

                Console.WriteLine();
            }


            /* ETAP_6 (1.5 Pts) 3 osoby z najkrótszym lotem.
                * Dla trzech osób z maksymalną średnią czasu lotu oraz licencją ważną dłużej niż 7 lat wypisz Nazwisko,Imię,TypLicencji,ŚredniCzasLotu (Surname,Name,AircraftCategory,AverageFlightTime). Gdzie:
                    * ŚredniCzasLotu opisuje średnią długość wszystkich lotów odbytych przez daną osobę.
                * Wyniki powinny być posortowane malejąco po średnim czasie lotu.
            */
            {
                Console.WriteLine("--------------- ETAP_6 (1.5 Pts) ---------------");

                /* TUTAJ IMPLEMENTACJA */
                {
                    TimeSpan minValidDate = new TimeSpan(7 * 365, 0, 0);
                    var seq6 = (from crew in database.Crews
                                join flight in database.Flights on crew.FlightID equals flight.ID
                                let k = new { crew.PersonID, flight.Duration }
                                group k by k.PersonID
                               into seq
                                let l = new { seq.Key, Count = seq.Average(x => x.Duration.Hours * 60 + x.Duration.Minutes) }
                                orderby l.Count descending
                                select l).Take(3);

                    foreach (var x in seq6)
                    {
                        Console.WriteLine($" {x.Key} - {x.Count}  ");
                    }

                }

                Console.WriteLine();
            }
        }
    }
}
