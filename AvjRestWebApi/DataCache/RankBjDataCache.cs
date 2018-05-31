﻿using avj.BizDac;
using DataModels;
using System.Collections.Generic;

namespace AvjRestWebApi.DataCache
{
    public class RankBjDataCache : DataCacheCore<List<RankBjModel>>
    {
        private static RankBjDataCache instance = null;
        private static readonly object SycStaticLock = new object();
        /// <summary>
        /// Singleton Instance
        /// </summary>
        public static RankBjDataCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SycStaticLock)
                    {
                        if (instance == null)
                        {
                            instance = new RankBjDataCache();
                        }
                    }
                }
                return instance;
            }
        }

        // 0. 캐시 데이터 별 고유 키를 정의하여 줍니다.
        private readonly string KeyOfGetRankBjModels = "KeyOfGetRankBjModels";

        public List<RankBjModel> GetRankBjModels
        {
            get
            {
                // 1. 캐시에 저장할 키는  하나로 공유하여 전체가 사용할 수 있도록 합니다.
                string key = KeyOfGetRankBjModels;

                // 2. 캐시 된 데이터가 없는 경우 조회하여 캐시합니다.
                if (!ContainsKey(key))
                {
                    Set(key, () => {

                        var biz = new BizBjRank();
                        var result = biz.GetAllRankBjModels();

                        return result;
                    });
                }

                // 3. 캐시된 정보를 반환합니다.
                return Get(key);
            }
        }

        public void RefreshRankBjModels()
        {
            Refresh(KeyOfGetRankBjModels);
        }

        /// <summary>
        /// 재 조회 주기 가져오기 (시간단위)
        /// </summary>
        /// <returns></returns>
        protected override int GetDuration()
        {
            return 24;
        }
    }
}