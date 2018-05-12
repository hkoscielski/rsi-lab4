using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfCallbackContract
{
    [ServiceContract(SessionMode = SessionMode.Required,
                    CallbackContract = typeof(ICallbackHandler))]
    public interface ICallbackKalkulator
    {
        [OperationContract(IsOneWay = true)]
        void Silnia(double n);
        [OperationContract(IsOneWay = true)]
        void ObliczCos(int sek);
    }   

    public interface ICallbackHandler
    {
        [OperationContract(IsOneWay = true)]
        void ZwrotSilnia(double result);
        [OperationContract(IsOneWay = true)]
        void ZwrotObliczCos(string result);
    }
}
