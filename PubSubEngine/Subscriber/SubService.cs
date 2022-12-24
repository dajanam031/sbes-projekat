using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    public class SubService : ISubscribe
    {
        public void Send(string topic)
        {
            Console.WriteLine("Tema : " + topic);
        }
    }
}
