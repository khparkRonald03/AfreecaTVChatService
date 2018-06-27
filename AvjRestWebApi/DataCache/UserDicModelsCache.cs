using avj.BizDac;
using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AvjRestWebApi.DataCache
{
    public class UserDicModelsCache : DataCacheCore<Dictionary<string, List<UserModel>>>
    {
        private static UserDicModelsCache instance = null;
        private static readonly object SycStaticLock = new object();

        private BizAbjLog bizAbjLog = new BizAbjLog();

        /// <summary>
        /// Singleton Instance
        /// </summary>
        public static UserDicModelsCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SycStaticLock)
                    {
                        if (instance == null)
                        {
                            instance = new UserDicModelsCache();
                        }
                    }
                }
                return instance;
            }
        }

        // 0. 캐시 데이터 별 고유 키를 정의하여 줍니다.
        private readonly string KeyOfUserDicModels = "KeyOfUserDicModels";

        public Dictionary<string, List<UserModel>> GetUserDicModels
        {
            get
            {
                // 1. 캐시에 저장할 키는  하나로 공유하여 전체가 사용할 수 있도록 합니다.
                string key = KeyOfUserDicModels;

                bizAbjLog.SetAbjLog($"UserDicModelsCache 캐시 있는지 확인 : {ContainsKey(key).ToString()}");
                // 2. 캐시 된 데이터가 없는 경우 조회하여 캐시합니다.
                if (!ContainsKey(key))
                {
                    bizAbjLog.SetAbjLog($"UserDicModelsCache 캐시 없음 Set함수 시작");

                    Set(key, () => {

                        var start = DateTime.Now;
                        bizAbjLog.SetAbjLog($"UserDicModelsCache Set함수 시작 시간 : {start.ToString()}");

                        var userDicModels = new Dictionary<string, List<UserModel>>();
                        var biz = new BizUserRank();
                        var firstChars = biz.GetFirstCharListByRankUserModels();
                        var allUserModels = biz.GetAllRankUserModels();
                        // 36건
                        for (int Idx = 0; Idx < firstChars.Count; Idx++)
                        {
                            var firstChar = firstChars[Idx];
                            var rankUserModels = biz.GetFirstCharRankUserModels(firstChar.FirstChar);

                            var userModels = new List<UserModel>();

                            // 몇백 ~ 몇천
                            //Parallel.For(0, rankUserModels.Count, (index) => {
                            for (int index = 0; index < rankUserModels.Count; index++)
                            {
                                var tmp = RankUserModelToUserModel(rankUserModels[index], allUserModels);
                                lock (SycStaticLock)
                                {
                                    userModels.Add(tmp);
                                }
                            }
                            //);

                            userDicModels.Add(firstChar.FirstChar, userModels);
                        }

                        var end = (DateTime.Now - start).Minutes;
                        bizAbjLog.SetAbjLog($"UserDicModelsCache Set함수 종료 시간 : {DateTime.Now.ToString()}, 런닝타임 : {end.ToString()}");

                        return userDicModels;
                    });
                }

                // 3. 캐시된 정보를 반환합니다.
                return Get(key);
            }
        }

        private UserModel RankUserModelToUserModel(RankUserModel rankUser, List<RankUserModel> allUserModels)
        {
            var userModel = new UserModel()
            {
                ID = rankUser.UserID,
                Nic = rankUser.UserNick,
            };

            //var biz = new BizUserRank();
            //var matchingUser = biz.GetAllRankUserModelsByUserID(rankUser.UserID);
            var matchingUser = allUserModels.FindAll(a => a.UserID == rankUser.UserID);

            int mainBigFanRanking = matchingUser?.OrderBy(m => m.BigFanRanking)?.FirstOrDefault()?.BigFanRanking ?? -1;

            if (mainBigFanRanking == 1)
            {
                // 1등 회장
                userModel.Type = UserType.King;
            }
            else if (mainBigFanRanking >= 2 && mainBigFanRanking <= 5)
            {
                // 2~5등
                userModel.Type = UserType.BigFan;
            }
            else if (mainBigFanRanking >= 6 && mainBigFanRanking <= 10)
            {
                // 6~10등
                userModel.Type = UserType.BigFan;
            }
            else if (mainBigFanRanking >= 11 && mainBigFanRanking <= 20)
            {
                // 11~20등
                userModel.Type = UserType.BigFan;
            }

            // 몇건 ~ 십몇건
            for (int Idx = 0; Idx < matchingUser.Count; Idx++)
            {
                var Addbj = new BjModel()
                {
                    ID = matchingUser[Idx].BjID,
                    Nic = matchingUser[Idx].BjNic,
                };

                int bigFanRanking = matchingUser[Idx].BigFanRanking;
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
                userModel.BJs.Add(Addbj);
            }

            return userModel;
        }

        public bool IsCache()
        {
            var t1 = ContainsKey(KeyOfUserDicModels);
            var t2 = GetValue(KeyOfUserDicModels)?.Count ?? 0;
            return  t1 && t2 > 0;
        }

        public void RefreshUserDicModels()
        {
            if (!Refresh(KeyOfUserDicModels))
            {
                var ttt = GetUserDicModels;
            }
        }

        /// <summary>
        /// 재 조회 주기 가져오기 (시간단위)
        /// </summary>
        /// <returns></returns>
        protected override int GetDuration()
        {
            return 24 * 100;
        }
    }
}