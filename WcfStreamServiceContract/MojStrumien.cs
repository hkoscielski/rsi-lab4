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
            string filePath = Path.Combine(System.Environment.CurrentDirectory, ".\\image.jpg");
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
    }
}
