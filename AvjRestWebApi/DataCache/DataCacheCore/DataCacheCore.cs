using avj.BizDac;
using System;
using System.Collections.Generic;
using System.Data;

namespace AvjRestWebApi.DataCache
{
    public class DataCacheCore<T>
    {
        private BizAbjLog bizAbjLog = new BizAbjLog();

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
            // -1이면 현재날짜가 작은거 (만료 안됨)
            // 1이면 현재날짜가 큰거 (만료 됨)
            return (result == 1);
        }
        
        /// <summary>
        /// 재 조회 주기 가져오기
        /// </summary>
        /// <returns></returns>
        protected virtual int GetDuration()
        {
            try
            {
                return 24 * 100;
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return 24 * 100;
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
                bizAbjLog.SetAbjLog($"DataCacheCore - ContainsKey ");
                return List.ContainsKey(key);
            }
            catch (Exception ex)
            {
                bizAbjLog.SetAbjLog($"DataCacheCore - ContainsKey err : {ex.Message}");
                string log = ex.Message;
                return false;
            }
        }

        protected T GetValue(string key)
        {
            try
            {
                if (List.ContainsKey(key))
                {
                    return List[key].Value;
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return default(T);
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

            bizAbjLog.SetAbjLog($"DataCacheCore - Set - Duration : {Duration.ToString()}");
            bizAbjLog.SetAbjLog($"DataCacheCore - Set 값 가져오기 실행 전");

            var value = new CachedValue()
            {
                Value = act(),
                CachedDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddHours(Duration),
                SetAction = act
            };

            bizAbjLog.SetAbjLog($"DataCacheCore - Set 값 가져오기 실행 후");

            lock (SycRoot)
            {
                try
                {
                    if (List.ContainsKey(key))
                    {
                        bizAbjLog.SetAbjLog($"DataCacheCore 값초기화: 키가 있음");
                        List[key] = value;
                    }
                    else
                    {
                        bizAbjLog.SetAbjLog($"DataCacheCore 값초기화: 키가 없음");
                        List.Add(key, value);
                    }
                }
                catch (Exception ex)
                {
                    string log = ex.Message;
                    bizAbjLog.SetAbjLog($"DataCacheCore - err : {log}");
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
                bizAbjLog.SetAbjLog($"DataCacheCore Get 시작");
                
                if (!List.ContainsKey(key))
                    return default(T);

                var val = List[key];
                if (IsExpired(val))
                {
                    bizAbjLog.SetAbjLog($"DataCacheCore Get 기간 만료로 다시 받아옴");
                    val.Value = val.SetAction();
                    val.CachedDate = DateTime.Now;
                    val.ExpireDate = DateTime.Now.AddDays(Duration);
                }

                bizAbjLog.SetAbjLog($"DataCacheCore Get 종료");
                return val.Value;
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                bizAbjLog.SetAbjLog($"DataCacheCore Get err : {log}");
                return default(T);
            }
        }

        /// <summary>
        /// 해당 키의 자료를 재 조회
        /// </summary>
        protected bool Refresh(string key)
        {
            lock (SycRoot)
            {
                try
                {
                    if (!List.ContainsKey(key))
                        return false;

                    CachedValue val = List[key];
                    val.Value = val.SetAction();
                    val.ExpireDate = DateTime.Now.AddDays(Duration);
                    return true;

                }
                catch (Exception ex)
                {
                    string log = ex.Message;
                    return false;
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