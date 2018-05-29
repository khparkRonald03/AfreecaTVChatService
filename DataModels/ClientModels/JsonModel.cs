using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DataModels
{
    [Serializable]
    [DataContract]
    public class JsonModel
    {
        [DataMember(Name = "BjModel")]
        public BjModel BjModel { get; set; }

        [DataMember(Name = "UserModels")]
        public List<UserModel> UserModels { get; set; }
    }
}
