using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.Users
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Alias { get; set; }
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public List<int> ContractList { get; set; }
        [DataMember]
        public List<int> PhaseList { get; set; }
        [DataMember]
        public List<int> TaskList { get; set; }

        public User Clone()
        {
            return MemberwiseClone() as User;
        }
    }
}
