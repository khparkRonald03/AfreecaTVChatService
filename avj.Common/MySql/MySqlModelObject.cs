using System;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;

namespace avj.Common
{
    public abstract class MySqlModelObject
    {
        /// <summary>
        /// 데이터베이스에 관련된 기능을 제공합니다.
        /// </summary>
        protected MySqlControl sql = new MySqlControl();

        /// <summary>
        /// sql파라매터정보를 설정합니다.
        /// </summary>
        protected Hashtable sqlParameters = new Hashtable();

        /// <summary>
        /// sql 검색조건을 가져오거나 설정합니다.
        /// </summary>
        protected string sqlFilter = string.Empty;

        /// <summary>
        /// sql 정렬조건을 가져오거나 설정합니다.
        /// </summary>
        protected string sqlSort = string.Empty;

        /// <summary>
        /// sql 쿼리문을 가져오거나 설정합니다.
        /// </summary>
        protected string sqlQuery = string.Empty;

        /// <summary>
        /// 실행정보확인
        /// </summary>
        /// <param name="sqlQuery">쿼리내용</param>
        /// <param name="parameters">파라매터정보</param>
        /// <returns>실행정보텍스트</returns>
        protected string GetQueryText(string sqlQuery, Hashtable parameters)
        {
            string result = sqlQuery;

            if (parameters != null)
            {
                foreach (DictionaryEntry item in parameters)
                {
                    try
                    {
                        string keyValue = item.Key.ToString();
                        string itemValue = item.Value.ToString();

                        if (result.IndexOf("'%' + " + keyValue + " + '%'") > -1)
                        {
                            result = result.Replace("'%' + " + keyValue + " + '%'", "'%" + itemValue + "%'");
                        }
                        else if (result.IndexOf("'%'+" + keyValue + "+'%'") > -1)
                        {
                            result = result.Replace("'%'+" + keyValue + "+'%'", "'%" + itemValue + "%'");
                        }
                        else if (result.IndexOf("'%' +" + keyValue + "+ '%'") > -1)
                        {
                            result = result.Replace("'%' +" + keyValue + "+ '%'", "'%" + itemValue + "%'");
                        }
                        else if (result.IndexOf("'%' +" + keyValue + "+ '%'") > -1)
                        {
                            result = result.Replace("'%'+ " + keyValue + " +'%'", "'%" + itemValue + "%'");
                        }
                        else
                        {
                            result = result.Replace(keyValue, itemValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        string log = ex.Message;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 프로시저 실행정보확인
        /// </summary>
        /// <param name="sqlQuery">쿼리내용</param>
        /// <param name="parameters">파라매터정보</param>
        /// <returns>실행정보텍스트</returns>
        protected string GetSPQueryText(string spName, Hashtable parameters)
        {
            string result = "Call " + spName;

            if (parameters != null)
            {
                //해당 프로시저정보 가져오기
                DataTable dt = sql.GetSPInfo(spName);

                if (dt != null)
                {
                    result = result + "<br/>(<br/>";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string parameterName = dt.Rows[i]["parameter_name"].ToString();
                        string dataType = dt.Rows[i]["data_type"].ToString().ToLower();
                        string itemValue = "";

                        if (parameters.ContainsKey(parameterName) == false)
                        {
                            //문자형
                            //char, nchar
                            //varchar, nvarchar
                            //text, ntext
                            if (dataType.IndexOf("char") > -1 | dataType.IndexOf("text") > -1)
                            {
                                itemValue = "";
                            }

                            //숫자형
                            if (dataType.IndexOf("int") > -1 | dataType.IndexOf("double") > -1 | dataType.IndexOf("float") > -1 | dataType.IndexOf("decimal") > -1)
                            {
                                itemValue = "0";
                            }

                            //날짜형
                            if (dataType.IndexOf("date") > -1)
                            {
                                itemValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                        else
                        {
                            itemValue = parameters[parameterName]?.ToString();
                        }

                        if (i == 0)
                        {
                            result = result + " &nbsp;'" + itemValue ?? "null" + "' &nbsp;&nbsp;&nbsp;&nbsp; -- " + parameterName + "<br/>";
                        }
                        else
                        {
                            result = result + ", '" + itemValue ?? "null" + "' &nbsp;&nbsp;&nbsp;&nbsp; -- " + parameterName + "<br/>";
                        }
                    }

                    result = result + "<br/>);<br/>";
                }
                else
                {
                    result = "";
                }
            }

            return result;
        }

        /// <summary>
        /// 파라매터 설정 반환
        /// </summary>
        /// <param name="parameters">파라매터정보</param>
        /// <returns>실행정보텍스트</returns>
        protected Hashtable GetSqlParameters(Hashtable parameters)
        {
            Hashtable sqlParameters = new Hashtable();

            if (parameters != null)
            {
                foreach (DictionaryEntry item in parameters)
                {
                    sqlParameters.Add("P_" + item.Key.ToString(), item.Value);
                }
            }

            return sqlParameters;
        }

        /// <summary>
        /// 파라매터 설정 반환
        /// </summary>
        /// <param name="spName">프로시저명</param>
        /// <param name="parameters">파라매터정보</param>
        /// <returns>실행정보텍스트</returns>
        protected Hashtable GetSqlParameters(string spName, Hashtable parameters)
        {
            Hashtable sqlParameters = new Hashtable();

            if (parameters != null)
            {
                foreach (DictionaryEntry item in parameters)
                {
                    sqlParameters.Add("P_" + item.Key.ToString(), item.Value);
                }

                try
                {
                    //해당 프로시저의 나머지 파라매터 기본값으로 설정
                    DataTable dt = sql.GetSPInfo(spName);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string parameterName = dt.Rows[i]["parameter_name"].ToString();
                        string dataType = dt.Rows[i]["data_type"].ToString().ToLower();

                        if (sqlParameters.ContainsKey(parameterName) == false)
                        {
                            //문자형
                            //char, nchar
                            //varchar, nvarchar
                            //text, ntext
                            if (dataType.IndexOf("char") > -1 | dataType.IndexOf("text") > -1)
                            {
                                sqlParameters.Add(parameterName, "");
                            }

                            //숫자형
                            if (dataType.IndexOf("int") > -1 | dataType.IndexOf("double") > -1 | dataType.IndexOf("float") > -1 | dataType.IndexOf("decimal") > -1)
                            {
                                sqlParameters.Add(parameterName, 0);
                            }

                            //날짜형
                            if (dataType.IndexOf("date") > -1)
                            {
                                sqlParameters.Add(parameterName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    string log = ex.Message;
                }
            }

            return sqlParameters;
        }

        /// <summary>
        /// 검증 후 데이터 반환 (검색 데이터용)
        /// </summary>
        /// <param name="search">검색조건</param>
        /// <returns>검증 데이터</returns>
        protected Hashtable GetValidationData(Hashtable search)
        {
            Hashtable data = new Hashtable();

            if (search != null)
            {
                foreach (DictionaryEntry item in search)
                {
                    try
                    {
                        string keyValue = item.Key.ToString();
                        string itemValue = item.Value.ToString();

                        //null이면 빈값으로 설정
                        if (string.IsNullOrEmpty(itemValue) == true)
                        {
                            itemValue = string.Empty;
                        }

                        if (itemValue != string.Empty)
                        {
                            //html태그가 포함되어있다면 빈값으로 설정
                            if (Regex.IsMatch(itemValue, @"<[^>]+>") == true)
                            {
                                itemValue = string.Empty;
                            }

                            //향후 방어할 필요가 있는 문자가 생기면 추가...
                            //참고 : http://wikisecurity.net/guide:asp.net_%EA%B0%9C%EB%B0%9C_%EB%B3%B4%EC%95%88_%EA%B0%80%EC%9D%B4%EB%93%9C
                            string[] arrCheck = new string[] {
                                                                "/*"
                                                              , "*/"
                                                              , "@@"
                                                              , "char"
                                                              , "nchar"
                                                              , "varchar"
                                                              , "nvarchar"
                                                              , "alter"
                                                              , "begin"
                                                              , "cast"
                                                              , "create"
                                                              , "cursor"
                                                              , "declare"
                                                              , "select"
                                                              , "insert"
                                                              , "update"
                                                              , "delete"
                                                              , "drop"
                                                              , "execute"
                                                              , "fetch"
                                                              , "kill"
                                                              , "open"
                                                              , "sys"
                                                              , "sysobjects"
                                                              , "syscolumns"
                                                              , "<script"
                                                              , "<iframe"
                                                              , "<frame"
                                                              , "<frameset"
                                                              , "<applet"
                                                              , "<html"
                                                              , "<meta"
                                                              , "<object"
                                                         };

                            if (itemValue != string.Empty)
                            {
                                //내용 체크 : 방어 문자가 포함되어있다면 빈값으로 설정
                                for (int i = 0; i < arrCheck.Length; i++)
                                {
                                    if (itemValue.ToLower().IndexOf(arrCheck[i]) > -1)
                                    {
                                        itemValue = string.Empty;
                                    }
                                }
                            }
                        }

                        data.Add(keyValue, System.Web.HttpUtility.HtmlEncode(itemValue));
                    }
                    catch (Exception ex)
                    {
                        string log = ex.Message;
                        data.Add(item.Key.ToString(), "");
                    }
                }
            }

            return data;
        }
    }
}
