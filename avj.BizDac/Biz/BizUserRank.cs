using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avj.BizDac
{
    public class BizUserRank
    {
        readonly RankCollectorSettingsModel rankCollectorSettingsModel;

        public BizUserRank()
        {

        }

        public BizUserRank(RankCollectorSettingsModel settingsModel)
        {
            rankCollectorSettingsModel = settingsModel;
        }

        public void SetInitUserValues()
        {
            string query = RankUserQuery.UpdateInitUserValues;

            var dac = new DacUserRank();
            dac.SetInitUserValues(query);
        }

        public void SetUserModels(List<RankUserModel> userModels)
        {
            var insertAbjUserRank = RankUserQuery.InsertAbjUserRank;

            var dac = new DacUserRank();

            foreach (var userModel in userModels)
            {
                userModel.HistoryDepth = rankCollectorSettingsModel.LastHistoryDepth;
                dac.SetUserModel(insertAbjUserRank, userModel);
            }
                
        }

    }
}
