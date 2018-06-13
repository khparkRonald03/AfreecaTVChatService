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

        DacUserRank Dac { get; set; }

        public BizUserRank()
        {
            Dac = new DacUserRank();
        }

        #region 랭킹 수집

        public BizUserRank(RankCollectorSettingsModel settingsModel)
            : this()
        {
            rankCollectorSettingsModel = settingsModel;
        }

        public void SetInitUserValues()
        {
            string query = RankUserQuery.UpdateInitUserValues;

            Dac.SetInitUserValues(query);
        }

        public void SetUserModels(List<RankUserModel> userModels)
        {
            var insertAbjUserRank = RankUserQuery.InsertAbjUserRank;

            foreach (var userModel in userModels)
            {
                userModel.HistoryDepth = rankCollectorSettingsModel.LastHistoryDepth;
                Dac.SetUserModel(insertAbjUserRank, userModel);
            }
                
        }

        public void SetInitBjInfoValues()
        {
            string query = BjInfoQuery.UpdateInitBjInfoValues;

            Dac.SetInitUserValues(query);
        }

        public void SetBjInfoModels(List<BjInfoModel> bjinfoModels)
        {
            var insertAbjBjInfo = BjInfoQuery.InsertAbjBjInfo;

            foreach (var bjinfoModel in bjinfoModels)
            {
                bjinfoModel.HistoryDepth = rankCollectorSettingsModel.LastHistoryDepth;
                Dac.SetBjInfoModel(insertAbjBjInfo, bjinfoModel);
            }
        }

        #endregion

        public List<RankUserModel> GetAllRankUserModels()
        {
            string query = RankUserQuery.SelectAllValidYUserRank;

            var result = Dac.GetAllRankUserModels(query);
            return result;
        }
    }
}
