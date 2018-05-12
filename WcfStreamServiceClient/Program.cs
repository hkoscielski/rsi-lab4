using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfStreamServiceClient.ServiceReference1;

namespace WcfStreamServiceClient
{
    class Program
    {
        static void ZapiszPlik(Stream instream, string filePath)
        {
            const int bufferLength = 8192; //dlugosc bufora 8KB
            int byteCount = 0; //licznik
            int counter = 0; //licznik pomocniczy

            byte[] buffer = new byte[bufferLength];
            Console.WriteLine("--> Zapisuje plik {0}", filePath);
            FileStream outstream = File.Open(filePath, FileMode.Create, FileAccess.Write);

            //zapisywanie danych porcjami
            while((counter = instream.Read(buffer, 0, bufferLength)) > 0)
            {
                outstream.Write(buffer, 0, counter);
                Console.Write(".{0}", counter); //wypisywanie info po kazdej czesci
                byteCount += counter;
            }
            Console.WriteLine();
            Console.WriteLine("Zapisano {0} bajtow", byteCount);

            outstream.Close();
            instream.Close();
            Console.WriteLine();
            Console.WriteLine("--> Plik {0} zapisany", filePath);
        }

        static void Main(string[] args)
        {
            StrumienClient client2 = new StrumienClient();
            string filePath = Path.Combine(System.Environment.CurrentDirectory, "klient.jpg");
            Console.WriteLine("Wywoluje GetStream()");
            Stream stream2 = client2.GetStream("image.jpg");
            ZapiszPlik(stream2, filePath);

            client2.Close();
            Console.WriteLine();
            Console.WriteLine("Nacisnij <ENTER> aby zakonczyc.");
            Console.ReadLine();
        }
    }
}
