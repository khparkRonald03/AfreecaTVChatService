using avj.BizDac;
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

                        var start = DateTime.Now;

                        var bjDicModels = new Dictionary<string, List<UserModel>>();
                        var biz = new BizBjRank();
                        var firstChars = biz.GetFirstCharListByRankBjModels();
                        for (int Idx = 0; Idx < firstChars.Count; Idx++)
                        {
                            var firstChar = firstChars[Idx];
                            var rankUserModels = biz.GetFirstCharRankBjModels(firstChar.FirstChar);

                            var userModels = new List<UserModel>();
                            for (int index = 0; index < rankUserModels.Count;  index++)
                            {
                                var tmp = RankBjModelToUserModel(rankUserModels[index]);
                                if (tmp != null)
                                    userModels.Add(tmp);
                            }

                            bjDicModels.Add(firstChar.FirstChar, userModels);
                        }

                        var end = (DateTime.Now - start).Minutes;

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

        public bool IsCache()
        {
            var t1 = ContainsKey(KeyOfGetBjDicModels);
            var t2 = GetValue(KeyOfGetBjDicModels)?.Count ?? 0;
            return t1 && t2 > 0;
        }

        public void RefreshUserDicModels()
        {
            var refresh = GetBjDicModels;
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