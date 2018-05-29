using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

/// <summary>
/// Obszar nazw dotyczący projektu, który zawiera interfejs kontraktu serwisu i jego implementację.
/// Autor: 228141, Konrad Jakubowski.
/// </summary>
namespace WcfStreamServiceContract
{
    /// <summary>
    /// Klasa implementująca interfejs 'IStrumien'. 
    /// Obsługuje wszystkie metody zawarte w interfejsie.
    /// Autor: 228141, Konrad Jakubowski.
    /// </summary>
    public class MojStrumien : IStrumien
    {
        /// <summary>
        /// string Rozszerzenie pliku tekstowego stanowiącego opis głównego pliku.
        /// </summary>
        private const string DESCRIPTION_FILE_EXTENSION = "_opis.txt";

        /// <summary>
        /// Metoda pobierająca z serwera plik o określonej nazwie. 
        /// Metoda jest wywoływana po stronie serwera. Wyszukuje plik o zadanej nazwie w specjalnym katalogu przechowującym pliki i należącym do hosta.
        /// Nazwa pliku jest zawarta w typie argumentu: RequestFileMessage.
        /// </summary>
        /// <param name="request">RequestFileMessage Specjalny typ, w którym przechowywana jest Nazwa pliku do pobrania.</param>
        /// <returns>ResponseFileMessage Zwraca specjalny obiekt kontraktu, który zawiera nazwę pliku, rozmiar pliku, opis pliku i strumień (dane) pliku.</returns>
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

        /// <summary>
        /// Metoda pobierająca z serwera listę plików wraz z opisami. 
        /// Metoda jest wywoływana po stronie serwera. Wyszukuje wszystkie pliki w specjalnym katalogu przechowującym pliki i należącym do hosta.
        /// Lista plików jest zwracana w formie tablicy obiektów: ResponseFileInfoMessage.
        /// </summary>
        /// <returns>ResponseFileInfoMessage[] Tablica obiektów specjalnego typu zdefiniowanego dla kontraktu. Każdy obiekt zawiera Nazwę pliku i jego Opis.</returns>
        public ResponseFileInfoMessage[] GetFilesInfo()
        {
            List<ResponseFileInfoMessage> results = new List<ResponseFileInfoMessage>();
            string mainPath = Path.Combine(System.Environment.CurrentDirectory, "files\\");                 
            
            foreach (string name in Directory.GetFiles(mainPath).Select(Path.GetFileName))
            {                
                ResponseFileInfoMessage result = new ResponseFileInfoMessage
                {
                    filename = name,
                    description = GetText(Path.Combine(System.Environment.CurrentDirectory, "descriptions\\" + Path.GetFileNameWithoutExtension(name) + DESCRIPTION_FILE_EXTENSION))
                };
                results.Add(result);
            }
            return results.ToArray();        
        }

        /// <summary>
        /// Metoda wysyłająca na serwer plik pochodzący od klienta.
        /// Metoda jest wywoływana po stronie serwera. W specjalnym folderze zapisuje otrzymany plik.
        /// </summary>
        /// <param name="request">RequestUploadFileMessage Obiekt zawierający nazwę pliku, opis i strumień (dane) pliku.</param>
        /// <returns>ResponseUploadFileMessage Zwraca obiekt, którzy przechowuje informację o powodzeniu operacji (czy plik udało się zapisać).</returns>
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

        /// <summary>
        /// Metoda prywatna. Z podanej ścieżki (pliku), pobiera ciąg tekstowy w postaci 1 napisu.
        /// </summary>
        /// <param name="path">string Ścieżka pliku, z którego zostanie pobrany tekst.</param>
        /// <returns>string Napis pobrany z pliku.</returns>
        private static string GetText(string path)
        {
            return string.Join(" ", File.ReadAllLines(path));            
        }

        /// <summary>
        /// Metoda prywatna. Do podanej ścieżki, zapisuje zadany ciąg tekstowy.
        /// </summary>
        /// <param name="text">string Napis, który zostanie zapisany do pliku.</param>
        /// <param name="path">strign Ścieżka pliku, do którego zostanie zapisany napis.</param>
        private static void SaveText(string text, string path)
        {
            File.WriteAllText(path, text);
        }

        /// <summary>
        /// Metoda prywatna. Do podanej ścieżki, zapisuje plik zadany w postaci Strumienia.
        /// </summary>
        /// <param name="instream">Stream Strumień, który ma zostać zapisany jako plik.</param>
        /// <param name="filePath">string Ścieżka, do której zostanie zapisany strumień.</param>
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
