using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfStreamServiceContract
{    
    [ServiceContract]
    public interface IStrumien
    {        
        [OperationContract]
        ResponseFileMessage GetFile(RequestFileMessage request);
        [OperationContract]
        List<ResponseFileInfoMessage> GetFilesInfo();
        [OperationContract]
        ResponseUploadFileMessage UploadFile(RequestUploadFileMessage request);
    }

    [MessageContract]
    public class RequestFileMessage
    {
        [MessageBodyMember]
        public string nazwa1;
    }

    [MessageContract]
    public class RequestUploadFileMessage
    {
        [MessageHeader]
        public string nazwa;
        [MessageHeader]
        public string opis;
        [MessageBodyMember]
        public Stream dane;
    }

    [MessageContract]
    public class ResponseUploadFileMessage
    {
        [MessageBodyMember]
        public bool uploadSuccess;
    }

    [MessageContract]
    public class ResponseFileMessage
    {
        [MessageHeader]
        public string nazwa2;
        [MessageHeader]
        public long rozmiar;
        [MessageHeader]
        public string opis;
        [MessageBodyMember]
        public Stream dane;        
    }

    [MessageContract]
    public class ResponseFileInfoMessage
    {
        [MessageHeader]
        public string nazwa;
        [MessageBodyMember]
        public string opis;
    }
}
