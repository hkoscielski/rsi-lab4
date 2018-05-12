using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using WcfServiceContract;

namespace WcfServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            // Krok 1 Utworz URI dla bazowego adresu serwisu.
            Uri baseAddress1 = new Uri("http://localhost:10004/MojSerwis");

            // Krok 2 Utworz instancje serwisu.
            ServiceHost mojHost1 = new ServiceHost(typeof(MojSerwis), baseAddress1);
            try
            {
                // Krok 3 Dodaj endpoint.
                ServiceEndpoint endpoint1 = mojHost1.AddServiceEndpoint(typeof(ISerwis), new WSHttpBinding(), "endpoint1");

                // Krok 4 Ustaw wymiane metadanych.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();               
                smb.HttpGetEnabled = true;
                mojHost1.Description.Behaviors.Add(smb);

                // Krok 5 Uruchom serwis.
                mojHost1.Open();
                Console.WriteLine("--->MojSerwis jest uruchomiony.");
                Console.WriteLine("--->Nacisnij <ENTER> aby zakonczyc.\n");
                Console.ReadLine();
                mojHost1.Close();
                Console.WriteLine("---> Serwis zakonczyl dzialanie.");
            }
            catch(CommunicationException ce)
            {
                Console.WriteLine("Wystapil wyjatek: {0}", ce.Message);
                mojHost1.Abort();
            }
        }
    }
}
