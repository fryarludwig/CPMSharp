using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common.WorkItems
{
    [DataContract]
    public class Material
    {
        public Material(string name, string description, string units)
        {
            Name = name;
            Description = description;
            Units = units;
        }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Units { get; set; }
        
        [DataMember]
        public double UnitsReserved { get; set; }
        [DataMember]
        public double UnitsUsed { get; set; }

        public double ReservedUnitsRemaining => UnitsReserved - UnitsUsed;
    }
}
