using System;
using System.Text;
using System.Runtime.Serialization;

namespace Common.Users
{
    [DataContract]
    public class Identity
    {
        [DataMember]
        public string ANumber { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Alias { get; set; }

        public Identity Clone()
        {
            return MemberwiseClone() as Identity;
        }
    }
}
