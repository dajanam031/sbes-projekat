using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    class Program
    {

        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:5353/PubService";

            using (ClientProxy proxy = new ClientProxy(binding, address))
            {
                while (true)
                {
                    Console.WriteLine("Konekcija uspostavljena\n(exit za izlaz iz programa)");
                    Console.WriteLine("Alarm za koji želite da objavite poruku : ");
                    Console.WriteLine("Unesite vreme generisanja (h:m:s) : ");
                    string time = Console.ReadLine();
                    if (time == "exit")
                        break;
                    Console.WriteLine("Unesite datum generisanja (month/day/year) : ");
                    string date = Console.ReadLine();

                    DateTime dateTime;
                    if (DateTime.TryParse(date + " " + time, out dateTime))
                    {
                        Console.WriteLine(dateTime);
                    }
                    else
                    {
                        Console.WriteLine("Datum nije dobar probajte opet ");

                    }
                    Console.WriteLine("Poruka o alarmu : ");
                    string poruka = Console.ReadLine();
                    Console.WriteLine("Rizik : (1-100)");
                    string rizik = Console.ReadLine();
                    int rizikInt;
                    if (!(Int32.TryParse(rizik, out rizikInt)))
                    {
                        Console.WriteLine("Rizik je broj");
                    }
                    Alarm alarm = new Alarm(dateTime, poruka, rizikInt);
                    proxy.Send(alarm);
                }
               
            }

            Console.ReadLine();
        }
       
    }

}
