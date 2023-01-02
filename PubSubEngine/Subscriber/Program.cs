using Contracts;
using Encrypting;
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
        private static List<Alarm> AllAlarmsForThisSub = new List<Alarm>();
        static void Main(string[] args)
        {
            // ocekivani serverski sertifikat

            string serverCertCN = "pubsubserver";

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople,
               StoreLocation.LocalMachine, serverCertCN);

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:8000/SubService"), new X509CertificateEndpointIdentity(srvCert));

            using (ClientProxy proxy = new ClientProxy(binding, address))
            {
                Console.WriteLine("Konekcija uspostavljena\n(exit za izlaz iz programa)");
                Console.WriteLine("Opseg rizika alarm za koji želite da prijavite (min-max) : ");

                string rizik = Console.ReadLine();
                    
                string rizikMin = rizik.Split('-')[0];
                string rizikMax = rizik.Split('-')[1];
                int minRizik = Int32.Parse(rizikMin);
                int maxRizik = Int32.Parse(rizikMax);
                string key = SecretKey.LoadKey("keyFile.txt");
 
                while (true)
                {

                    Dictionary<byte[],byte[]> alarms = proxy.ForwardAlarm(minRizik, maxRizik);
                    if (alarms != null && alarms.Count>0)
                    {
                        List<Alarm> NewAlarms = new List<Alarm>();
                        foreach (byte[] a in alarms.Values)
                        {
                            if (AllAlarmsForThisSub.Count == alarms.Count)
                            {
                                NewAlarms.Clear();
                            }
                            else if ((AllAlarmsForThisSub.Count == 0) && (alarms.Count > 0))
                            {
                                foreach (KeyValuePair<byte[], byte[]> al in alarms)
                                {
                                    Alarm alarmForSub = AESInECB.DecryptAlarm(al.Value, key);
                                    AllAlarmsForThisSub.Add(alarmForSub);
                                    NewAlarms.Add(alarmForSub);
                                }
                            }
                            else
                            {
                                List<byte[]> alarmsInList = alarms.Values.ToList();
                                for (int i = AllAlarmsForThisSub.Count - 1; i < alarmsInList.Count; i++)
                                {
                                    Alarm alarmForSub = AESInECB.DecryptAlarm(alarmsInList[i], key);
                                    AllAlarmsForThisSub.Add(alarmForSub);
                                    NewAlarms.Add(alarmForSub);
                                }
                            }
                        }
                        if (NewAlarms.Count > 0)
                        {
                            foreach (Alarm a in NewAlarms)
                            {
                                Console.WriteLine("Alarm : ");
                                Console.WriteLine("Vreme generisanja : " + a.GeneratingTime);
                                Console.WriteLine("Poruka o alarmu : " + a.MessegAlarm);
                                Console.WriteLine("Rizik : " + a.Risk);
                            }
                            NewAlarms.Clear();
                        }
                    }
                }
            }

        }
    }
}
