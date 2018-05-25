using System;

namespace DataModels
{
    [Serializable]
    public class ChatModel
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
        /// 수집 된 채팅 html
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// 새로추가된 채팅인지 여부
        /// </summary>
        public bool IsNew { get; set; }
    }
}
