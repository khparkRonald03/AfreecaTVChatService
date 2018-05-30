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

            // bj 확인은 테스트 기간때 기능 추가하기 ### 
            var result = new JsonModel
            {
                UserModels = new List<UserModel>()
            };

            var bj = jsonModel.BjModel;
            var users = jsonModel.UserModels;

            Parallel.For(0, 2, (i) =>
            {
                switch (i)
                {
                    case 0:
                        var serverBjs = (from sBj in RankBjDataCache.Instance.GetRankBjModels
                                         join cBj in users on sBj.BjID equals cBj.ID
                                         select sBj)?.ToList();

                        var clientBjs = from sBj in RankBjDataCache.Instance.GetRankBjModels
                                        join cBj in users on sBj.BjID equals cBj.ID
                                        select cBj;

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
                                           select cUsers)?.ToList();

                        if (serverUsers != null && serverUsers.Count > 0)
                        {
                            var tmp = BigFanUserMatching(bj, clientUsers, serverUsers);
                            if (tmp != null)
                                result.UserModels.AddRange(tmp);
                        }
                        break;
                }
                
            });

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

                return user;
            }
            catch(Exception e)
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
            for (int clientUserIdx = 0; clientUserIdx < clientUsers.Count; clientUserIdx++)
            {
                if (clientUsers[clientUserIdx].BJs == null)
                    clientUsers[clientUserIdx].BJs = new List<BjModel>();

                var matchingUser = serverUsers.FindAll(b => b.UserID == clientUsers[clientUserIdx].ID);

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
                        clientUsers[clientUserIdx].Type = UserType.King;
                        Addbj.IconUrl = IconUrl.BulKing;
                    }
                    else if (bigFanRanking >= 2 && bigFanRanking <= 5)
                    {
                        // 2~5등
                        clientUsers[clientUserIdx].Type = UserType.BigFan;
                        Addbj.IconUrl = IconUrl.BulRedHeart;
                    }
                    else if (bigFanRanking >= 6 && bigFanRanking <= 10)
                    {
                        // 6~10등
                        clientUsers[clientUserIdx].Type = UserType.BigFan;
                        Addbj.IconUrl = IconUrl.BulYellowHeart;

                    }
                    else if (bigFanRanking >= 11 && bigFanRanking <= 20)
                    {
                        // 11~20등
                        clientUsers[clientUserIdx].Type = UserType.BigFan;
                        Addbj.IconUrl = IconUrl.BulGrayHeart;
                    }

                    clientUsers[clientUserIdx].BJs.Add(Addbj);
                });

            }

            return clientUsers;
        }
    }
}