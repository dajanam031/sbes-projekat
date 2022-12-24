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
                Console.WriteLine("Konekcija uspostavljena");
                Console.WriteLine("Tema za vaš tekst : ");
                Console.WriteLine("1. Sport");
                Console.WriteLine("2. Muzika");
                Console.WriteLine("3. Politika");
                Console.WriteLine("4. Film");
                Console.WriteLine("Unesite broj teme : ");
                string brojTeme=Console.ReadLine();
                int br;
                if(Int32.TryParse(brojTeme,out br))
                {
                    Console.WriteLine("Unesite tekst : ");
                    string text = Console.ReadLine();
                    proxy.Send(brojTeme,text);
                }
                else
                {
                    Console.WriteLine("Pogrešan broj, probajte ponovo!");
                }
            }

            Console.ReadLine();
        }
       
    }

}
