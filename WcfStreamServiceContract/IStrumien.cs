using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

/// <summary>
/// Obszar nazw dotyczący projektu, który zawiera interfejs kontraktu serwisu i jego implementację.
/// Autor: 228172, Hubert Kościelski.
/// </summary>
namespace WcfStreamServiceContract
{
    /// <summary>
    /// Interfejs kontraktu. Zawiera sygnatury metod, które powinny być implementowane przez kontrakt.
    /// Autor: 228172, Hubert Kościelski.
    /// </summary>
    [ServiceContract]
    public interface IStrumien
    {   
        /// <summary>
        /// Operacja kontraktu.
        /// Metoda powinna pobierać pożądany plik z serwera. 
        /// </summary>
        /// <param name="request">RequestFileMessage Specjalny typ żądania, zawierający nazwę pliku.</param>
        /// <returns>ResponseFileMessage Specjalny typ odpowiedzi, zawierający nazwę pliku, jego rozmiar, opis, jak i sam plik.</returns>
        [OperationContract]
        ResponseFileMessage GetFile(RequestFileMessage request);

        /// <summary>
        /// Operacja kontraktu.
        /// Metoda powinna pobierać listę wszystkich plików przechowywanych an serwerze.
        /// </summary>
        /// <returns>ResponseFileInfoMessage[] Tablica zawierająca wszystkie pliki przechoywane na serwerze. Każdy element zawiera nazwę i opis pliku.</returns>
        [OperationContract]
        ResponseFileInfoMessage[] GetFilesInfo();

        /// <summary>
        /// Operacja kontraktu.
        /// Metoda powinna wysyłać na serwer wybrany plik.
        /// </summary>
        /// <param name="request">RequestUploadFileMessage Specjalny typ pliku, zawierający nazwę pliku, jego opis, jak i sam plik.</param>
        /// <returns>ResponseUploadFileMessage Specjalny typ odpowiedzi. Zawiera informację o tym czy operacja wysyłania się powiodła.</returns>
        [OperationContract]
        ResponseUploadFileMessage UploadFile(RequestUploadFileMessage request);
    }

    /// <summary>
    /// Specjalny klasa dla kontraktu. Wykorzystywana jako zapytanie przesyłane do serwera.
    /// Zawiera nazwę pliku.
    /// </summary>
    [MessageContract]
    public class RequestFileMessage
    {
        /// <summary>
        /// string Nazwa pliku.
        /// </summary>
        [MessageBodyMember]
        public string filename;
    }

    /// <summary>
    /// Specjalny klasa dla kontraktu. Wykorzystywana jako zapytanie przesyłane do serwera.
    /// Zawiera nazwę pliku, opis i strumień pliku.
    /// </summary>
    [MessageContract]
    public class RequestUploadFileMessage
    {
        /// <summary>
        /// string Nazwa pliku.
        /// </summary>
        [MessageHeader]
        public string filename;
        /// <summary>
        /// string Opis pliku.
        /// </summary>
        [MessageHeader]
        public string description;
        /// <summary>
        /// Stream Strumień pliku.
        /// </summary>
        [MessageBodyMember]
        public Stream data;
    }

    /// <summary>
    /// Specjalny klasa dla kontraktu. Wykorzystywana jako odpowiedź zwracana przez serwer.
    /// Zawiera informację o tym czy wysyłanie pliku się powiodło.
    /// </summary>
    [MessageContract]
    public class ResponseUploadFileMessage
    {
        /// <summary>
        /// bool Informacja o tym, czy operacja się powiodła.
        /// </summary>
        [MessageBodyMember]
        public bool uploadSuccess;
    }

    /// <summary>
    /// Specjalny klasa dla kontraktu. Wykorzystywana jako odpowiedź zwracana przez serwer.
    /// Zawiera nazwę pliku, rozmiar pliku, opis pliku i strumień (dane) pliku.
    /// </summary>
    [MessageContract]
    public class ResponseFileMessage
    {
        /// <summary>
        /// string Nazwa pliku.
        /// </summary>
        [MessageHeader]
        public string filename;
        /// <summary>
        /// long Rozmiar pliku.
        /// </summary>
        [MessageHeader]
        public long size;
        /// <summary>
        /// string Opis pliku.
        /// </summary>
        [MessageHeader]
        public string description;
        /// <summary>
        /// Stream Strumień pliku.
        /// </summary>
        [MessageBodyMember]
        public Stream data;        
    }

    /// <summary>
    /// Specjalny klasa dla kontraktu. Wykorzystywana jako odpowiedź zwracana przez serwer.
    /// Zawiera nazwę pliku i opis pliku.
    /// </summary>
    [MessageContract]
    public class ResponseFileInfoMessage
    {
        /// <summary>
        /// string Nazwa pliku.
        /// </summary>
        [MessageHeader]
        public string filename;
        /// <summary>
        /// string Opis pliku.
        /// </summary>
        [MessageBodyMember]
        public string description;
    }
}
