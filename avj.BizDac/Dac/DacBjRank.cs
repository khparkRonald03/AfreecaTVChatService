using avj.Common;
using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avj.BizDac
{
    public class DacBjRank : MySql
    {
        public void SetInitBjValues(string query)
        {
            ExecuteNonQuery(query);
        }

        public void SetBjModel(RankBjModel bjModel, string query)
        {
            var mysqlParams = new List<MysqlParam>
            {
                // 동작 구분
                //new MysqlParam("@p_WorkingTag", MysqlDbType.VarChar, "AP"),

                // BJ 아이디
                new MysqlParam("@BjID", MysqlDbType.VarChar, bjModel.BjID),

                // BJ 닉네임
                new MysqlParam("@BjNick", MysqlDbType.VarChar, bjModel.BjNick),

                // BJ 사진 url
                new MysqlParam("@BjImgUrl", MysqlDbType.VarChar, bjModel.BjImgUrl),

                // 
                new MysqlParam("@HistoryDepth", MysqlDbType.Int16, bjModel.HistoryDepth),

                // 아프리카 신입bj 랭킹
                new MysqlParam("@RookieRanking", MysqlDbType.Int16, bjModel.RookieRanking),

                // 최고시청자 평균
                new MysqlParam("@ViewerRanking", MysqlDbType.Int16, bjModel.ViewerRanking),

                // 애청자 증가수
                new MysqlParam("@FavoriteupRanking", MysqlDbType.Int16, bjModel.FavoriteupRanking),

                // UP 수
                new MysqlParam("@UpRanking", MysqlDbType.Int16, bjModel.UpRanking),

                // 모바일
                new MysqlParam("@MobileRanking", MysqlDbType.Int16, bjModel.MobileRanking),

                // 게임
                new MysqlParam("@GameRanking", MysqlDbType.Int16, bjModel.GameRanking),

                // 모바일게임
                new MysqlParam("@MobilegameRanking", MysqlDbType.Int16, bjModel.MobilegameRanking),

                // 스포츠중계
                new MysqlParam("@SportsBroadcastRanking", MysqlDbType.Int16, bjModel.SportsBroadcastRanking),

                // 스포츠일반
                new MysqlParam("@SportsGeneralRanking", MysqlDbType.Int16, bjModel.SportsGeneralRanking),

                // 토크/캠방
                new MysqlParam("@TalkcamRanking", MysqlDbType.Int16, bjModel.TalkcamRanking),

                // 먹방/쿡방
                new MysqlParam("@MukbangRanking", MysqlDbType.Int16, bjModel.MukbangRanking),

                // 음악
                new MysqlParam("@MusicRanking", MysqlDbType.Int16, bjModel.MusicRanking),

                // 펫방
                new MysqlParam("@PetRanking", MysqlDbType.Int16, bjModel.PetRanking),

                // 취미
                new MysqlParam("@HobbyRanking", MysqlDbType.Int16, bjModel.HobbyRanking),

                // 학습
                new MysqlParam("@StudyRanking", MysqlDbType.Int16, bjModel.StudyRanking),

                // 더빙/라디오
                new MysqlParam("@DubradioRanking", MysqlDbType.Int16, bjModel.DubradioRanking),

                // 주식/금융
                new MysqlParam("@StockRanking", MysqlDbType.Int16, bjModel.StockRanking),

                // 엔터테인먼트
                new MysqlParam("@EnterRanking", MysqlDbType.Int16, bjModel.EnterRanking),

                // 동영상
                new MysqlParam("@VideosRanking", MysqlDbType.Int16, bjModel.VideosRanking),

                // 누적 애청자 수
                new MysqlParam("@FavoriteRanking", MysqlDbType.Int16, bjModel.FavoriteRanking),

                // 누적 팬클럽 수
                new MysqlParam("@FanclubRanking", MysqlDbType.Int16, bjModel.FanclubRanking),

                // 값 유효성 여부
                //new MysqlParam("@iId", MysqlDbType.VarChar, bjModel.Valid),

                // 추가된 날짜
                //new MysqlParam("@AddDate", MysqlDbType.Datetime, bjModel.AddDate)

            };

            ExecuteNonQuery(query, mysqlParams, System.Data.CommandType.Text);
        }

        public List<RankBjModel> GetAllRankBjModels(string query)
        {
            var result = GetDataModel<List<RankBjModel>>(query);
            return result;
        }

        public List<BjInfoModel> GetAllBjInfoModels(string query)
        {
            var result = GetDataModel<List<BjInfoModel>>(query);
            return result;
        }

        public List<FirstCharModel> GetFirstCharListByRankBjModels(string query)
        {
            var result = GetDataModel<List<FirstCharModel>>(query);
            return result;
        }

    }
}
