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
            // 3 kor utan kiirja az reszpontokat / kor + osszpontot , talan leaderboard


            string[] szavak = new string[]
            {
                "ember",
                "körte",
                "kulcs",
                "madár",
                "füzet",
            };

            string mostaniSzo = UjSzo(szavak, r);
            int korSzam = 1;
            int pontSzam = 0;
            // Console.WriteLine(mostaniSzo, mostaniSzo.Length); // csak tesztelesre 

            string jatekosSzo = "";

            for (int jatekosProba = 0; jatekosProba < 5 && korSzam <= 3;  jatekosProba++)
            {
                jatekosSzo = BekerUjSzo(jatekosSzo);
                if (EltalaltaE(jatekosSzo, mostaniSzo) == true)
                {
                    pontSzam += 5 - jatekosProba;
                    if (korSzam != 3)
                        Console.WriteLine($"{korSzam}. kör nyert! // {jatekosProba} hiba a körben // {pontSzam} pont");
                    else
                    {
                       Console.WriteLine($"{korSzam}. kör nyert! // {jatekosProba} hiba körben // {pontSzam} pont // meccs vege");
                        // itt kell lementeni majd a leaderboardho a statot
                    }

                    korSzam++;
                    mostaniSzo = UjSzo(szavak, r);
                    jatekosProba = -1; // ha eltalalja megnoveli a korszamot, uj szót ker, és 5 uj hiba lehetoseget kap
                }
            }
            Console.ReadKey();
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
