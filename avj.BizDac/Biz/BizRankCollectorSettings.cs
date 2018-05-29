using DataModels;

namespace avj.BizDac
{
    public class BizRankCollectorSettings
    {
        DacRankCollectorSettings Dac { get; set; }

        public BizRankCollectorSettings()
        {
            Dac = new DacRankCollectorSettings();
        }
            
        #region 랭킹 수집

        public RankCollectorSettingsModel GetSettings()
        {
            var sqlQuery = RankCollectorSettingsQuery.SelectSettings;
            
            var result = Dac.GetSettings(sqlQuery);
            return result;
        }

        #endregion

        #region 랭킹 매칭

        public void SetAferSettings()
        {
            var sqlQuery = RankCollectorSettingsQuery.UpdateLastActionDate;

            Dac.SetLastActionDateSettings(sqlQuery);
        }

        #endregion
    }
}
