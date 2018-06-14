using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using avj.BizDac;
using DataModels;
using RankCollector;

namespace RankCollectorFront
{
    public class Program
    {
        static void Main(string[] args)
        {
            //// test
            //var bjPagePaser = new BjPagePaser();
            //RankBjModel test = new RankBjModel()
            //{
            //    BjID = "lolbjmatch"
            //};
            //bjPagePaser.GetBjInfo(test);
            //return;
            //test
            GoRankCollector(20);

            //Thread.Sleep(5000 * 60 * 60 * 60);
            return;

            var autoEvent = new AutoResetEvent(false);

            var GoRankTimer = new Timer(
                CheckActionTime, 
                autoEvent, 
                TimeSpan.FromHours(1).Milliseconds, 
                0 //TimeSpan.FromHours(1).Milliseconds
            );

            autoEvent.WaitOne();
        }

        private static void CheckActionTime(object state)
        {
            var biz = new BizRankCollectorSettings();
            var setting = biz.GetSettings();

            var actionWeek = setting.ActionWeek ?? string.Empty;
            var actionTime = setting.ActionTime;
            if (string.IsNullOrEmpty(actionTime))
                return;

            var Weeks = actionWeek.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (Weeks == null || Weeks.Count() <= 0)
                return;

            var nowWeek = DateTime.Now.ToString("ddd");
            int fromHour = DateTime.Now.Hour;
            int toHour = DateTime.Now.AddHours(1).Hour;
            int actionHour = int.Parse(actionTime);

            
            foreach (var week in Weeks)
            {
                if (week == nowWeek)
                {
                    if (fromHour < actionHour && toHour > actionHour)
                        GoRankCollector(setting.EndPageNum);
                    break;
                }
            }
        }

       
        private static void GoRankCollector(int pEndPage)
        {
            // 1. BJ 수집
            int endPage = pEndPage;
            var categoryRankParser = new CategoryRankParser();
            var resultBjModels = new List<RankBjModel>();

            foreach (RankingType rankingType in Enum.GetValues(typeof(RankingType)))
            {
                for (int i = 1; i <= endPage; i++)
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
            var resultBjInfo = new List<BjInfoModel>();

            // BJ 수집 리스트 루프 돌기
            foreach (var BjModel in resultBjModels)
            {
                // BJ 정보
                var BjInfo = bjPagePaser.GetBjInfo(BjModel);
                resultBjInfo.AddRange(BjInfo);

                // 열혈팬 Top20
                var bigFans = bjPagePaser.GetBigFans(BjModel.BjID);
                resultUserModel.AddRange(bigFans);

                // 서포터 Top20
                var supporters = bjPagePaser.GetSupporters(BjModel.BjID);
                resultUserModel.AddRange(supporters);
            }

            ;

            var bizSettings = new BizRankCollectorSettings();
            var Settings = bizSettings.GetSettings();

            var bizBjRank = new BizBjRank(Settings);
            bizBjRank.SetInitBjValues();
            bizBjRank.SetBjModels(resultBjModels);

            foreach (var userModel in resultUserModel)
            {
                var bjModel = resultBjModels.Find(b => b.BjID == userModel.BjID);
                userModel.BjNic = bjModel.BjNick;
            }

            var bizUserRank = new BizUserRank(Settings);
            bizUserRank.SetInitUserValues();
            bizUserRank.SetUserModels(resultUserModel);
            bizUserRank.SetInitBjInfoValues();
            bizUserRank.SetBjInfoModels(resultBjInfo);

            bizSettings.SetAferSettings();
            ;
        }
    }
}
