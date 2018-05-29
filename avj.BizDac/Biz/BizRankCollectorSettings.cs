using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //sqlQuery 설정
            var sqlQuery = RankCollectorSettingsQuery.SelectSettings;
            
            var result = Dac.GetSettings(sqlQuery);
            return result;
        }

        #endregion
    }
}
