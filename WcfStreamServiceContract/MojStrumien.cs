using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfStreamServiceContract
{
    public class MojStrumien : IStrumien
    {
        public Stream GetStream(string nazwa)
        {
            FileStream myFile;
            Console.WriteLine("-->Wywolano GetStream");
            string filePath = Path.Combine(System.Environment.CurrentDirectory, ".\\" + nazwa);
            //wyjatek na wypadek bledu otwarcia pliku
            try
            {
                myFile = File.OpenRead(filePath);
            }
            catch(IOException ex)
            {
                Console.WriteLine(string.Format("Wyjatek otwarcia pliku {0} :", filePath));
                Console.WriteLine(ex.ToString());
                throw ex;
            }
            return myFile;
        }

        public ResponseFileMessage GetMStream(RequestFileMessage request)
        {
            ResponseFileMessage wynik = new ResponseFileMessage();
            string nazwa = request.nazwa1;
            FileStream myFile;
            Console.WriteLine("-->Wywolano GetMStream");
            string filePath = Path.Combine(System.Environment.CurrentDirectory, ".\\" + nazwa);
            //wyjatek na wypadek bledu otwarcia pliku
            try
            {
                myFile = File.OpenRead(filePath);
            }
            catch (IOException ex)
            {
                Console.WriteLine(string.Format("Wyjatek otwarcia pliku {0} :", filePath));
                Console.WriteLine(ex.ToString());
                throw ex;
            }

            wynik.nazwa2 = "klient2.jpg";
            wynik.rozmiar = myFile.Length;
            wynik.dane = myFile;
            return wynik;
        }
    }
}
