using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfStreamServiceClient.ServiceReference1;
using System.IO;

/// <summary>
/// Obszar nazw dotyczący projektu, który zawiera implementację klienta.
/// Autor: 228141, Konrad Jakubowski.
/// </summary>
namespace WcfStreamServiceClient
{
    /// <summary>
    /// Klasa klienta. Pozwala na interakcje z użytkownikiem udostępniając mu tekstowe menu z możliwymi operacjami do wyboru.    
    /// 
    /// Autor: 228141, Konrad Jakubowski.
    /// </summary>
    class Client
    {
        private StrumienClient client;

        private const int LIST = 1;
        private const int UPLOAD = 2;
        private const int DOWNLOAD = 3;        
        private const int EXIT = 4;

        /// <summary>
        /// Konstruktor klienta.
        /// </summary>
        /// <param name="client">StrumienClient Instancja Strumienia.</param>
        public Client(StrumienClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Metoda stanowiąca główną pętlę programu klienta.
        /// Wyświetla dostępne opcje dla użytkownika (poprzez wywołanie pomocniczej metody).
        /// Zarządza wywoływaniem pomocniczych metod.
        /// </summary>
        void RunClient()
        {
            bool _isExit = false;
            int _option;

            while (!_isExit)
            {
                ShowOptions();
                _option = ReadOption();

                switch (_option)
                {
                    case LIST:
                        GetListOfFiles();
                        break;
                    case UPLOAD:
                        UploadFile();
                        break;
                    case DOWNLOAD:
                        DownloadFile();
                        break;                    
                    case EXIT:
                        _isExit = true;
                        break;
                    default:
                        Console.WriteLine("Nie ma takiej opcji!");
                        break;
                }
            }
        }

        /// <summary>
        /// Metoda pomocnicza odpowiedzialna za wyświetlenie w konsoli dostępnych operacji do wyboru dla użytkownika.
        /// </summary>
        private void ShowOptions()
        {
            Console.WriteLine("Menu użytkownika:");
            Console.WriteLine("{0}. Wyświetl listę plików.", LIST);
            Console.WriteLine("{0}. Wyślij plik.", UPLOAD);
            Console.WriteLine("{0}. Pobierz plik.", DOWNLOAD);            
            Console.WriteLine("{0}. Wyjście.", EXIT);
        }

        /// <summary>
        /// Metoda pomocnicza odpowiadająca za interakcję użytkownika z programem
        /// pozwalająca na podanie przez użytkownika numeru opcji w menu. 
        /// Podawanie opcji następuje dopóki użytkownik nie wprowadzi liczby.
        /// </summary>
        /// <returns>int Numer opcji wybranej przez użytkownika</returns>
        private int ReadOption()
        {
            string _line;
            int _option = -1;
            bool _isFirstRead = true;

            do
            {
                if (!_isFirstRead)
                {
                    Console.WriteLine("Opcja musi mieć wartość liczbową!");
                }
                Console.WriteLine("Podaj opcję:");
                _line = Console.ReadLine();
                _isFirstRead = false;
            }
            while (!int.TryParse(_line, out _option));

            return _option;
        }

        /// <summary>
        /// Metoda wykorzystująca Strumień w celu pobrania danych nt. listy plików przechowywanych na serwerze.
        /// Po pobraniu drukuje na ekranie konsoli pobraną listę plików lub informuje o braku plików.
        /// </summary>
        private void GetListOfFiles()
        {           
           ResponseFileInfoMessage[] _filesInfo = this.client.GetFilesInfo();
            Console.WriteLine("Pomyslnie pobrano liste plikow");
           if (_filesInfo.Length > 0)
           {
               Console.WriteLine("Pliki dostępne na serwerze:");
               for (int i = 0; i < _filesInfo.Length; i++)
               {
                    Console.WriteLine("[{0}]. {1} - {2}", i + 1, _filesInfo[i].filename, _filesInfo[i].description);
               }
           }
           else
           {
               Console.WriteLine("Nie ma żadnych plików na serwerze");
           }
        }

        /// <summary>
        /// Metoda wysyłająca wybrany plik na serwer.
        /// Wpierw pobiera listę plików z folderu 'Uploads', następnie umożliwia użytkownikowi wybranie 1 z przechowywanych tam plików. 
        /// Po wybraniu pliku, użytkownik ma możliwość podania opisu pliku.
        /// Następnie metoda wykorzystuje Strumień w celu wysłania pliku na serwer.
        /// Po próbie przesłania informuje użytkownika o powodzeniu operacji.
        /// </summary>
        private void UploadFile()
        {
            string _directory = Path.Combine(Environment.CurrentDirectory, "Uploads\\");            
            string[] _files = Directory.GetFiles(_directory).ToArray();                        

            if(_files.Length > 0)
            {
                Console.WriteLine("Pliki do przesłania:");
                for(int i = 0; i < _files.Length; i++)
                {
                    Console.WriteLine("[{0}]. {1}", i, Path.GetFileName(_files[i]));
                }
                Console.Write("Podaj numer pliku, który chcesz przesłać: ");
                int _fileIndex = ReadOption();
                if (_fileIndex > _files.Length)
                {
                    Console.WriteLine("Nie ma pliku o tym indeksie!");
                    return;
                }
                else
                {
                    Console.WriteLine("Podaj opis przesyłanego pliku:");
                    string _description = Console.ReadLine();
                    Stream stream = FileToStream(_files[_fileIndex]);         
                    
                    if (this.client.UploadFile(_description, Path.GetFileName(_files[_fileIndex]), stream))                    
                    {
                        Console.WriteLine("Pomyślnie przesłano plik");
                    }
                    else
                    {
                        Console.WriteLine("Nie udało się przesłać pliku. Spróbuj ponownie później");
                    }
                }
            }
            else
            {
                Console.WriteLine("Brak plików do przesłania!");
            }            
        }

        /// <summary>
        /// Metoda umożliwiająca pobranie plików z serwera.
        /// Pobranie pliku odbywa się poprzez podanie przez użytkownika nazwy pliku, który chce pobrać.
        /// Następnie wysyła do serwera żądanie o pobranie podanego pliku. 
        /// Po pobraniu informuje o tym użytkownika, następnie wywołuje metodę pomocniczą w celu zapisania pliku.
        /// </summary>
        private void DownloadFile()
        {
            //pobiera plik z serwera
            Console.WriteLine("Podaj nazwę pliku do pobrania.");
            string _filename = Console.ReadLine();
            Stream _data;
            long _size;
            string _description = this.client.GetFile(ref _filename, out _size, out _data);
            Console.WriteLine("Pomyslnie pobrano plik");
            string _directory = Path.Combine(Environment.CurrentDirectory, "Downloads\\" + _filename);            
            SaveFile(_data, _directory);
        }

        /// <summary>
        /// Metoda pomocnicza umożliwiająca zapisanie pliku ze strumienia do wskazanej ścieżki.
        /// Zapisuje plik porcjami.
        /// </summary>
        /// <param name="instream">Stream Strumień, który ma zostać zapisany</param>
        /// <param name="filePath">string Ścieżka, do której ma zostać zapisany plik.</param>
        static void SaveFile(Stream instream, string filePath)
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

        /// <summary>
        /// Metoda pomocnicza mająca za zadanie odczytać plik ze wskazanej ścieżki i zwrócić ten plik w postaci Strumienia.
        /// </summary>
        /// <param name="filePath">string Ścieżka do pliku, który zostanie zwrócony w postaci strumienia.</param>
        /// <returns></returns>
        static Stream FileToStream(string filePath)
        {            
            return File.Open(filePath, FileMode.Open, FileAccess.Read);
        }

        /// <summary>
        /// Główna metoda klasy, która uruchamia główną pętlę programu klienta.
        /// </summary>
        /// <param name="args">string[] Parametry wywołania (nie są wykorzystywane).</param>
        static void Main(string[] args)
        {
            StrumienClient dictionaryClient = new StrumienClient();
            Client client = new Client(dictionaryClient);
            client.RunClient();
        }
    }
}