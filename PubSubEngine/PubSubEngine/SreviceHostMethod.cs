using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PubSubEngine
{
    public static class SreviceHostMethod
    {
        public static ServiceHost HostMethod(string address,Type serviceType,Type contractType)
        {
            NetTcpBinding binding = new NetTcpBinding();
            
            ServiceHost host = new ServiceHost(serviceType);
            host.AddServiceEndpoint(contractType, binding, address);

            return host;
        }
    }
}
