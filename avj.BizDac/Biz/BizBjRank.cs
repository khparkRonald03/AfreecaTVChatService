using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avj.BizDac
{
    public class BizBjRank
    {
        readonly RankCollectorSettingsModel rankCollectorSettingsModel;

        public BizBjRank()
        {
        }

        public BizBjRank(RankCollectorSettingsModel RankCollectorSettingsModel)
        {
            rankCollectorSettingsModel = RankCollectorSettingsModel;
        }

        public void SetInitBjValues()
        {
            string query = BjRankQuery.UpdateInitBjValues;
            var dac = new DacBjRank();
            dac.SetInitBjValues(query);
        }

        public void SetBjModels(List<RankBjModel> bjModels)
        {
            var query = BjRankQuery.InsertAbjBjRank;

            var dac = new DacBjRank();

            foreach (var bjModel in bjModels)
            {
                bjModel.HistoryDepth = rankCollectorSettingsModel.LastHistoryDepth;
                dac.SetBjModel(bjModel, query);
            }
                
        }
    }
}
