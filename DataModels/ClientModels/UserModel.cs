using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DataModels
{
    [Serializable]
    [DataContract]
    public class UserModel
    {
        /// <summary>
        /// 유저 유형
        /// </summary>
        [DataMember(Name = "Type")]
        public UserType Type { get; set; }

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
        /// 포함된 bj정보
        /// </summary>
        [DataMember(Name = "BJs")]
        public List<BjModel> BJs { get; set; }

        /// <summary>
        /// 사진url (BJ일경우)
        /// </summary>
        [DataMember(Name = "PictureUrl")]
        public string PictureUrl { get; set; }

        /// <summary>
        /// 아이콘 Url (하위 정보로 쓰일 때 사용) (BJ일경우?)
        /// </summary>
        [DataMember(Name = "IconUrl")]
        public string IconUrl { get; set; }

        /// <summary>
        /// 새로 추가된 요소 여부 (전부다 리프레쉬 해주니까 필요없음)
        /// </summary>
        [DataMember(Name = "IsNew")]
        public bool IsNew { get; set; }

        /// <summary>
        /// 화면에 표시할 html (재사용을 위해 캐시)
        /// </summary>
        [DataMember(Name = "Html")]
        public string Html { get; set; }

        /// <summary>
        /// BJ랭킹 정보 (BJ일 경우)
        /// </summary>
        [DataMember(Name = "RankingInfo")]
        public RankBjModel RankingInfo { get; set; }

        /// <summary>
        /// BJ정보 (BJ일 경우)
        /// </summary>
        [DataMember(Name = "BjInfo")]
        public List<BjInfoModel> BjInfo { get; set; }
    }

    public enum UserType
    {
        BJ,
        King,
        BigFan,
    }
}
