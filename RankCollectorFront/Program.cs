using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels;
using RankCollector;

namespace RankCollectorFront
{
    public class Program
    {
        RankCollectorSettingsModel Settings = new RankCollectorSettingsModel();

        static void Main(string[] args)
        {
            // 1. BJ 수집
            int endPage = 2;
            var categoryRankParser = new CategoryRankParser();
            var resultBjModels = new List<RankBjModel>();
            
            foreach (RankingType rankingType in Enum.GetValues(typeof(RankingType)))
            {
                for (int i =1; i <= endPage; i++)
                {
                    var bjModels = categoryRankParser.GetData(rankingType, i);

                    if (resultBjModels.Count <= 0)
                    {
                        resultBjModels.AddRange(bjModels);
                        continue;
                    }

                    foreach (var bj in bjModels)
                    {
                        //기존 BJ 없다면 새로 추가
                        if (!resultBjModels.Any(b => b.BjID == bj.BjID))
                        {
                            resultBjModels.Add(bj);
                            continue;
                        }

                        // 기존 BJ 있으면 기존 BJ에 담고 해당 랭크만 채우기
                        foreach (var resultBjModel in resultBjModels)
                        {
                            if (resultBjModel.BjID != bj.BjID)
                                continue;

                            categoryRankParser.SetTargetRank(bj, resultBjModel, rankingType);
                        }
                    }

                }

            }

            // 2. 사용자 수집
            var bjPagePaser = new BjPagePaser();
            var resultUserModel = new List<RankUserModel>();

            // BJ 수집 리스트 루프 돌기
            foreach (var BjModel in resultBjModels)
            {
                // 열혈팬 Top20
                var bigFans = bjPagePaser.GetBigFans(BjModel.BjID);
                resultUserModel.AddRange(bigFans);

                // 서포터 Top20
                var supporters = bjPagePaser.GetSupporters(BjModel.BjID);
                resultUserModel.AddRange(supporters);
            }

            ;

            // resultBjModels
            // resultUserModel
        }
    }
}
