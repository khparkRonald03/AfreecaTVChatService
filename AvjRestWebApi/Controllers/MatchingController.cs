using AvjRestWebApi.DataCache;
using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AvjRestWebApi.Controllers
{
    public class MatchingController : ApiController
    {
        [HttpPost]
        [Route("")]
        public UserModel UserMatching (BjModel bj, UserModel user)
        {
            //user.Id = Guid.NewGuid();
            //UsersController.Users.Add(user);

            // bj 확인은 테스트 기간때 기능 추가하기 ### 

            var rankBjModels = RankBjDataCache.Instance.GetRankBjModels;
            var rankUserModels = RankUserModelDataCache.Instance.GetRankUserModels;

            // BJ 매칭
            if (rankBjModels.Any(b => b.BjID == user.ID))
            {
                var resultBj = rankBjModels.Where(b => b.BjID == user.ID)?.FirstOrDefault();

                user.Type = UserType.BJ;
                user.PictureUrl = resultBj.BjImgUrl;

                // BJ면 열혈팬이 될 수 없으니 리턴
                return user;
            }

            if (user.BJs == null)
                user.BJs = new List<BjModel>();

            // 빅팬 매칭
            if (rankUserModels.Any(b => b.BjID == user.ID))
            {
                foreach (var bjModel in rankBjModels)
                {
                    var matchingBjUser = rankUserModels.Where(b => b.BjID == bjModel.BjID);

                    foreach (var userModel in matchingBjUser)
                    {
                        if (userModel.BjID != user.ID)
                            continue;

                        var Addbj = new BjModel()
                        {
                            ID = bjModel.BjID,
                            Nic = bjModel.BjNick,
                            PictureUrl = bjModel.BjImgUrl,
                        };

                        int bigFanRanking = userModel.BigFanRanking;
                        if (bigFanRanking == 1)
                        {
                            // 1등면 회장
                            user.Type = UserType.King;
                            Addbj.IconUrl = IconUrl.BulKing;
                        }
                        else if (bigFanRanking >= 2 && bigFanRanking <= 5)
                        {
                            // 2~5등
                            user.Type = UserType.BigFan;
                            Addbj.IconUrl = IconUrl.BulRedHeart;
                        }
                        else if (bigFanRanking >= 6 && bigFanRanking <= 10)
                        {
                            // 6~10등
                            user.Type = UserType.BigFan;
                            Addbj.IconUrl = IconUrl.BulYellowHeart;

                        }
                        else if (bigFanRanking >= 11 && bigFanRanking <= 20)
                        {
                            // 11~20등
                            user.Type = UserType.BigFan;
                            Addbj.IconUrl = IconUrl.BulGrayHeart;
                        }

                        user.BJs.Add(Addbj);
                    }

                }

            }


            return user;
        }
    }
}