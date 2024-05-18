using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Wordle
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // pontozas: elsore - 5 pont
            // pontozas: masodjara - 4 pont
            // pontozas: harmadjara - 3 pont
            // pontozas: negyedjere - 2 pont
            // pontozas: otodjere - 1 pont
            // pontozas: nem talalja ki ennyi proba utan akkor 0 pont -> uj szo uj kor
            // 3 kor utan kiirja az reszpontokat / kor + osszpontot , talan leaderboard

            JatekMenu();
            Console.ReadKey();
        }
        private static void JatekMenu()
        {
            Console.Clear();
            Console.WriteLine("Főmenü");
            Console.WriteLine("1. Játék Kezdete \t 2. Előző Pontszámok Megtekintése \t 3. Vissza a Főmenübe");
            // Add toggle admin menu !!!!!!

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    Jatek();
                    JatekMenu();
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    ElozmenyMenu();
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    // FOmenu
                    break;
                default: JatekMenu(); break;
            }
        }

        private static void ElozmenyMenu()
        {
            Console.Clear();
            Console.WriteLine("Előző Pontszámok: ");
            Elozmenyek();
            Console.WriteLine("1. Vissza A Játékmenübe \t 2. Vissza A Főmenübe");
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    JatekMenu();
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    // Fomenu
                    break;
                default: ElozmenyMenu(); break;
            }
        }

        private static void Elozmenyek()
        {
            string eleresiUt = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

            List<string> sorok = new List<string>();
            sorok = File.ReadAllLines(eleresiUt + @"\Ranglista.txt").ToList();
            foreach (string sor in sorok)
            {
                Console.WriteLine(sor);
            }
        }

        private static void Jatek()
        {
            Console.Clear();
            Random r = new Random();
            string[] szavak = new string[]
            {
                "ember",
                "körte",
                "kulcs",
                "madár",
                "füzet",
                "ablak",
                "aláír",
                "ápoló",
                "aszal",
                "átlép",
                "bájos",
                "bátor",
                "bogár",
                "borjú",
                "búvár",
                "cégér",
                "csiga",
                "darab",
                "dobál",
                "ébred",
                "gomba",
                "gyárt",
                "habos",
                "harap",
                "kakas",
                "kamra",
                "kocka",
                "kutya",
                "lakás",
                "levél",
                "napok",
                "nemes",
                "olíva",
                "repül",
            };

            string mostaniSzo = UjSzo(szavak, r);
            int korSzam = 1;
            int pontSzam = 0;
            // Console.WriteLine(mostaniSzo, mostaniSzo.Length); // csak tesztelesre 

            string jatekosSzo = "";

            for (int jatekosProba = 0; jatekosProba < 5 && korSzam <= 3; jatekosProba++)
            {
                jatekosSzo = BekerUjSzo(jatekosSzo);
                Console.WriteLine(mostaniSzo, mostaniSzo.Length); // csak tesztelesre
                if (EltalaltaE(jatekosSzo, mostaniSzo) == true)
                {
                    pontSzam += 5 - jatekosProba;
                    if (korSzam != 3)
                        Console.WriteLine($"{korSzam}. kör nyert! // {jatekosProba} hiba a körben // {pontSzam} pont");
                    else
                    {
                        Console.WriteLine($"{korSzam}. kör nyert! // {pontSzam} pont // Nyomjon meg egy gombot a továbblépéshez");
                        Console.ReadKey();

                        string eleresiUt = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Ranglista.txt";

                        List<string> sorok = new List<string>();
                        sorok = File.ReadAllLines(eleresiUt).ToList();
                        sorok.Add($"{pontSzam} Pont // //{DateTime.Now}"); // player neve kell majd ide !!!!!
                        File.WriteAllLines(eleresiUt, sorok);
                    }

                    korSzam++;
                    mostaniSzo = UjSzo(szavak, r);
                    jatekosProba = -1; // ha eltalalja megnoveli a korszamot, uj szót ker, és 5 uj hiba lehetoseget kap
                }
            }
        }

        private static string BekerUjSzo(string jatekosSzo)
        {
            do
            {
                Console.Write("Tipp (5 karakter): ");
                string input = Console.ReadLine();
                if (input.Length != 5)
                    Console.WriteLine("nem 5 karakter hosszu");
                jatekosSzo = input;
            } while (jatekosSzo.Length != 5);

            return jatekosSzo;
        }

        private static string UjSzo(string[] szavak, Random r)
        {
            return szavak[r.Next(szavak.Length)];
        }

        private static bool EltalaltaE(string jatekosSzo, string mostaniSzo)
        {
            int betukJoHelyen = 0; // betuk ami jo helyen van (zold)
            int betukNemjoHelyen = 0; // betuk amik benne vannak a szoban de nem jo helyen (sarga)
            for (int i = 0; i < jatekosSzo.Length; i++)
            {
                if (jatekosSzo[i] == mostaniSzo[i]) // jo helyen van e a betu
                {
                    betukJoHelyen++;
                    Console.WriteLine($"A(z) \"{jatekosSzo[i]}\" betű jó helyen van! ({i + 1}. karakter)");
                }

                for (int j = 0; j < jatekosSzo.Length; j++)
                {
                    if (jatekosSzo[i] == mostaniSzo[j] && jatekosSzo[i] != mostaniSzo[i])
                    {
                        betukNemjoHelyen++;
                        Console.WriteLine($"A(z) \"{jatekosSzo[i]}\" betű benne van de nem jó helyen van! ({i + 1}. karakter)");
                    }
                }
            }

            Console.WriteLine($"{betukJoHelyen} darab betű van jó helyen és {betukNemjoHelyen} db betű van benne de nem jó helyen!");

            if (betukJoHelyen == 5)
                return true;
            else
                return false;
        }
    }
}
