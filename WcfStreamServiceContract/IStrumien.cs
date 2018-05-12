using System;
using System.Collections.Generic;
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
        System.IO.Stream GetStream(string nazwa);
    }
}
