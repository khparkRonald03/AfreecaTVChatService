using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvjRestWebApi.DataCache
{
    public class BjDicModelsCache : DataCacheCore<Dictionary<string, List<UserModel>>>
    {
        private static BjDicModelsCache instance = null;
        private static readonly object SycStaticLock = new object();
        /// <summary>
        /// Singleton Instance
        /// </summary>
        public static BjDicModelsCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SycStaticLock)
                    {
                        if (instance == null)
                        {
                            instance = new BjDicModelsCache();
                        }
                    }
                }
                return instance;
            }
        }

        // 0. 캐시 데이터 별 고유 키를 정의하여 줍니다.
        private readonly string KeyOfGetBjDicModels = "KeyOfGetBjDicModels";

        public Dictionary<string, List<UserModel>> GetBjDicModels
        {
            get
            {
                // 1. 캐시에 저장할 키는  하나로 공유하여 전체가 사용할 수 있도록 합니다.
                string key = KeyOfGetBjDicModels;

                // 2. 캐시 된 데이터가 없는 경우 조회하여 캐시합니다.
                if (!ContainsKey(key))
                {
                    Set(key, () => {

                        var bjDicModels = new Dictionary<string, List<UserModel>>();

                        if (RankBjDataCache.Instance.GetRankBjModels != null && RankBjDataCache.Instance.GetRankBjModels.Count > 0)
                        {
                            Parallel.For(0, RankBjDataCache.Instance.GetRankBjModels.Count, (Idx) =>
                            {
                                var bjModel = RankBjDataCache.Instance.GetRankBjModels[Idx];

                                var tmp = RankBjModelToUserModel(bjModel);
                                if (tmp != null)
                                {
                                    var indexKey = tmp.ID.Substring(0, 1);
                                    if (bjDicModels.Keys.Any(d => d == indexKey))
                                    {
                                        if (bjDicModels[indexKey] == null)
                                            bjDicModels[indexKey] = new List<UserModel>();
                                    }
                                    else
                                    {
                                        bjDicModels.Add(indexKey, new List<UserModel>());
                                    }

                                    bjDicModels[indexKey].Add(tmp);
                                }

                            });
                        }

                        return bjDicModels;
                    });
                }

                // 3. 캐시된 정보를 반환합니다.
                return Get(key);
            }
        }

        private UserModel RankBjModelToUserModel(RankBjModel serverBjs)
        {
            try
            {
                var user = new UserModel
                {
                    ID = serverBjs.BjID,
                    Type = UserType.BJ,
                    PictureUrl = serverBjs.BjImgUrl,
                    RankingInfo = serverBjs,
                    BjInfo = serverBjs.Bjinfo
                };

                return user;
            }
            catch (Exception e)
            {
                string log = e.Message;
                return null;
            }
        }

        public void RefreshUserDicModels()
        {
            Refresh(KeyOfGetBjDicModels);
        }

        /// <summary>
        /// 재 조회 주기 가져오기 (시간단위)
        /// </summary>
        /// <returns></returns>
        protected override int GetDuration()
        {
            return 24*100;
        }
    }
}