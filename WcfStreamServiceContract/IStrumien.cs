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
        ResponseFileInfoMessage[] GetFilesInfo();
        [OperationContract]
        ResponseUploadFileMessage UploadFile(RequestUploadFileMessage request);
    }

    [MessageContract]
    public class RequestFileMessage
    {
        [MessageBodyMember]
        public string filename;
    }

    [MessageContract]
    public class RequestUploadFileMessage
    {
        [MessageHeader]
        public string filename;
        [MessageHeader]
        public string description;
        [MessageBodyMember]
        public Stream data;
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
        public string filename;
        [MessageHeader]
        public long size;
        [MessageHeader]
        public string description;
        [MessageBodyMember]
        public Stream data;        
    }

    [MessageContract]
    public class ResponseFileInfoMessage
    {
        [MessageHeader]
        public string filename;
        [MessageBodyMember]
        public string description;
    }
}
