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
        /// BJ 빅팬 랭킹
        /// </summary>
        [DataMember(Name = "Ranking")]
        public int Ranking { get; set; }

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

        /// <summary>
        /// 클라이언트 버전
        /// </summary>
        [DataMember(Name = "ClientVersion")]
        public string ClientVersion { get; set; }

        /// <summary>
        /// 클라이언트 버전 업로드 여부
        /// </summary>
        [DataMember(Name = "IsNewUpload")]
        public bool IsNewUpload { get; set; }

        /// <summary>
        /// 클라이언트 버전 업로드 여부
        /// </summary>
        [DataMember(Name = "VersionMessage")]
        public string VersionMessage { get; set; }

        /// <summary>
        /// 클라이언트 레벨
        /// </summary>
        [DataMember(Name = "ClientLevel")]
        public int ClientLevel { get; set; }

        /// <summary>
        /// 로그인 아이디
        /// </summary>
        [DataMember(Name = "LoginID")]
        public string LoginID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "ChatID")]
        public string ChatID { get; set; }

        /// <summary>
        /// 로그인 아이디
        /// </summary>
        [DataMember(Name = "LoginID2")]
        public string LoginID2 { get; set; }

        /// <summary>
        /// 만료 여부
        /// </summary>
        [DataMember(Name = "ExpireFlag")]
        public bool ExpireFlag { get; set; }

        /// <summary>
        /// 인증 여부
        /// </summary>
        [DataMember(Name = "FaildFalg")]
        public bool CertificationFlag { get; set; }

        /// <summary>
        /// 인증 메시지
        /// </summary>
        [DataMember(Name = "CertificationMessage")]
        public string CertificationMessage { get; set; }

        /// <summary>
        /// 만료 여부
        /// </summary>
        [DataMember(Name = "ExpireMessage")]
        public string ExpireMessage { get; set; }

        /// <summary>
        /// 만료일
        /// </summary>
        [DataMember(Name = "ExpireDate")]
        public DateTime? ExpireDate { get; set; }
    }
}
