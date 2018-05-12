using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace WcfCallbackContract
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MojCallbackKalkulator : ICallbackKalkulator
    {
        double result;
        ICallbackHandler callback = null;

        public MojCallbackKalkulator()
        {
            callback = OperationContext.Current.GetCallbackChannel<ICallbackHandler>();
        }

        public void ObliczCos(int sek)
        {
            Console.WriteLine("...wywolano Oblicz({0})", sek);
            if (sek < 10)
                Thread.Sleep(sek * 1000);
            else
                Thread.Sleep(1000);
            callback.ZwrotObliczCos("Obliczenia trwaly " + (sek + 1) + "sekund(y)");
        }

        public void Silnia(double n)
        {
            Console.WriteLine("...wywolano Silnia({0})", n);
            Thread.Sleep(1000);
            result = 1;
            for (int i = 1; i <= n; i++)
                result *= i;
            callback.ZwrotSilnia(result);
        }
    }
}
