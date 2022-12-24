using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    internal class ClientProxy : ChannelFactory<ISubscribe>, ISubscribe, IDisposable
    {
        ISubscribe factory;

        public ClientProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void Send(string topic)
        {
            factory.Send(topic);
        }
    }
}
