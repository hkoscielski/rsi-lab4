using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WcfServiceClient.ServiceReference1;

namespace WcfServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Krok 1: Utworzenie instancji WCF proxy.
            SerwisClient client1 = new SerwisClient("WSHttpBinding_ISerwis");

            // Krok 2: Wywolanie operacji uslugi.
            Console.WriteLine("...wywoluje funkcja 1:");
            client1.Funkcja1("Klient1");
            Thread.Sleep(10);
            Console.WriteLine("...kontynuacja po funkcji 1");
            Console.WriteLine("...wywoluje funkcja 2:");
            client1.Funkcja2("Klient1");
            Thread.Sleep(10);
            Console.WriteLine("...kontynuacja po funkcji 2");
            Console.WriteLine("...wywoluje funkcja 1:");
            client1.Funkcja1("Klient1");
            Thread.Sleep(10);
            Console.WriteLine("...kontynuacja po funkcji 1");
            client1.Close();
            Console.WriteLine("KONIEC KLIENT1");
        }
    }
}
