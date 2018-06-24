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

                // 2. 캐시 된 데이터가 없는 경우 조회하여 캐시합니다.
                if (!ContainsKey(key))
                {
                    Set(key, () => {

                        var userDicModels = new Dictionary<string, List<UserModel>>();

                        // 중복 제거 후 루프 돌리기
                        var firstList = RankUserModelDataCache.Instance.GetRankUserModels.GroupBy(u => u.UserID).Select(c => c.First()).ToList();

                        Parallel.For(0, firstList.Count, (firstListIndex) =>
                        {
                            var tmp = RankUserModelToUserModel(firstList[firstListIndex]);

                            if (tmp != null)
                            {
                                var indexKey = tmp.ID.Substring(0, 1);
                                if (userDicModels.Keys.Any(d => d == indexKey))
                                {
                                    if (userDicModels[indexKey] == null)
                                        userDicModels[indexKey] = new List<UserModel>();
                                }
                                else
                                {
                                    userDicModels.Add(indexKey, new List<UserModel>());
                                }

                                userDicModels[indexKey].Add(tmp);
                            }

                        });

                        return userDicModels;
                    });
                }

                // 3. 캐시된 정보를 반환합니다.
                return Get(key);
            }
        }

        private UserModel RankUserModelToUserModel(RankUserModel rankUser)
        {
            var userModel = new UserModel()
            {
                ID = rankUser.UserID,
                Nic = rankUser.UserNick,
            };

            var matchingUser = new List<RankUserModel>();
            for (int Idx = 0; Idx < RankUserModelDataCache.Instance.GetRankUserModels.Count(); Idx++)
            {
                if (RankUserModelDataCache.Instance.GetRankUserModels[Idx].UserID == rankUser.UserID)
                {
                    matchingUser.Add(RankUserModelDataCache.Instance.GetRankUserModels[Idx]);
                }
            }

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
                userModel.BJs.Add(Addbj);
            });

            return userModel;
        }

        public void RefreshUserDicModels()
        {
            Refresh(KeyOfUserDicModels);
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