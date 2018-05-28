using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace avj.Common
{
    public class SqlServer
    {
        //private string SqlServerConnectionString = ConfigurationManager.AppSettings["SqlServerConn"].ToString();
        //private readonly string SqlServerConnectionString = "Server=ronald03s-mssql-01.cfav9hfzfs7y.ap-northeast-2.rds.amazonaws.com;Database=Ronald03s;Persist Security Info=True;User ID=ronald03;Password=ky850224";
        private readonly string SqlServerConnectionString = ConfigurationManager.AppSettings["SqlServerConn"].ToString();

        public Tresult GetDataModel<Tresult>(string procedure, List<SqlServerParam> dacParams)
        {
            GetValidationData(ref dacParams);
            var ds = GetDataSet(procedure, dacParams);
            return GetDataToModel<Tresult>(ds);
        }

        private List<SqlServerParam> GetValidationData(ref List<SqlServerParam> dacParams)
        {
            GetValidationData(ref dacParams);

            var data = new List<SqlServerParam>();

            if (dacParams != null)
            {
                foreach (var item in dacParams)
                {
                    try
                    {
                        string keyValue = item.ParamName;
                        string itemValue = item.ParamValue?.ToString() ?? string.Empty;

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

                        item.ParamValue = System.Web.HttpUtility.HtmlEncode(itemValue);
                    }
                    catch (Exception ex)
                    {
                        string lgo = ex.Message;
                        item.ParamValue = string.Empty;
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// 데이터 셋 가져오기
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string procedure, List<SqlServerParam> dacParams)
        {
            GetValidationData(ref dacParams);
            var ds = new DataSet();
            var param = TransferParamsType(dacParams);

            using (SqlConnection conn = new SqlConnection(SqlServerConnectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(procedure, conn)
                {
                    CommandType = CommandType.StoredProcedure
                }; // 이건 어떻게 Dispose 할까 고민중... 안해도 될라나?
                cmd.Parameters.AddRange(param.ToArray());
                
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(ds);
                }
            }
                
            return ds;
        }

        private Tresult GetDataToModel<Tresult>(DataSet ds)
        {
            var obj = Activator.CreateInstance<Tresult>();
            obj = (Tresult)ds.Tables[0].ToDataObject(typeof(Tresult));
            return obj;
        }

        private List<SqlParameter> TransferParamsType(List<SqlServerParam> dacParams)
        {
            GetValidationData(ref dacParams);
            var mySqlParams = new List<SqlParameter>();
            foreach (var dacParam in dacParams)
            {
                mySqlParams.Add(AddParam(dacParam.ParamName, (SqlDbType)dacParam.DbType, dacParam.ParamValue));
            }

            return mySqlParams;
        }

        /// <summary>
        /// 파라미터추가
        /// </summary>
        /// <param name="parametername"></param>
        /// <param name="dbType"></param>
        /// <param name="pmDirection"></param>
        /// <param name="pmValue"></param>
        /// <returns></returns>
        public SqlParameter AddParam(string parametername, SqlDbType dbType, object pmValue)
        {
            return new SqlParameter(parametername, dbType)
            {
                SqlValue = pmValue
            };
        }

        // 긁어온 코드 사용안함
        private DataSet SqlString(string city, DateTime date)
        {
            DataSet ds = new DataSet();

            SqlConnection conn = new SqlConnection(SqlServerConnectionString);
            conn.Open();

            // 2개의 파라미터 정의 (항상 @로 시작)
            string sql = "SELECT * FROM Employees WHERE City=@city AND [Hire Date]>=@date";
            SqlCommand cmd = new SqlCommand(sql, conn);

            // 각 파라미터 타입 및 값 입력
            SqlParameter paramCity = new SqlParameter("@city", SqlDbType.NVarChar, 15)
            {
                Value = city
            };
            // SqlCommand 객체의 Parameters 속성에 추가
            cmd.Parameters.Add(paramCity);

            SqlParameter paramHire = new SqlParameter("@date", SqlDbType.DateTime)
            {
                Value = date
            };
            cmd.Parameters.Add(paramHire);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();
            return ds;
        }
    }
}
