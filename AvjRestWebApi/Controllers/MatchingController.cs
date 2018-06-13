using AvjRestWebApi.DataCache;
using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AvjRestWebApi.Controllers
{
    //[RoutePrefix("api/users")]
    public class MatchingController : ApiController
    {
        [HttpPost]
        [Route("Matching/UsersMatching")]
        public JsonModel UsersMatching (JsonModel jsonModel)
        {
            var bj = jsonModel.BjModel;
            var users = jsonModel.UserModels;

            if (bj == null || string.IsNullOrEmpty(bj.ID))
                return null;

            if (users == null || users.Count <= 0)
                return null;

            // bj 확인 -> 테스트때는 
            //var G5Member = G5_memberDataCache.Instance.GetG5MemberModels;

            //if (!G5Member.Any(g => g.mb_id == bj.ID))
            //    return null;


            var result = new JsonModel
            {
                UserModels = new List<UserModel>()
            };


            Parallel.For(0, 2, (i) =>
            {
                switch (i)
                {
                    case 0:
                        var serverBjs = (from sBj in RankBjDataCache.Instance.GetRankBjModels
                                         join cBj in users on sBj.BjID equals cBj.ID
                                         select sBj)?.ToList();

                        var clientBjs = (from sBj in RankBjDataCache.Instance.GetRankBjModels
                                        join cBj in users on sBj.BjID equals cBj.ID
                                        select cBj)?.Distinct();

                        if (serverBjs != null && serverBjs.Count > 0)
                        {
                            foreach (var user in clientBjs)
                            {
                                var tmp = BjUserMatching(bj, user, serverBjs);
                                if (tmp != null)
                                    result.UserModels.Add(tmp);
                            }
                        }
                        break;

                    case 1:
                        var serverUsers = (from sUsers in RankUserModelDataCache.Instance.GetRankUserModels
                                           join cUsers in users on sUsers.UserID equals cUsers.ID
                                           select sUsers).ToList();

                        var clientUsers = (from sUsers in RankUserModelDataCache.Instance.GetRankUserModels
                                           join cUsers in users on sUsers.UserID equals cUsers.ID
                                           select cUsers)?.Distinct()?.ToList();

                        if (serverUsers != null && serverUsers.Count > 0)
                        {
                            var tmp = BigFanUserMatching(bj, clientUsers, serverUsers);
                            if (tmp != null)
                                result.UserModels.AddRange(tmp);
                        }
                        break;
                }
                
            });

            result.UserModels = result.UserModels?.Distinct()?.ToList();
            result.BjModel = bj;
            return result;
        }

        /// <summary>
        /// BJ 매칭
        /// </summary>
        /// <param name="bj"></param>
        /// <param name="user"></param>
        /// <param name="serverBjs"></param>
        /// <returns></returns>
        private UserModel BjUserMatching(BjModel bj, UserModel user, List<RankBjModel> serverBjs)
        {
            try
            {
                var resultBj = serverBjs.Find(b => b.BjID == user.ID);
                if (resultBj == null && string.IsNullOrEmpty(resultBj.BjID))
                    return null;

                user.Type = UserType.BJ;
                user.PictureUrl = resultBj.BjImgUrl;
                user.BjInfo = resultBj.Bjinfo;

                return user;
            }
            catch (Exception e)
            {
                string log = e.Message;
                return null;
            }
        }

        /// <summary>
        /// 빅팬 매칭
        /// </summary>
        /// <param name="bj"></param>
        /// <param name="clientUsers"></param>
        /// <param name="serverUsers"></param>
        /// <returns></returns>
        private List<UserModel> BigFanUserMatching(BjModel bj, List<UserModel> clientUsers, List<RankUserModel> serverUsers)
        {
            Parallel.For(0, clientUsers.Count, (clientUserIdx) =>
            {
                if (clientUsers[clientUserIdx].BJs == null)
                    clientUsers[clientUserIdx].BJs = new List<BjModel>();

                var matchingUser = serverUsers.FindAll(b => b.UserID == clientUsers[clientUserIdx].ID);
                int mainBigFanRanking = matchingUser?.OrderBy(m => m.BigFanRanking)?.FirstOrDefault()?.BigFanRanking ?? -1;

                if (mainBigFanRanking == 1)
                {
                    // 1등 회장
                    clientUsers[clientUserIdx].Type = UserType.King;
                }
                else if (mainBigFanRanking >= 2 && mainBigFanRanking <= 5)
                {
                    // 2~5등
                    clientUsers[clientUserIdx].Type = UserType.BigFan;
                }
                else if (mainBigFanRanking >= 6 && mainBigFanRanking <= 10)
                {
                    // 6~10등
                    clientUsers[clientUserIdx].Type = UserType.BigFan;

                }
                else if (mainBigFanRanking >= 11 && mainBigFanRanking <= 20)
                {
                    // 11~20등
                    clientUsers[clientUserIdx].Type = UserType.BigFan;
                }

                Parallel.For(0, matchingUser.Count, (matchingUserIdx) => {

                    var Addbj = new BjModel()
                    {
                        ID = matchingUser[matchingUserIdx].BjID,
                        Nic = matchingUser[matchingUserIdx].BjNic,
                    };

                    int bigFanRanking = matchingUser[matchingUserIdx].BigFanRanking;
                    if (bigFanRanking == 1)
                    {
                        // 1등 회장
                        Addbj.IconUrl = IconUrl.BulKing;
                    }
                    else if (bigFanRanking >= 2 && bigFanRanking <= 5)
                    {
                        // 2~5등
                        Addbj.IconUrl = IconUrl.BulRedHeart;
                    }
                    else if (bigFanRanking >= 6 && bigFanRanking <= 10)
                    {
                        // 6~10등
                        Addbj.IconUrl = IconUrl.BulYellowHeart;

                    }
                    else if (bigFanRanking >= 11 && bigFanRanking <= 20)
                    {
                        // 11~20등
                        Addbj.IconUrl = IconUrl.BulGrayHeart;
                    }

                    Addbj.Ranking = bigFanRanking;
                    clientUsers[clientUserIdx].BJs.Add(Addbj);
                });
            });

            return clientUsers;
        }
    }
}