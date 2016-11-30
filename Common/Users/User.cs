using System;
using System.Text;
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

        public User Clone()
        {
            return MemberwiseClone() as User;
        }
    }
}
