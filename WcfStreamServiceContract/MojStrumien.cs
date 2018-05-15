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
            ResponseFileMessage result = new ResponseFileMessage();
            string filename = request.filename;
            FileStream myFile;
            Console.WriteLine("-->Wywolano GetMStream");
            string filePath = Path.Combine(System.Environment.CurrentDirectory, ".\\" + filename);
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

            result.filename = filePath;
            result.size = myFile.Length;
            result.data = myFile;
            return result;
        }

        public ResponseFileInfoMessage[] GetFilesInfo()
        {
            List<ResponseFileInfoMessage> results = new List<ResponseFileInfoMessage>();
            string mainPath = Path.Combine(System.Environment.CurrentDirectory, ".\\files\\");
            string path = mainPath + "*";
            //foreach (string filename in Directory.GetFiles(path).Except(Directory.GetFiles(path, "*_opis.txt")).Select(Path.GetFileName))
            foreach (string name in Directory.GetFiles(path).Except(Directory.GetFiles(mainPath, "*_opis.txt")).Select(Path.GetFileName))
            {
                ResponseFileInfoMessage result = new ResponseFileInfoMessage
                {
                    filename = name,
                    description = GetText(System.Environment.CurrentDirectory + "\\" + name + "*_opis.txt")
                };
            }
            return results.ToArray();        
        }

        public ResponseUploadFileMessage UploadFile(RequestUploadFileMessage request)
        {
            ResponseUploadFileMessage result = new ResponseUploadFileMessage();
            string name = request.filename;            
            Console.WriteLine("-->Wywolano UploadFile");
            string filePath = Path.Combine(System.Environment.CurrentDirectory, ".\\files\\" + name);
            //wyjatek na wypadek bledu otwarcia pliku
            try
            {
                SaveFile(request.data, filePath);                 
                result.uploadSuccess = true;
            }
            catch (IOException ex)
            {                
                Console.WriteLine(ex.ToString());
                result.uploadSuccess = false;
                throw ex;
            }
            
            return result;
        }

        private static string GetText(string path)
        {
            return string.Join(" ", File.ReadAllLines(path));            
        }

        private static void SaveFile(Stream instream, string filePath)
        {
            const int bufferLength = 8192; //dlugosc bufora 8KB
            int byteCount = 0; //licznik
            int counter = 0; //licznik pomocniczy

            byte[] buffer = new byte[bufferLength];
            Console.WriteLine("--> Zapisuje plik {0}", filePath);
            FileStream outstream = File.Open(filePath, FileMode.Create, FileAccess.Write);

            //zapisywanie danych porcjami
            while ((counter = instream.Read(buffer, 0, bufferLength)) > 0)
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
    }
}
