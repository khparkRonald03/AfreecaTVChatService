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

        DacBjRank Dac { get; set; }

        public BizBjRank()
        {
            Dac = new DacBjRank();
        }

        public BizBjRank(RankCollectorSettingsModel RankCollectorSettingsModel)
            : this()
        {
            rankCollectorSettingsModel = RankCollectorSettingsModel;
        }

        #region 랭킹 수집

        public void SetInitBjValues()
        {
            string query = BjRankQuery.UpdateInitBjValues;
            Dac.SetInitBjValues(query);
        }

        public void SetBjModels(List<RankBjModel> bjModels)
        {
            var query = BjRankQuery.InsertAbjBjRank;

            foreach (var bjModel in bjModels)
            {
                bjModel.HistoryDepth = rankCollectorSettingsModel.LastHistoryDepth;
                Dac.SetBjModel(bjModel, query);
            }
                
        }

        #endregion

        public List<RankBjModel> GetAllRankBjModels()
        {
            string query1 = BjRankQuery.SelectAllValidYBjRank;
            var result = Dac.GetAllRankBjModels(query1);

            string query2 = BjRankQuery.SelectAllValidYBjInfo;
            var bjInfoModels = Dac.GetAllBjInfoModels(query2);
            foreach (var rankBjModel in result)
            {
                var findBjInfo = bjInfoModels.FindAll(b => b.BjID == rankBjModel.BjID);
                if (findBjInfo != null)
                    rankBjModel.Bjinfo = findBjInfo;
            }

            return result;
        }
    }
}
