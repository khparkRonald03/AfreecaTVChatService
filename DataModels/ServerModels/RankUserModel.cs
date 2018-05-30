using System;

namespace DataModels
{
    [Serializable]
    public class RankUserModel
    {
        /// <summary>
        /// Bj 아이디
        /// </summary>
        public string BjID { get; set; }

        /// <summary>
        /// BJ 닉네임
        /// </summary>
        public string BjNic { get; set; }

        /// <summary>
        /// 사용자 아이디 (사용자별 중복가능)
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 사용자 닉네임
        /// </summary>
        public string UserNick { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? HistoryDepth { get; set; }

        /// <summary>
        /// 빅팬 순위
        /// </summary>
        public int BigFanRanking { get; set; }

        /// <summary>
        /// 서포터 순위
        /// </summary>
        public int SupportRanking { get; set; }

        /// <summary>
        /// 값 유효성 여부
        /// </summary>
        public string Valid { get; set; }

        /// <summary>
        /// 추가된 날짜
        /// </summary>
        public DateTime? AddDate { get; set; }
    }
}
