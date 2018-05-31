using System;
using System.Collections.Generic;
using System.Data;

namespace AvjRestWebApi.DataCache
{
    public class DataCacheCore<T>
    {
        /// <summary>
        /// 데이터 저장 클래스
        /// </summary>
        private class CachedValue
        {
            public T Value { get; set; }
            public DateTime? CachedDate { get; set; }
            public DateTime? ExpireDate { get; set; }
            public Func<T> SetAction { get; set; }
        }

        /// <summary>
        /// 재조회 주기
        /// </summary>
        private int Duration { get; set; }

        /// <summary>
        /// Lock개체
        /// </summary>
        private readonly object SycRoot = new object();

        /// <summary>
        /// 데이터 저장소
        /// </summary>
        private Dictionary<string, CachedValue> List = new Dictionary<string, CachedValue>();


        /// <summary>
        /// 만료여부를 점검
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private bool IsExpired(CachedValue val)
        {
            if(val.ExpireDate == null)
                return false;

            var result = DateTime.Compare(DateTime.Now, (DateTime)val.ExpireDate);
            return (result < 0);
        }
        
        /// <summary>
        /// 재 조회 주기 가져오기
        /// </summary>
        /// <returns></returns>
        protected virtual int GetDuration()
        {
            try
            {
                // DB에서 가져오는 걸로 바꾸기####################    
                return 10;
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return 10;
            }
        }
        
        /// <summary>
        /// 캐시에서 데이터가 존재하는지 점검
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected bool ContainsKey(string key)
        {
            try
            {
                return List.ContainsKey(key);
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 캐시에 저장
        /// </summary>
        /// <param name="key"></param>
        /// <param name="act"></param>
        protected void Set(string key, Func<T> act)
        {
            Duration = GetDuration();

            var value = new CachedValue()
            {
                Value = act(),
                CachedDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddHours(Duration),
                SetAction = act
            };

            lock (SycRoot)
            {
                try
                {
                    if (List.ContainsKey(key))
                    {
                        List[key] = value;
                    }
                    else
                    {
                        List.Add(key, value);
                    }
                }
                catch (Exception ex)
                {
                    string log = ex.Message;
                }
            }
        }

        /// <summary>
        /// 캐시에서 데이터를 조회
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected T Get(string key)
        {
            try
            {
                if (!List.ContainsKey(key))
                    return default(T);

                var val = List[key];
                if (IsExpired(val))
                {
                    val.Value = val.SetAction();
                    val.CachedDate = DateTime.Now;
                    val.ExpireDate = DateTime.Now.AddDays(Duration);
                }
                return val.Value;
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return default(T);
            }
        }

        /// <summary>
        /// 해당 키의 자료를 재 조회
        /// </summary>
        protected void Refresh(string key)
        {
            lock (SycRoot)
            {
                try
                {
                    CachedValue val = List[key];
                    val.Value = val.SetAction();
                    val.ExpireDate = DateTime.Now.AddDays(Duration);
                }
                catch (Exception ex)
                {
                    string log = ex.Message;
                }
            }
        }

        /// <summary>
        /// 해당 키를 캐시에서 삭제
        /// </summary>
        /// <param name="key">데이터의 키</param>
        protected void Remove(string key)
        {
            lock (SycRoot)
            {
                try
                {
                    if (List.ContainsKey(key))
                        List.Remove(key);
                }
                catch (Exception ex)
                {
                    string log = ex.Message;
                }
            }
        }

        /// <summary>
        /// 해당 키의 자료를 재 조회
        /// </summary>
        public void AllRefresh()
        {
            foreach (var item in List)
            {
                Refresh(item.Key);
            }

            Duration = GetDuration();
        }

        /// <summary>
        /// 전체 캐시를 초기화
        /// </summary>
        public void AllClear()
        {
            lock (SycRoot)
            {
                try
                {
                    List.Clear();
                }
                catch (Exception ex)
                {
                    string log = ex.Message;
                }
            }
        }
    }
}