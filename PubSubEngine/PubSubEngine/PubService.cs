using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubEngine
{
    public class PubService : IPublish
    {
        public void Send(string topic, string text)
        {
            Console.WriteLine("Tema : " + topic);
            Console.WriteLine("Text : " + text);
        }

        public void Send(Alarm alarm)
        {
            AlarmStorage.alarms.Add(alarm);
        }
    }
}
