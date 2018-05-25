using System;

namespace DataModels
{
    [Serializable]
    public class BjModel
    {
        /// <summary>
        /// 아이디 
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 닉네임
        /// </summary>
        public string Nic { get; set; }

        /// <summary>
        /// 사진url
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// 아이콘 Url (하위 정보로 쓰일 때 사용)
        /// </summary>
        public string IconUrl { get; set; }
    }
}
