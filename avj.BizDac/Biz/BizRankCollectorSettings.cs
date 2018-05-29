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
        public RankCollectorSettingsModel GetSettings()
        {
            //sqlQuery 설정
            var sqlQuery = RankCollectorSettingsQuery.SelectSettings;

            var dac = new DacRankCollectorSettings();
            var result = dac.GetSettings(sqlQuery);
            return result;
        }
    }
}
