using avj.BizDac;
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
        public JsonModel UsersMatching(JsonModel jsonModel)
        {
            var bj = jsonModel.BjModel;
            var users = jsonModel.UserModels;

            if (bj == null)
                return null;

            if (users == null || users.Count <= 0)
                return null;

            // 여기는 bj 필터 -> jsonModel에 필터할 값 bj아이디 넣기 인증 안된 접근 차단
            var g5Biz = new BizG5_member();
            var certInfos = g5Biz.GetG5_memberModelsByAbjID(bj.LoginID2);
            if (certInfos == null || certInfos.Count <= 0)
                return null;

            var certInfo = certInfos.FirstOrDefault();
            var level = certInfo.mb_level;
            var date = certInfo.mb_3;
            DateTime.TryParse(date, out DateTime expireDate);
            var days = (DateTime.Now - expireDate).Days;

            if (level < 4)
                return null;
            if (string.IsNullOrEmpty(date) || days > 0)
                return null;

            var result = new JsonModel();
            if (BjDicModelsCache.Instance.IsCache() && UserDicModelsCache.Instance.IsCache())
            {
                result = ActionType1(users);
            }
            else
            {
                result = ActionType2(bj, users);
            }

            result.UserModels = result.UserModels?.Distinct()?.ToList();
            result.BjModel = bj;
            return result;
        }

        private JsonModel ActionType1(List<UserModel> users)
        {
            var result = new JsonModel
            {
                UserModels = new List<UserModel>()
            };

            for (int Idx = 0; Idx < users.Count; Idx++)
            {
                var user = users[Idx];
                var firstChar = user.ID.Substring(0, 1);

                var bjDicModels = BjDicModelsCache.Instance.GetBjDicModels;
                var userDicModels = UserDicModelsCache.Instance.GetUserDicModels;

                if (bjDicModels.Keys.Any(k => k == firstChar))
                {
                    for (int bjIdx = 0; bjIdx < bjDicModels[firstChar].Count; bjIdx++)
                    {
                        if (bjDicModels[firstChar][bjIdx].ID == user.ID)
                        {
                            result.UserModels.Add(bjDicModels[firstChar][bjIdx]);
                            continue;
                        }
                    }

                }
                else if (userDicModels.Keys.Any(k => k == firstChar))
                {
                    for (int userIdx = 0; userIdx < userDicModels[firstChar].Count; userIdx++)
                    {
                        if (userDicModels[firstChar][userIdx].ID == user.ID)
                        {
                            result.UserModels.Add(userDicModels[firstChar][userIdx]);
                            continue;
                        }
                    }

                }
            }

            return result;
        }

        private JsonModel ActionType2(BjModel bj, List<UserModel> users)
        {
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
                                         select cBj)?.Distinct().ToList();

                        if (serverBjs != null && serverBjs.Count > 0)
                        {
                            Parallel.For(0, clientBjs.Count, (Idx) =>
                            {
                                var tmp = BjUserMatching(bj, clientBjs[Idx], serverBjs);
                                if (tmp != null)
                                    result.UserModels.Add(tmp);
                            });
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
                var resultBj = new RankBjModel();
                for (int Idx = 0; Idx < serverBjs.Count(); Idx++)
                {
                    if (serverBjs[Idx].BjID == user.ID)
                    {
                        resultBj = serverBjs[Idx];
                        break;
                    }
                }
                if (resultBj == null && string.IsNullOrEmpty(resultBj.BjID))
                    return null;

                user.Type = UserType.BJ;
                user.PictureUrl = resultBj.BjImgUrl;
                user.RankingInfo = resultBj;
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

                //var matchingUser = serverUsers.FindAll(b => b.UserID == clientUsers[clientUserIdx].ID);
                var matchingUser = new List<RankUserModel>();
                for (int Idx = 0; Idx < serverUsers.Count(); Idx++)
                {
                    if (serverUsers[Idx].UserID == clientUsers[clientUserIdx].ID)
                        matchingUser.Add(serverUsers[Idx]);
                }
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

        [HttpPost]
        [Route("Matching/Refresh")]
        public JsonModel Refresh(JsonModel text)
        {
            if (text.Text == "rfsStart")
            {
                BjDicModelsCache.Instance.RefreshUserDicModels();
                UserDicModelsCache.Instance.RefreshUserDicModels();
            }
            text.Text = "end";
            return text;
        }

        [HttpPost]
        [Route("Matching/CheckVersion")]
        public JsonModel CheckVersion(JsonModel jsonModel)
        {
            if (jsonModel == null)
                return null;

            if (jsonModel.BjModel == null)
                return null;

            if (string.IsNullOrEmpty(jsonModel.BjModel.LoginID))
                return null;

            var settingBiz = new BizRankCollectorSettings();
            var setting = settingBiz.GetSettings();

            var loginIdArray = jsonModel.BjModel.LoginID.Split(new string[] { setting.AbjIDSplitString }, StringSplitOptions.RemoveEmptyEntries);

            if (loginIdArray.Length == 1)
            {
                jsonModel.BjModel.LoginID2 = loginIdArray[0];
                jsonModel.BjModel.ChatID = loginIdArray[0];
            }
            else if (loginIdArray.Length == 2)
            {
                jsonModel.BjModel.LoginID2 = loginIdArray[1];
                jsonModel.BjModel.ChatID = loginIdArray[0];
            }

            var g5Biz = new BizG5_member();
            var result = g5Biz.GetG5_memberModelsByAbjID(jsonModel.BjModel.LoginID2);

            if (result != null && result.Count > 0)
            {
                var member = result.FirstOrDefault();

                var level = member.mb_level;
                var date = member.mb_3;
                DateTime.TryParse(date, out DateTime expireDate);
                var days = (DateTime.Now - expireDate).Days;

                if (level >= 4)
                {
                    jsonModel.BjModel.CertificationFlag = true;
                }
                else if (level < 4)
                {
                    jsonModel.BjModel.CertificationFlag = false;
                    jsonModel.BjModel.ExpireMessage = "사용기간이 만료되어 프로그램이 종료 됩니다.";
                }
                if (string.IsNullOrEmpty(date) || days > 0)
                {
                    // 만료
                    jsonModel.BjModel.ExpireFlag = true;
                    jsonModel.BjModel.ExpireMessage = "사용기간이 만료되어 프로그램이 종료 됩니다.";
                }
                else if (!string.IsNullOrEmpty(setting.ClientVersion) && setting.ClientVersion != jsonModel.BjModel.ClientVersion)
                {
                    jsonModel.BjModel.IsNewUpload = true;
                    jsonModel.BjModel.VersionMessage = "새 버전이 업로드 되었습니다. \r\n사이트에서 새 버전을 다운받아주세요.";
                }

                jsonModel.BjModel.ExpireDate = expireDate;
                jsonModel.BjModel.ClientLevel = member.mb_level;

            }

            return jsonModel;
        }
    }
}