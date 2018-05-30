using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DataModels
{
    [Serializable]
    [DataContract]
    public class JsonModel
    {
        [JsonProperty("BjModel")]
        //[DataMember(Name = "BjModel")]
        public BjModel BjModel { get; set; }

        [JsonProperty("UserModels")]
        //[DataMember(Name = "UserModels")]
        public List<UserModel> UserModels { get; set; }
    }
}
