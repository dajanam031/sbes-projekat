﻿using Contracts;
using SecurityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    class Program
    {

        static void Main(string[] args)
        {
            // ocekivani serverski sertifikat

            string serverCertCN = "pubsubserver";
            string signCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name) + "_sign";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople,
                StoreLocation.LocalMachine, serverCertCN);

            X509Certificate2 signCert = CertManager.GetCertificateFromStorage(StoreName.My,
                StoreLocation.LocalMachine, signCertCN);

            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:5353/PubService"),
                                      new X509CertificateEndpointIdentity(srvCert));

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
                    try
                    {
                        UnicodeEncoding encoding = new UnicodeEncoding();
                        byte[] data = encoding.GetBytes(alarm.ToString());
                        byte[] signature = DigitalSignature.Create(data, HashAlgorithm.SHA1, signCert);
                        proxy.Send(alarm);
                    } catch(Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                    
                }
               
            }

            Console.ReadLine();
        }
       
    }

}
