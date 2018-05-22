using System.Collections.Generic;

namespace RankCollector
{
    public class Supporters
    {
        public CHANNEL CHANNEL { get; set; }
    }

    public class List
    {
        public string Bj_id { get; set; }
        public string Sticker_top { get; set; }
        public string Supporter_cnt { get; set; }
    }

    public class SUPPORTERLIST
    {
        public List<List> List { get; set; }
    }

    public class CHANNEL
    {
        public int RESULT { get; set; }
        public List<SUPPORTERLIST> SUPPORTER_LIST { get; set; }
    }
}
