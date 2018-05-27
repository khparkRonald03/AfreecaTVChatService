using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankCollector
{
    public class CategoryRankParser : CommonWeb
    {
        public List<BjModel> GetData(RankingType rankingType, int page)
        {
            var result = new List<BjModel>();

            var categoryRank = GetCategoryRank(rankingType, page);

            foreach (var ranks in categoryRank.ALL_RANK)
            {
                var item = GetBjModel(ranks, rankingType);
                result.Add(item);
            }

            return result;
        }

        public void SetTargetRank(BjModel fromModel, BjModel toModel, RankingType rankingType)
        {
            int rank = GetRank(fromModel, rankingType);
            SetRank(ref toModel, rank, rankingType);
        }

        private BjModel GetBjModel(ALLRANK allrank, RankingType rankingType)
        {
            if (string.IsNullOrEmpty(allrank.Bj_id))
                return new BjModel();

            var bjModel = new BjModel()
            {
                BjID = allrank.Bj_id,
                BjNick = allrank.Bj_nick,
                BjImgUrl = $"http://stimg.afreecatv.com/LOGO/{allrank.Bj_id.Substring(0, 2)}/{allrank.Bj_id}/{allrank.Bj_id}.jpg"
            };

            if (!int.TryParse(allrank.Total_rank, out int rank))
                return null;

            SetRank(ref bjModel, rank, rankingType);

            return bjModel;
        }

        private void SetRank(ref BjModel bjModel, int rank, RankingType rankingType)
        {
            switch (rankingType)
            {
                case RankingType.rookie:
                    bjModel.RookieRanking = rank;
                    break;
                case RankingType.viewer:
                    bjModel.ViewerRanking = rank;
                    break;
                case RankingType.favoriteup:
                    bjModel.FavoriteupRanking = rank;
                    break;
                case RankingType.up:
                    bjModel.UpRanking = rank;
                    break;
                case RankingType.mobile:
                    bjModel.MobileRanking = rank;
                    break;
                case RankingType.game:
                    bjModel.GameRanking = rank;
                    break;
                case RankingType.mobilegame:
                    bjModel.MobilegameRanking = rank;
                    break;
                case RankingType.sports_broadcast:
                    bjModel.SportsBroadcastRanking = rank;
                    break;
                case RankingType.sports_general:
                    bjModel.SportsGeneralRanking = rank;
                    break;
                case RankingType.talkcam:
                    bjModel.TalkcamRanking = rank;
                    break;
                case RankingType.mukbang:
                    bjModel.MukbangRanking = rank;
                    break;
                case RankingType.music:
                    bjModel.MusicRanking = rank;
                    break;
                case RankingType.pet:
                    bjModel.PetRanking = rank;
                    break;
                case RankingType.hobby:
                    bjModel.HobbyRanking = rank;
                    break;
                case RankingType.study:
                    bjModel.StudyRanking = rank;
                    break;
                case RankingType.dubradio:
                    bjModel.DubradioRanking = rank;
                    break;
                case RankingType.stock:
                    bjModel.StockRanking = rank;
                    break;
                case RankingType.enter:
                    bjModel.EnterRanking = rank;
                    break;
                case RankingType.videos:
                    bjModel.VideosRanking = rank;
                    break;
                case RankingType.favorite:
                    bjModel.FavoriteRanking = rank;
                    break;
                case RankingType.fanclub:
                    bjModel.FanclubRanking = rank;
                    break;
            }
        }

        private int GetRank(BjModel bjModel, RankingType rankingType)
        {
            switch (rankingType)
            {
                case RankingType.rookie:
                    return bjModel.RookieRanking;
                case RankingType.viewer:
                    return bjModel.ViewerRanking;
                case RankingType.favoriteup:
                    return bjModel.FavoriteupRanking;
                case RankingType.up:
                    return bjModel.UpRanking;
                case RankingType.mobile:
                    return bjModel.MobileRanking;
                case RankingType.game:
                    return bjModel.GameRanking;
                case RankingType.mobilegame:
                    return bjModel.MobilegameRanking;
                case RankingType.sports_broadcast:
                    return bjModel.SportsBroadcastRanking;
                case RankingType.sports_general:
                    return bjModel.SportsGeneralRanking;
                case RankingType.talkcam:
                    return bjModel.TalkcamRanking;
                case RankingType.mukbang:
                    return bjModel.MukbangRanking;
                case RankingType.music:
                    return bjModel.MusicRanking;
                case RankingType.pet:
                    return bjModel.PetRanking;
                case RankingType.hobby:
                    return bjModel.HobbyRanking;
                case RankingType.study:
                    return bjModel.StudyRanking;
                case RankingType.dubradio:
                    return bjModel.DubradioRanking;
                case RankingType.stock:
                    return bjModel.StockRanking;
                case RankingType.enter:
                    return bjModel.EnterRanking;
                case RankingType.videos:
                    return bjModel.VideosRanking;
                case RankingType.favorite:
                    return bjModel.FavoriteRanking;
                case RankingType.fanclub:
                    return bjModel.FanclubRanking;
                default:
                    return 0;
            }
        }
    }
}
