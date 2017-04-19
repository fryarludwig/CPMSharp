using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Common.WorkItems
{
    [DataContract]
    public class Contract : WorkItem
    {
        public Contract() : base()
        {
            Phases = new Dictionary<int, Phase>();
        }

        public Dictionary<int, Phase> Phases { get; set; }
        public double DollarValue { get; set; }
    }
}
