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
            try
            {
                //목록데이터반환
                return GetDataModel<RankCollectorSettingsModel>(sqlQuery);
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return new RankCollectorSettingsModel();
            }
        }
    }
}
