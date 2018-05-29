using System;
using System.Runtime.Serialization;

namespace DataModels
{
    [Serializable]
    [DataContract]
    public class BjModel
    {
        /// <summary>
        /// 아이디 
        /// </summary>
        [DataMember(Name = "ID")]
        public string ID { get; set; }

        /// <summary>
        /// 닉네임
        /// </summary>
        [DataMember(Name = "Nic")]
        public string Nic { get; set; }

        /// <summary>
        /// 사진url
        /// </summary>
        [DataMember(Name = "PictureUrl")]
        public string PictureUrl { get; set; }

        /// <summary>
        /// 아이콘 Url (하위 정보로 쓰일 때 사용)
        /// </summary>
        [DataMember(Name = "IconUrl")]
        public string IconUrl { get; set; }
    }
}
