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
        private const string DESCRIPTION_FILE_EXTENSION = "_opis.txt";

        public ResponseFileMessage GetFile(RequestFileMessage request)
        {
            ResponseFileMessage result = new ResponseFileMessage();
            string filename = request.filename;
            FileStream myFile;
            Console.WriteLine("-->Wywolano GetMStream");
            string filePath = Path.Combine(System.Environment.CurrentDirectory, "files\\" + filename);
            Console.WriteLine("filePath: {0}", filePath);
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

            result.filename = filename;
            result.size = myFile.Length;
            result.data = myFile;
            return result;
        }

        public ResponseFileInfoMessage[] GetFilesInfo()
        {
            List<ResponseFileInfoMessage> results = new List<ResponseFileInfoMessage>();
            string mainPath = Path.Combine(System.Environment.CurrentDirectory, "files\\");
            Console.WriteLine("mainPath: {0}", mainPath);
            string path = mainPath + "*";
            //foreach (string filename in Directory.GetFiles(path).Except(Directory.GetFiles(path, "*_opis.txt")).Select(Path.GetFileName))
            Console.WriteLine("Liczba plikow: {0}", Directory.GetFiles(mainPath).Select(Path.GetFileName));
            foreach (string name in Directory.GetFiles(mainPath).Select(Path.GetFileName))
            {
                Console.WriteLine("name: {0}", name);
                Console.WriteLine("descname: {0}", Path.Combine(System.Environment.CurrentDirectory, "descriptions\\" + Path.GetFileNameWithoutExtension(name) + DESCRIPTION_FILE_EXTENSION));

                ResponseFileInfoMessage result = new ResponseFileInfoMessage
                {
                    filename = name,
                    description = GetText(Path.Combine(System.Environment.CurrentDirectory, "descriptions\\" + Path.GetFileNameWithoutExtension(name) + DESCRIPTION_FILE_EXTENSION))
                };
                results.Add(result);
            }
            return results.ToArray();        
        }

        public ResponseUploadFileMessage UploadFile(RequestUploadFileMessage request)
        {
            ResponseUploadFileMessage result = new ResponseUploadFileMessage();            
            Console.WriteLine("-->Wywolano UploadFile");
            string filePath = Path.Combine(System.Environment.CurrentDirectory, "files\\" + request.filename);
            string descriptionPath = Path.Combine(System.Environment.CurrentDirectory, "descriptions\\" + Path.GetFileNameWithoutExtension(request.filename) + DESCRIPTION_FILE_EXTENSION);
            //wyjatek na wypadek bledu otwarcia pliku
            try
            {
                SaveFile(request.data, filePath);
                SaveText(request.description, descriptionPath);
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

        private static void SaveText(string text, string path)
        {
            File.WriteAllText(path, text);
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
