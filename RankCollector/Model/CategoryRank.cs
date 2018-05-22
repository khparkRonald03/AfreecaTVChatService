using System.Collections.Generic;

namespace RankCollector
{
    /// <summary>
    /// Jsoon에서 변환 된
    /// </summary>
    public class CategoryRank
    {
        public int RESULT { get; set; }
        public List<ALLRANK> ALL_RANK { get; set; }
        public string TOTAL_CNT { get; set; }
    }

    public class ALLRANK
    {
        public bool Is_broad { get; set; }
        public string Bj_id { get; set; }
        public string Bj_nick { get; set; }
        public string Station_title { get; set; }
        public string Total_rank { get; set; }
        public string Total_rank_last { get; set; }
        public bool Is_favorite { get; set; }
    }
}
