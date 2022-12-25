using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    internal class ClientProxy : ChannelFactory<IPublish>, IPublish, IDisposable
    {
        IPublish factory;

        public ClientProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void Send(Alarm alarm)
        {
            factory.Send(alarm);
        }
    }
}
