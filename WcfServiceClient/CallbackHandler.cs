using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfServiceClient.ServiceReference2;

namespace WcfCallbackContract
{
    class CallbackHandler : ICallbackKalkulatorCallback
    {
        public void ZwrotSilnia(double result)
        {
            //tu mozna wykorzystac result
            Console.WriteLine(" Silnia = {0}", result);
        }

        public void ZwrotObliczCos(string info)
        {
            //tu mozna wykorzystac info
            Console.WriteLine(" Obliczenia: {0}", info);
        }
    }
}
