using System.Runtime.Serialization;

namespace Common.Communication
{
    [DataContract]
    public class PublicKey
    {
        [DataMember]
        public byte[] Exponent { get; set; }

        [DataMember]
        public byte[] Modulus { get; set; }
    }
}
