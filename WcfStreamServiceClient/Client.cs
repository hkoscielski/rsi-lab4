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
    /// Klasa klienta. Pozwala na interakcje z użytkownikim udostępniając mu tekstowe menu z możliwymi operacjami do wyboru.    
    /// 
    /// Autor: 228141, Konrad Jakubowski.
    /// </summary>
    class Client
    {
        private StrumienClient client;

        private const int LIST = 1;
        private const int UPLOAD = 2;
        private const int DOWNLOAD = 3;
        //private const int REMOVE = 4;
        private const int EXIT = 5;

        /// <summary>
        /// Konstruktor klienta.
        /// </summary>
        /// <param name="client">Instancja WCF proxy</param>
        public Client(StrumienClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Metoda odpowiedzialna za uruchomienie programu dla klienta.
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
                        AddTranslation();
                        break;
                    case UPLOAD:
                        SearchTranslation();
                        break;
                    case DOWNLOAD:
                        ModifyTranslation();
                        break;
                    //case REMOVE:
                    //    RemoveTranslation();
                    //    break;
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
            //Console.WriteLine("{0}. Usuń tłumaczenie", REMOVE);
            Console.WriteLine("{0}. Wyjście.", EXIT);
        }

        /// <summary>
        /// Metoda pomocnicza odpowiadająca za interakcję użytkownika z programem
        /// pozwalająca na podanie przez użytkownika numeru opcji w menu. 
        /// Podawanie opcji następuje dopóki użytkownik nie wprowadzi liczby.
        /// </summary>
        /// <returns>int numer opcji wybrany przez użytkownika</returns>
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

        private void ListOfFiles()
        {
            //pobiera listę plików z opisami

        }

        private void UploadFile()
        {
            string _directory = Path.Combine(Environment.CurrentDirectory, ".\\Uploads\\*");
            string[] _files = Directory.GetFiles(_directory);

            for (int i = 0; i < _files.Length; i++)
            {
                Console.WriteLine("[{0}]. {1}", i, _files[i]);
            }
            int _option = ReadOption();

            if (_option > _files.Length - 1)
            {
                Console.WriteLine("Nie ma pliku o tym indeksie!");
                return;
            }

            //START PRZESYŁANIE PLIKU
            Console.WriteLine("Podaj opis przesyłanego pliku:");
            string _opis = Console.ReadLine();

            
        }

        private void DownloadFile()
        {
            //pobiera plik z serwera
            Console.WriteLine("Podaj nazwę pliku do pobrania.");
            string _nazwa = Console.ReadLine();

            ResponseFileMessage _stream = client.GetStream(_nazwa);
            string _directory = Path.Combine(Environment.CurrentDirectory, ".\\Downloads\\", _nazwa);
            SaveFile(_stream, _directory);
        }

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
        /// Metoda pomocnicza odpowiadająca za interakcje użytkownika z programem 
        /// pozwalająca na dodanie nowego tłumaczenia do słownika. 
        /// </summary>
        private void AddTranslation()
        {
            Console.WriteLine("Podaj polskie słowo:");
            string _polishWord = Console.ReadLine();
            Console.WriteLine("Podaj jego angielskie tłumaczenie:");
            string _englishWord = Console.ReadLine();

            if (this.client.AddTranslate(_polishWord, _englishWord))
                Console.WriteLine("Pomyslnie dodano słowo");
            else
                Console.WriteLine("Tłumaczenie już istnieje");
        }

        /// <summary>
        /// Metoda pomocnicza odpowiadająca za interakcje użytkownika z programem 
        /// pozwalająca na wyszukanie tłumaczenia w słowniku.
        /// </summary>
        private void SearchTranslation()
        {
            Console.WriteLine("Podaj polskie słowo, które chcesz wyszukać:");
            string _polishWord = Console.ReadLine();
            string[] _translation = this.client.SearchTranslate(_polishWord);

            foreach (string s in _translation)
                Console.WriteLine(s);

            if (_translation.Length == 0)
                Console.WriteLine("Nie znaleziono tlumaczenia");

        }

        /// <summary>
        /// Metoda pomocnicza odpowiadająca za interakcje użytkownika z programem 
        /// pozwalająca na modyfikację tłumaczenia w słowniku.
        /// </summary>
        private void ModifyTranslation()
        {
            Console.WriteLine("Podaj polskie słowo, którego tłumaczenie chcesz zmodyfikować:");
            string _polishWord = Console.ReadLine();
            string[] _translation = this.client.SearchTranslate(_polishWord);
            if (_translation.Length == 0)
            {
                Console.WriteLine("Nie znaleziono tlumaczenia");
            }
            else
            {
                Console.WriteLine("Podaj nowe tłumaczenie");
                string _newTranslation = Console.ReadLine();
                if (this.client.ModifyTranslate(_polishWord, _newTranslation))
                {
                    Console.WriteLine("Pomyślnie zmodyfikowano tłumaczenie");
                }
                else
                {
                    Console.WriteLine("Nie udało się zmienić tłumaczenia");
                }
            }
        }

        /// <summary>
        /// Metoda pomocnicza odpowiadająca za interakcje użytkownika z programem 
        /// pozwalająca na usunięcie tłumaczenia ze słownika.
        /// </summary>
        private void RemoveTranslation()
        {
            Console.WriteLine("Podaj polskie słowo, które chcesz usunąć ze słownika:");
            string _polishWord = Console.ReadLine();
            string[] _translation = this.client.SearchTranslate(_polishWord);
            if (_translation.Length == 0)
            {
                Console.WriteLine("Nie znaleziono tlumaczenia");
            }
            else
            {
                this.client.RemoveTranslate(_polishWord);
                Console.WriteLine("Pomyślnie usunięto tłumaczenie");
            }
        }

        /// <summary>
        /// Główna metoda klasy, która uruchamia program dla klienta.
        /// </summary>
        /// <param name="args">string[] parametry wywołania (w zadaniu nie są wykorzystywane)</param>
        static void Main(string[] args)
        {
            StrumienClient dictionaryClient = new StrumienClient();
            Client client = new Client(dictionaryClient);
            client.RunClient();
        }
    }
}