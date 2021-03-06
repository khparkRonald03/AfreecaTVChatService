using avj.Common;
using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avj.BizDac
{
    public class DacRankCollectorSettings : MySql
    {
        public RankCollectorSettingsModel GetSettings(string sqlQuery)
        {
            return GetDataModel<RankCollectorSettingsModel>(sqlQuery);
        }

        public void SetLastActionDateSettings(string query)
        {
            ExecuteNonQuery(query);
        }
    }
}
