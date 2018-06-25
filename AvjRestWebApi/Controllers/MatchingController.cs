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

            if (bj == null || string.IsNullOrEmpty(bj.ID))
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

            result.UserModels = result.UserModels?.Distinct()?.ToList();
            result.BjModel = bj;
            return result;
        }

        [HttpPost]
        [Route("Matching/Refresh")]
        public string Refresh(string text)
        {
            if (text == "rfsStart")
            {
                BjDicModelsCache.Instance.RefreshUserDicModels();
                UserDicModelsCache.Instance.RefreshUserDicModels();
            }
            return "end";
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
                else
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