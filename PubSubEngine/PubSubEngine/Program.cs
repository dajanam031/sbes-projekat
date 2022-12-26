﻿using Contracts;
using Publisher;
using Subscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SecurityManager;

namespace PubSubEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            string addressPub = "net.tcp://localhost:5353/PubService";
            ServiceHost pubHost = SreviceHostMethod.HostMethod(addressPub, typeof(PubService), typeof(IPublish), srvCertCN);

            string addressSub = "net.tcp://localhost:5353/SubService";
            ServiceHost subHost = SreviceHostMethod.HostMethod(addressSub, typeof(SubService), typeof(ISubscribe), srvCertCN);
            
            try
            {
                pubHost.Open();
                Console.WriteLine("Publisher host has started.");
                subHost.Open();
                Console.WriteLine("Subscriber host has started.");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }
            finally
            {
                pubHost.Close();
                subHost.Close();
            }
        }
    }
}
