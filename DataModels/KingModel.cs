using System.Collections.Generic;

namespace DataModels
{
    public class KingModel
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
        /// 포함된 bj정보
        /// </summary>
        public List<BjModel> BJs { get; set; }
    }
}
