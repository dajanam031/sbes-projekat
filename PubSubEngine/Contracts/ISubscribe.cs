using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface ISubscribe
    {
        [OperationContract]
        void Send(string topic);
        [OperationContract]
        List<Alarm> ForwardAlarm(int min, int max);
    }
}
