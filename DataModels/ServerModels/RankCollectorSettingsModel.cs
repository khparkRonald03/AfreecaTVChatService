using System;

namespace DataModels
{
    [Serializable]
    public class RankCollectorSettingsModel
    {
        public int Idx { get; set; }

        public int? LastHistoryDepth { get; set; }

        public string ActionWeek { get; set; }

        public string ActionTime { get; set; } // 11:00

        public int EndPageNum { get; set; }

        public string ClientVersion { get; set; }

        public string AbjIDSplitString { get; set; }

        public DateTime? LastActionDate { get; set; }
    }
}
