﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using WcfStreamServiceContract;

/// <summary>
/// Obszar nazw dotyczący projektu, który zawiera implementację serwera - hosta.
/// Autor: 228172, Hubert Kościelski.
/// </summary>
namespace WcfStreamServiceHost
{
    /// <summary>
    /// Główna klasa hosta. 
    /// Zawiera tylko 1 metodę, odpowiedzialną za uruchomienie serwera i jego późniejsze zamknięcie na żądanie użytkownika.
    /// Autor: 228172, Hubert Kościelski.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Jedyna metoda programu będącego hostem.
        /// Tworzy instancję serwisu, dodaje endpointy, ustawia metadane.
        /// Następnie uruchamia serwis i oczekuje na jego zamknięcie wskutek żądania użytkownika.
        /// </summary>
        /// <param name="args">string[] Parametry wywołania (nie są wykorzystywane).</param>
        static void Main(string[] args)
        {
            // Krok 1 Utworz URI dla bazowego adresu serwisu.
            Uri baseAddress = new Uri("http://192.168.43.82:50004/");

            // Krok 2 Utworz instancje serwisu.
            ServiceHost mojHost = new ServiceHost(typeof(MojStrumien), baseAddress);
            try
            {
                // Krok 3 Dodaj endpoint.
                BasicHttpBinding b = new BasicHttpBinding
                {
                    TransferMode = TransferMode.Streamed,
                    MaxReceivedMessageSize = 1000000000,
                    MaxBufferSize = 8192
                };
                ServiceEndpoint endpoint = mojHost.AddServiceEndpoint(typeof(IStrumien), b, baseAddress);

                // Krok 4 Ustaw wymiane metadanych.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                mojHost.Description.Behaviors.Add(smb);

                // Krok 5 Uruchom serwis.
                mojHost.Open();
                Console.WriteLine("--->MojStrumien jest uruchomiony.");
                Console.WriteLine("--->Nacisnij <ENTER> aby zakonczyc.\n");
                Console.ReadLine();
                mojHost.Close();
                Console.WriteLine("---> Serwis zakonczyl dzialanie.");
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("Wystapil wyjatek: {0}", ce.Message);
                mojHost.Abort();                
            }
        }
    }
}
