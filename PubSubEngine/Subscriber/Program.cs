using Contracts;
using SecurityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            // ocekivani serverski sertifikat

            string serverCertCN = "pubsubserver";

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople,
                StoreLocation.LocalMachine, serverCertCN);

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:5353/SubService"),
                                     new X509CertificateEndpointIdentity(srvCert));

            using (ClientProxy proxy = new ClientProxy(binding, address))
            {
                while (true)
                {
                    Console.WriteLine("Konekcija uspostavljena\n(exit za izlaz iz programa)");
                    Console.WriteLine("Opseg rizika alarm za koji želite da prijavite (min-max) : ");

                    string rizik = Console.ReadLine();
                    if (rizik == "exit")
                        break;
                    string rizikMin = rizik.Split('-')[0];
                    string rizikMax = rizik.Split('-')[1];
                    int minRizik = Int32.Parse(rizikMin);
                    int maxRizik = Int32.Parse(rizikMax);

                    List<Alarm> alarms = proxy.ForwardAlarm(minRizik, maxRizik);
                    foreach (Alarm a in alarms)
                    {
                        Console.WriteLine("Alarm : ");
                        Console.WriteLine("Vreme generisanja : " + a.GeneratingTime);
                        Console.WriteLine("Poruka o alarmu : " + a.MessegAlarm);
                        Console.WriteLine("Rizik : " + a.Risk);
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
