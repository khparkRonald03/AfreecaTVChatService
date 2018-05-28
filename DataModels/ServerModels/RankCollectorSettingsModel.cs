using System;

namespace DataModels
{
    [Serializable]
    public class RankCollectorSettingsModel
    {
        public int Idx { get; set; }

        public int? LastHistoryDepth { get; set; }

        public string ActionWeek { get; set; }

        public DateTime? ActionTime { get; set; }
    }
}
