using System.Collections.Generic;

namespace RankCollector
{
    public class BigFan
    {
        public int Result { get; set; }
        public string Bj_id { get; set; }
        public string Fanclub_cnt { get; set; }
        public List<StarballoonTop> Starballoon_top { get; set; }
    }

    public class StarballoonTop
    {
        public string User_id { get; set; }
        public string User_nick { get; set; }
    }
}
