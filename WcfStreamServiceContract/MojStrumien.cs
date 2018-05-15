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
        public ResponseFileMessage GetFile(RequestFileMessage request)
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

        public List<ResponseFileInfoMessage> GetFilesInfo()
        {
            List<ResponseFileInfoMessage> results = new List<ResponseFileInfoMessage>();
            string mainPath = Path.Combine(System.Environment.CurrentDirectory, ".\\files\\");
            string path = mainPath + "*";
            //foreach (string filename in Directory.GetFiles(path).Except(Directory.GetFiles(path, "*_opis.txt")).Select(Path.GetFileName))
            foreach (string filename in Directory.GetFiles(path).Except(Directory.GetFiles(mainPath, "*_opis.txt")).Select(Path.GetFileName))
            {
                ResponseFileInfoMessage result = new ResponseFileInfoMessage
                {
                    nazwa = filename,
                    opis = GetText(System.Environment.CurrentDirectory + "\\" + filename + "*_opis.txt")
                };
            }
            return results;        
        }

        public ResponseUploadFileMessage UploadFile(RequestUploadFileMessage request)
        {
            ResponseUploadFileMessage result = new ResponseUploadFileMessage();
            string nazwa = request.nazwa;
            FileStream myFile;
            Console.WriteLine("-->Wywolano UploadFile");
            string filePath = Path.Combine(System.Environment.CurrentDirectory, ".\\files\\" + nazwa);
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

            result.uploadSuccess = true;
            return wynik;
        }

        private string GetText(string path)
        {
            return string.Join(" ", File.ReadAllLines(path));            
        }
   }
}
