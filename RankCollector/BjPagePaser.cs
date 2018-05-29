using DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankCollector
{
    public class BjPagePaser : CommonWeb
    {
        public List<RankUserModel> GetBigFans(string bjID)
        {
            var result = new List<RankUserModel>();

            var url = $"http://live.afreecatv.com:8057/api/best_bj_action.php?szAction=GetBestBJDetail&szType=json&uid={bjID}&szBeforeCallBack=_bigfan&callback=_bigfan";

            var bigFanString = GetUserRank(bjID, url);

            bigFanString = bigFanString?.Replace("_bigfan(", "")?.Replace(")", "") ?? string.Empty;

            if (string.IsNullOrEmpty(bigFanString))
                return result;

            RankBigFan bigFan = null;
            try
            {
                bigFan = JsonConvert.DeserializeObject<RankBigFan>(bigFanString);
            }
            catch (Exception e)
            {
                string log = e.Message;
                return result;
            }

            for (int Idx = 0; Idx < bigFan.Starballoon_top.Count; Idx++)
            {
                var userNick = bigFan.Starballoon_top[Idx].User_nick;
                if (userNick.Contains("#") && userNick.Contains(":3"))
                {
                    userNick = userNick?.Replace(":3", "")?.Substring(1) ?? userNick;
                }

                var item = new RankUserModel()
                {
                    BjID = bjID,
                    UserID = bigFan.Starballoon_top[Idx].User_id,
                    UserNick = userNick,
                    BigFanRanking = Idx + 1
                };

                result.Add(item);
            }

            return result;
        }

        public List<RankUserModel> GetSupporters(string bjID)
        {
            var result = new List<RankUserModel>();

            // 사용자별 구분 문자열
            int unicode = 004;
            char character = (char)unicode;
            string split1 = character.ToString();

            // 아이디, 닉네임 구분 문자열
            int unicode2 = 002;
            char character2 = (char)unicode2;
            string split2 = character2.ToString();

            // 아이디에 붙어 있는 0- 문자 제거
            int unicode3 = 006;
            char character3 = (char)unicode3;
            string replaceString = $"0{character3.ToString()}";


            var url = $"http://live.afreecatv.com:8057/api/best_bj_action.php?szAction=GetSupporter&szType=json&uid={bjID}&szBeforeCallBack=_supporter&callback=_supporter";  // bj아이디 들어감

            var supporter = GetUserRank(bjID, url, false);
            supporter = supporter?.Replace("_supporter(", "")?.Replace(");", "") ?? string.Empty;

            if (string.IsNullOrEmpty(supporter))
                return result;

            var supporters = JsonConvert.DeserializeObject<Supporters>(supporter);

            var top = supporters?.CHANNEL?.SUPPORTER_LIST?.FirstOrDefault()?.List?.FirstOrDefault()?.Sticker_top ?? string.Empty;

            if (string.IsNullOrEmpty(top))
                return result;

            var topSplit1 = top.Split(new string[] { split1 }, StringSplitOptions.RemoveEmptyEntries);
            
            for (int Idx = 0; Idx < topSplit1.Length; Idx++)
            {
                if (topSplit1[Idx] == "0")
                    break;

                var userInfo = topSplit1[Idx].Split(new string[] { split2 }, StringSplitOptions.RemoveEmptyEntries);
                if (userInfo.Length != 2)
                    continue;

                var item = new RankUserModel()
                {
                    BjID = bjID,
                    UserID = userInfo[0].Replace(replaceString, ""),
                    UserNick = userInfo[1],
                    SupportRanking = Idx + 1
                };

                result.Add(item);
            }

            return result;
        }
    }
}
