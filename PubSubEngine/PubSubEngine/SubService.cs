using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubEngine
{
    public class SubService : ISubscribe
    {
        public List<Alarm> ForwardAlarm(int min, int max)
        {
            List<Alarm> alarms = new List<Alarm>();
            foreach (Alarm a in AlarmStorage.alarms)
            {
                if (a.Risk > min && a.Risk < max)
                {
                    alarms.Add(a);
                }
            }
            return alarms;
        }

        public void Send(string topic)
        {
            Console.WriteLine("Alarm rizika : " + topic);
        }
    }
}
