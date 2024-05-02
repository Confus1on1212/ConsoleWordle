using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Wordle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();

            // pontozas: elsore - 5 pont
            // pontozas: masodjara - 4 pont
            // pontozas: harmadjara - 3 pont
            // pontozas: negyedjere - 2 pont
            // pontozas: otodjere - 1 pont
            // pontozas: nem talalja ki ennyi proba utan akkor 0 pont -> uj szo uj kor
            // 3 kör
            // 3 kor utan kiirja az reszpontokat / kor + osszpontot , talan leaderboard
            // meg kell nezni hogy 5 betus legyen a szo amit tippel

            //Console.Write("Tipp: ");
            //string jatekosSzo = Console.ReadLine();
            //Console.WriteLine(Ellenoriz(jatekosSzo, mostaniSzo));

            string[] szavak = new string[]
            {
                "ember",
                "körte",
                "kulcs",
                "madár",
                "füzet",
            };

            string mostaniSzo = UjSzo(szavak, r);
            int korSzam = 0;
            Console.WriteLine(mostaniSzo + mostaniSzo.Length); // csak tesztelesre 

            // kulon kerdezes metodus 

            for (int jatekosProba = 0; jatekosProba < 5;  jatekosProba++)
            {
                string jatekosSzo = "";
                while (jatekosSzo.Length != 5) // ha nem 5 karakter hosszu kerjen ujat
                {
                    Console.Write("Tipp (5 karakter): ");
                    string input = Console.ReadLine();
                    Console.WriteLine("nem 5 karakter hosszu");
                    jatekosSzo = input;
                }
                

                if (Ellenoriz(jatekosSzo, mostaniSzo) != "Eltalalt!")
                {
                    // 
                }
                else
                {
                    Console.WriteLine($"{korSzam + 1}. kör nyert!");
                    korSzam++;
                    mostaniSzo = UjSzo(szavak, r);
                }
                // Console.WriteLine(Ellenoriz(jatekosSzo, mostaniSzo));
            }
            Console.ReadKey();
        }

        private static string UjSzo(string[] szavak, Random r)
        {
            
            return szavak[r.Next(szavak.Length)];
        }

        private static string Ellenoriz(string jatekosSzo, string mostaniSzo)
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
                if (betukJoHelyen == 5)
                    return $"Eltalalt!";

                for (int j = 0; j < jatekosSzo.Length; j++) 
                {
                    if (jatekosSzo[i] == mostaniSzo[j] && jatekosSzo[i] != mostaniSzo[i])
                    {
                        betukNemjoHelyen++;
                        Console.WriteLine($"A(z) \"{jatekosSzo[i]}\" betű benne van de nem jó helyen van! ({i + 1}. karakter)");
                    }
                }
            }

            return $"{betukJoHelyen} darab betű van jó helyen és {betukNemjoHelyen} db betű van benne de nem jó helyen!";
        }
    }
}
