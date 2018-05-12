using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace WcfServiceContract
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MojSerwis : ISerwis
    {
        void ISerwis.Funkcja(string s1)
        {
            Console.WriteLine("...{0}: funkcja1 - start", s1);
            Thread.Sleep(2000);
            Console.WriteLine("...{0}: funkcja1 - stop", s1);
            return;
        }

        void ISerwis.Funkcja2(string s2)
        {
            Console.WriteLine("...{0}: funkcja2 - start", s2);
            Thread.Sleep(2000);
            Console.WriteLine("...{0}: funkcja2 - stop", s2);
            return;
        }
    }
}
