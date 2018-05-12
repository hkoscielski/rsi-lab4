using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceContract
{    
    [ServiceContract]
    public interface ISerwis
    {
        [OperationContract]
        void Funkcja(string s1);
        [OperationContract(IsOneWay = true)]
        void Funkcja2(string s2);
    }
}
