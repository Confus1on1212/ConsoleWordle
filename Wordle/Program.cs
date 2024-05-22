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
            // 3 kor utan kiirja az osszpontot 

            JatekMenu();
            Console.ReadKey();
        }
        private static void JatekMenu()
        {
            Console.Clear();
            Console.WriteLine("Főmenü");
            Console.WriteLine("1. Játék Kezdete \t 2. Előző Pontszámok Megtekintése \t 3. Vissza a Főmenübe");

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
            Elozmenyek();
            Console.WriteLine("1. Előzmények törlése \t 2. Vissza A Játékmenübe \t 3. Vissza A Főmenübe");
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    ElozmenyekTorlése();
                    ElozmenyMenu();
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    JatekMenu();
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    // fomenu
                    break;
                default: ElozmenyMenu(); break;
            }
        }

        private static void ElozmenyekTorlése()
        {
            string eleresiUt = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Elozmenyek.csv";

            File.Delete(eleresiUt);
            File.Create(eleresiUt).Close();
        }

        private static void Elozmenyek()
        {
            string eleresiUt = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Elozmenyek.csv";

            List<string> sorok = new List<string>();
            sorok = File.ReadAllLines(eleresiUt).ToList();
            if (sorok.Count != 0)
            {
                Console.WriteLine("Előző Pontszámok:");

                foreach (string sor in sorok)
                {
                    Console.WriteLine(string.Join(" ", sor.Split(';')));
                }
            }
            else
                Console.WriteLine("Még nincs lejátszott mértkőzés!");
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
                // Console.WriteLine(mostaniSzo, mostaniSzo.Length); // csak tesztelesre
                if (EltalaltaE(jatekosSzo, mostaniSzo, 5 - jatekosProba) == true) // HA nyeri a kört
                {
                    pontSzam += 5 - jatekosProba;
                    if (korSzam != 3)
                    {
                        Console.WriteLine($"{korSzam}. kör nyert! // {pontSzam} pont");
                    }

                    else // ha az utolsó kör
                    {
                        PontSzamMentese(pontSzam);
                        Console.WriteLine($"{korSzam}. kör nyert! // Nyomjon meg egy gombot a továbblépéshez");
                        Console.ReadKey();
                    }
                    korSzam++;
                    mostaniSzo = UjSzo(szavak, r);
                    jatekosProba = -1; // megnoveli a korszamot, uj szót ker, és 5 uj hiba lehetoseget kap
                }
                else if (jatekosProba == 4)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Nem találta el 5 próba alatt!");
                    Console.ResetColor();
                    Console.WriteLine($"Nyomjon meg egy gombot a következő körhöz! // {pontSzam} pont");
                    Console.ReadKey(true);
                    mostaniSzo = UjSzo(szavak, r);
                    jatekosProba = -1;
                }
            }
        }

        private static void PontSzamMentese(int pontSzam)
        {
            string eleresiUt = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Elozmenyek.csv";

            List<string> sorok = new List<string>();
            sorok = File.ReadAllLines(eleresiUt).ToList();
            sorok.Add($"Pont: {pontSzam};Név: [név];Idő: {DateTime.Now}"); // player neve kell majd ide !!!!!
            File.WriteAllLines(eleresiUt, sorok);
        }

        private static string BekerUjSzo(string jatekosSzo)
        {
            do
            {
                Console.Write("Tipp (5 karakter): ");
                string input = Console.ReadLine();
                if (input.Length != 5)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("A megadott szó nem 5 karakter hosszú");
                    Console.ResetColor();
                }
                jatekosSzo = input;
            } while (jatekosSzo.Length != 5);

            return jatekosSzo;
        }

        private static string UjSzo(string[] szavak, Random r)
        {
            return szavak[r.Next(szavak.Length)];
        }

        private static bool EltalaltaE(string jatekosSzo, string mostaniSzo, int probaMaradt)
        {
            int betukJoHelyen = 0; // betuk ami jo helyen van (zold)
            int betukNemjoHelyen = 0; // betuk amik benne vannak a szoban de nem jo helyen (sarga)
            for (int i = 0; i < jatekosSzo.Length; i++)
            {
                if (jatekosSzo[i] == mostaniSzo[i]) // jo helyen van e a betu
                {
                    betukJoHelyen++;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"A(z) \"{jatekosSzo[i]}\" betű jó helyen van! ({i + 1}. karakter)");
                    Console.ResetColor();
                    Console.WriteLine($"{probaMaradt - 1} esély van még");
                }

                for (int j = 0; j < jatekosSzo.Length; j++)
                {
                    if (jatekosSzo[i] == mostaniSzo[j] && jatekosSzo[i] != mostaniSzo[i])
                    {
                        betukNemjoHelyen++;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"A(z) \"{jatekosSzo[i]}\" betű benne van de nem jó helyen van! ({i + 1}. karakter)");
                        Console.ResetColor();
                        Console.WriteLine($"{probaMaradt - 1} esély van még");
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
