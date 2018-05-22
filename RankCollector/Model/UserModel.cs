namespace RankCollector
{
    public class UserModel
    {
        /// <summary>
        /// Bj 아이디
        /// </summary>
        public string BjID { get; set; }

        /// <summary>
        /// 사용자 아이디 (사용자별 중복가능)
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 사용자 닉네임
        /// </summary>
        public string UserNick { get; set; }

        /// <summary>
        /// 빅팬 순위
        /// </summary>
        public int BigFanRanking { get; set; }

        /// <summary>
        /// 서포터 순위
        /// </summary>
        public int SupportRanking { get; set; }
    }
}
