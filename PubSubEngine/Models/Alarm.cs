using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [DataContract]
    public enum AlarmMessages
    {
        [EnumMember]
        Low,
        [EnumMember]
        Standard,
        [EnumMember]
        High

    }
    [DataContract]
    [Serializable]
    public class Alarm
    {
        [DataMember]
        private DateTime GeneratingTime;
        [DataMember]
        private string MessegAlarm;
        [DataMember]
        private int risk;

        public int Risk
        {
            get
            {
                return risk;
            }
            set
            {
                if(value <= 100 && value>=1)
                    risk = value;
            }
        }
    }
}
