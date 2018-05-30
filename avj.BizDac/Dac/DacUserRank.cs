using avj.Common;
using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avj.BizDac
{
    public class DacUserRank : MySql
    {
        public void SetInitUserValues(string query)
        {
            ExecuteNonQuery(query);
        }

        public void SetUserModel(string query,  RankUserModel userModel)
        {
            var mysqlParams = new List<MysqlParam>
            {
                // 동작 구분
                //new MysqlParam("@WorkingTag", MysqlDbType.VarChar, "AP"),

                // Bj 아이디
                new MysqlParam("@BjID", MysqlDbType.VarChar, userModel.BjID),

                // Bj 닉네임
                new MysqlParam("@BjNic", MysqlDbType.VarChar, userModel.BjNic),

                // 사용자 아이디 (사용자별 중복가능)
                new MysqlParam("@UserID", MysqlDbType.VarChar, userModel.UserID),

                // 사용자 닉네임
                new MysqlParam("@UserNick", MysqlDbType.VarChar, userModel.UserNick),

                new MysqlParam("@HistoryDepth", MysqlDbType.Int16, userModel.HistoryDepth),

                // 빅팬 순위
                new MysqlParam("@BigFanRanking", MysqlDbType.Int16, userModel.BigFanRanking),

                // 서포터 순위
                new MysqlParam("@SupportRanking", MysqlDbType.Int16, userModel.SupportRanking),

                // 값 유효성 여부 -> 기본값 'N'
                //new MysqlParam("@Valid", MysqlDbType.VarChar, userModel.Valid),

                // 추가된 날짜
                //new MysqlParam("@AddDate", MysqlDbType.Datetime, userModel.AddDate),
            };

            ExecuteNonQuery(query, mysqlParams, System.Data.CommandType.Text);
        }

        public List<RankUserModel> GetAllRankUserModels(string query)
        {
            var result = GetDataModel<List<RankUserModel>>(query);
            return result;
        }
    }
}
