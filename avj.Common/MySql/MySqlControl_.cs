using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace avj.Common
{
    public class MySqlControl_
    {
        private string sqlQuery         = string.Empty;
        private bool isProcedure        = true;
        private string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        private Hashtable parameters    = null;
        private MySqlCommand command    = null;

        /// <summary>
        /// Sql쿼리문을 가져오거나 설정합니다.
        /// </summary>
        public string SqlQuery
        {
            set { sqlQuery = value; }
            get { return sqlQuery; }
        }

        /// <summary>
        /// 프로시저여부를 가져오거나 설정합니다. (기본값 : true)
        /// </summary>
        public bool IsProcedure
        {
            set { isProcedure = value; }
            get { return isProcedure; }
        }

        /// <summary>
        /// 파라매터정보를 가져오거나 설정합니다.
        /// </summary>
        public Hashtable Parameters
        {
            set { parameters = value; }
            get { return parameters; }
        }

        /// <summary>
        /// 연결정보를 가져오거나 설정합니다. (기본값 : web.config의 connectionStrings > DbConnectionString)
        /// </summary>
        public string ConnectionString
        {
            set { connectionString = value; }
            get { return connectionString; }
        }


        public MySqlControl_()
        {
            this.parameters = new Hashtable();
        }


        #region public 함수


        /// <summary>
        /// DataSet 데이터를 반환합니다. 
        /// </summary>
        /// <returns>DataSet 데이터</returns>
        public DataSet GetDataSet()
        {
            try
            {
                if (InjectionCodeCheck(this.sqlQuery) == false)
                {
                    return GetDataSet(this.sqlQuery, GetCommandType(this.isProcedure), SetSqlParameter(this.parameters), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// DataSet 데이터를 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <returns>DataSet 데이터</returns>
        public DataSet GetDataSet(string sqlQuery)
        {
            try
            {
                if (InjectionCodeCheck(sqlQuery) == false)
                {
                    return GetDataSet(sqlQuery, GetCommandType(false), SetSqlParameter(null), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// DataSet 데이터를 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="parameters">파라매터</param>
        /// <returns>DataSet 데이터</returns>
        public DataSet GetDataSet(string sqlQuery, Hashtable parameters)
        {
            try
            {
                if (InjectionCodeCheck(sqlQuery) == false)
                {
                    return GetDataSet(sqlQuery, GetCommandType(false), SetSqlParameter(parameters), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// DataSet 데이터를 반환합니다. (페이징 기능) 
        /// </summary>
        /// <param name="pageNo">페이지번호</param>
        /// <param name="pageSize">페이지사이즈</param>
        /// <returns>DataSet 데이터</returns>
        public DataSet GetDataSet(int pageNo, int pageSize)
        {
            try
            {
                if (InjectionCodeCheck(this.sqlQuery) == false)
                {
                    return GetDataSet(this.sqlQuery, pageNo, pageSize, GetCommandType(this.isProcedure), SetSqlParameter(this.parameters), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// DataSet 데이터를 반환합니다. (페이징 기능) 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="pageNo">페이지번호</param>
        /// <param name="pageSize">페이지사이즈</param>
        /// <returns>DataSet 데이터</returns>
        public DataSet GetDataSet(string sqlQuery, int pageNo, int pageSize)
        {
            try
            {
                if (InjectionCodeCheck(sqlQuery) == false)
                {
                    return GetDataSet(sqlQuery, pageNo, pageSize, GetCommandType(false), SetSqlParameter(null), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// DataSet 데이터를 반환합니다. (페이징 기능) 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="parameters">파라매터</param>
        /// <param name="pageNo">페이지번호</param>
        /// <param name="pageSize">페이지사이즈</param>
        /// <returns>DataSet 데이터</returns>
        public DataSet GetDataSet(string sqlQuery, Hashtable parameters, int pageNo, int pageSize)
        {
            try
            {
                if (InjectionCodeCheck(sqlQuery) == false)
                {
                    return GetDataSet(sqlQuery, pageNo, pageSize, GetCommandType(false), SetSqlParameter(parameters), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// DataTable 데이터를 반환합니다. 
        /// </summary>
        /// <returns>DataTable 데이터</returns>
        public DataTable GetDataTable()
        {
            try
            {
                if (InjectionCodeCheck(this.sqlQuery) == false)
                {
                    return GetDataTable(this.sqlQuery, GetCommandType(this.isProcedure), SetSqlParameter(this.parameters), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// DataTable 데이터를 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <returns>DataTable 데이터</returns>
        public DataTable GetDataTable(string sqlQuery)
        {
            try
            {
                if (InjectionCodeCheck(sqlQuery) == false)
                {
                    return GetDataTable(sqlQuery, GetCommandType(false), SetSqlParameter(null), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// DataTable 데이터를 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="parameters">파라매터</param>
        /// <returns>DataTable 데이터</returns>
        public DataTable GetDataTable(string sqlQuery, Hashtable parameters)
        {
            try
            {
                if (InjectionCodeCheck(sqlQuery) == false)
                {
                    return GetDataTable(sqlQuery, GetCommandType(false), SetSqlParameter(parameters), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// DataTable 데이터를 반환합니다. (페이징 기능) 
        /// </summary>
        /// <param name="pageNo">페이지번호</param>
        /// <param name="pageSize">페이지사이즈</param>
        /// <returns>DataTable 데이터</returns>
        public DataTable GetDataTable(int pageNo, int pageSize)
        {
            try
            {
                if (InjectionCodeCheck(this.sqlQuery) == false)
                {
                    return GetDataTable(this.sqlQuery, pageNo, pageSize, GetCommandType(this.isProcedure), SetSqlParameter(this.parameters), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// DataTable 데이터를 반환합니다. (페이징 기능) 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="pageNo">페이지번호</param>
        /// <param name="pageSize">페이지사이즈</param>
        /// <returns>DataTable 데이터</returns>
        public DataTable GetDataTable(string sqlQuery, int pageNo, int pageSize)
        {
            try
            {
                if (InjectionCodeCheck(sqlQuery) == false)
                {
                    return GetDataTable(sqlQuery, pageNo, pageSize, GetCommandType(false), SetSqlParameter(null), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// DataTable 데이터를 반환합니다. (페이징 기능) 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="parameters">파라매터</param>
        /// <param name="pageNo">페이지번호</param>
        /// <param name="pageSize">페이지사이즈</param>
        /// <returns>DataTable 데이터</returns>
        public DataTable GetDataTable(string sqlQuery, Hashtable parameters, int pageNo, int pageSize)
        {
            try
            {
                if (InjectionCodeCheck(sqlQuery) == false)
                {
                    return GetDataTable(sqlQuery, pageNo, pageSize, GetCommandType(false), SetSqlParameter(parameters), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// DataRowCollection 데이터를 반환합니다. 
        /// </summary>
        /// <returns>DataRowCollection 데이터</returns>
        public DataRowCollection GetDataRowCollection()
        {
            try
            {
                if (InjectionCodeCheck(this.sqlQuery) == false)
                {
                    return GetDataRowCollection(this.sqlQuery, GetCommandType(this.isProcedure), SetSqlParameter(this.parameters), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// DataRow 데이터를 반환합니다. 
        /// </summary>
        /// <returns>DataRow 데이터</returns>
        public DataRow GetDataRow()
        {
            try
            {
                if (InjectionCodeCheck(this.sqlQuery) == false)
                {
                    return GetDataRow(this.sqlQuery, GetCommandType(this.isProcedure), SetSqlParameter(this.parameters), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// DataRow 데이터를 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <returns>DataRow 데이터</returns>
        public DataRow GetDataRow(string sqlQuery)
        {
            try
            {
                if (InjectionCodeCheck(sqlQuery) == false)
                {
                    return GetDataRow(sqlQuery, GetCommandType(false), SetSqlParameter(null), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// SqlDataReader 데이터를 반환합니다. 
        /// </summary>
        /// <returns>SqlDataReader 데이터</returns>
        public MySqlDataReader GetSqlDataReader()
        {
            try
            {
                if (InjectionCodeCheck(this.sqlQuery) == false)
                {
                    return GetSqlDataReader(this.sqlQuery, GetCommandType(this.isProcedure), SetSqlParameter(this.parameters), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// SqlDataReader 데이터를 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <returns>SqlDataReader 데이터</returns>
        public MySqlDataReader GetSqlDataReader(string sqlQuery)
        {
            try
            {
                if (InjectionCodeCheck(sqlQuery) == false)
                {
                    return GetSqlDataReader(sqlQuery, GetCommandType(false), SetSqlParameter(null), this.ConnectionString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 배열 데이터를 반환합니다.
        /// </summary>
        /// <returns>string[] 데이터</returns>
        public string[] GetArray()
        {
            try
            {
                string[] result = GetArray(this.sqlQuery, GetCommandType(this.isProcedure), SetSqlParameter(this.parameters), this.ConnectionString);

                string resultValue = string.Empty;

                //결과값 저장
                for (int i = 0; i < result.Length; i++)
                {
                    if (i == 0)
                    {
                        resultValue = result[i];
                    }
                    else
                    {
                        resultValue = resultValue + ", " + result[i];
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 단일값을 반환합니다. 
        /// </summary>
        /// <returns>단일값</returns>
        public int GetScalarInt()
        {
            try
            {
                if (InjectionCodeCheck(this.sqlQuery) == false)
                {
                    return GetScalarInt(this.sqlQuery, GetCommandType(this.isProcedure), SetSqlParameter(this.parameters), this.ConnectionString);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return 0;
            }
        }

        /// <summary>
        /// 단일값을 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="parameters">파라매터</param>
        /// <returns>단일값</returns>
        public int GetScalarInt(string sqlQuery, Hashtable parameters)
        {
            try
            {
                if (InjectionCodeCheck(sqlQuery) == false)
                {
                    return GetScalarInt(sqlQuery, GetCommandType(false), SetSqlParameter(parameters), this.ConnectionString);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return 0;
            }
        }

        /// <summary>
        /// 단일값을 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <returns>단일값</returns>
        public int GetScalarInt(string sqlQuery)
        {
            try
            {
                if (InjectionCodeCheck(sqlQuery) == false)
                {
                    return GetScalarInt(sqlQuery, GetCommandType(false), SetSqlParameter(null), this.ConnectionString);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return 0;
            }
        }

        /// <summary>
        /// 단일값을 반환합니다. 
        /// </summary>
        /// <returns>단일값</returns>
        public string GetScalarString()
        {
            try
            {
                if (InjectionCodeCheck(this.sqlQuery) == false)
                {
                    return GetScalarString(this.sqlQuery, GetCommandType(this.isProcedure), SetSqlParameter(this.parameters), this.ConnectionString);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return string.Empty;
            }
        }

        /// <summary>
        /// 단일값을 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <returns>단일값</returns>
        public string GetScalarString(string sqlQuery)
        {
            try
            {
                if (InjectionCodeCheck(sqlQuery) == false)
                {
                    return GetScalarString(sqlQuery, GetCommandType(false), SetSqlParameter(null), this.ConnectionString);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return string.Empty;
            }
        }

        /// <summary>
        /// 단일값을 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="parameters">파라매터</param>
        /// <returns>단일값</returns>
        public string GetScalarString(string sqlQuery, Hashtable parameters)
        {
            try
            {
                if (InjectionCodeCheck(sqlQuery) == false)
                {
                    return GetScalarString(sqlQuery, GetCommandType(false), SetSqlParameter(parameters), this.ConnectionString);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return string.Empty;
            }
        }

        /// <summary>
        /// 실행후 처리여부를 반환합니다. 
        /// </summary>
        /// <returns>처리여부</returns>
        public bool ExecuteSql()
        {
            try
            {
                return ExecuteSql(this.sqlQuery, GetCommandType(this.isProcedure), SetSqlParameter(this.parameters), this.ConnectionString);
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 실행후 처리여부를 반환합니다. 
        /// </summary>
        /// <param name="resultName">결과명 (identityValue, resultValue, resultValue)</param>
        /// <returns>처리여부</returns>
        public string ExecuteSql(string resultName)
        {
            try
            {
                string result = string.Empty;

                string[] resultData = GetArray(this.sqlQuery, GetCommandType(this.isProcedure), SetSqlParameter(this.parameters), this.ConnectionString);

                if (resultName == "identityValue")
                {
                    result = resultData[1].Trim();
                }
                if (resultName == "resultValue")
                {
                    result = resultData[1].Trim();
                }
                else if (resultName == "returnValue")
                {
                    result = resultData[0].Trim();
                }
                else
                {
                    result = "true";
                }

                return result;
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return string.Empty;
            }
        }


        public DataTable GetSPInfo(string spName)
        {
            return GetDataTable(
                                 string.Format(@"
                                                    SELECT ordinal_position, parameter_name, data_type 
                                                    FROM information_schema.parameters 
                                                    WHERE SPECIFIC_NAME = '{0}'
                                                    ORDER BY ordinal_position ASC
                                                "
                                                , spName
                                              )
                               );
        }

        #endregion






        #region private 함수


        /// <summary>
        /// DataSet 데이터를 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="commandType">명령타입</param>
        /// <param name="parameters">쿼리의 실행에 사용될 MySqlParameter 개체</param>
        /// <param name="connectionString">연결정보</param>
        /// <returns>DataSet데이터</returns>
        private DataSet GetDataSet(string sqlQuery, CommandType commandType, MySqlParameter[] parameters, string connectionString)
        {
            DataSet ds = new DataSet();

            try
            {
                // SqlCommand 세팅
                command = CommandSetting(sqlQuery, commandType, parameters, connectionString);

                // DataSet 내용 채우기
                (new MySqlDataAdapter(command)).Fill(ds);
            }
            finally
            {
                //연결닫기
                ConnectionClose(command);
            }

            return ds;
        }

        /// <summary>
        /// DataSet 데이터를 반환합니다. (페이징 기능) 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="pageNo">페이지번호</param>
        /// <param name="pageSize">페이지사이즈</param>
        /// <param name="commandType">명령타입</param>
        /// <param name="parameters">쿼리의 실행에 사용될 MySqlParameter 개체</param>
        /// <param name="connectionString">연결정보</param>
        /// <returns>DataSet데이터</returns>
        private DataSet GetDataSet(string sqlQuery, int pageNo, int pageSize, CommandType commandType, MySqlParameter[] parameters, string connectionString)
        {
            DataSet ds = new DataSet();

            try
            {
                sqlQuery = sqlQuery + string.Format(" limit {0}, {1}", ((pageNo - 1) * pageSize), pageSize);


                // SqlCommand 세팅
                command = CommandSetting(sqlQuery, commandType, parameters, connectionString);

                // DataSet 내용 채우기
                (new MySqlDataAdapter(command)).Fill(ds, ((pageNo - 1) * pageSize), pageSize, "PagingDataSet");
            }
            finally
            {
                //연결닫기
                ConnectionClose(command);
            }

            return ds;
        }

        /// <summary>
        /// DataTable 데이터를 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="commandType">명령타입</param>
        /// <param name="parameters">쿼리의 실행에 사용될 MySqlParameter 개체</param>
        /// <param name="connectionString">연결정보</param>
        /// <returns>DataTable데이터</returns>
        private DataTable GetDataTable(string sqlQuery, CommandType commandType, MySqlParameter[] parameters, string connectionString)
        {
            DataTable dt = new DataTable();

            try
            {
                // SqlCommand 세팅
                command = CommandSetting(sqlQuery, commandType, parameters, connectionString);

                // DataTable 내용 채우기
                (new MySqlDataAdapter(command)).Fill(dt);
            }
            finally
            {
                //연결닫기
                ConnectionClose(command);
            }

            return dt;
        }

        /// <summary>
        /// DataTable 데이터를 반환합니다. (페이징 기능) 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="pageNo">페이지번호</param>
        /// <param name="pageSize">페이지사이즈</param>
        /// <param name="commandType">명령타입</param>
        /// <param name="parameters">쿼리의 실행에 사용될 MySqlParameter 개체</param>
        /// <param name="connectionString">연결정보</param>
        /// <returns>DataTable데이터</returns>
        private DataTable GetDataTable(string sqlQuery, int pageNo, int pageSize, CommandType commandType, MySqlParameter[] parameters, string connectionString)
        {
            DataTable dt = new DataTable();

            try
            {
                sqlQuery = sqlQuery + string.Format(" limit {0}, {1}", ((pageNo - 1) * pageSize), pageSize);

                // SqlCommand 세팅
                command = CommandSetting(sqlQuery, commandType, parameters, connectionString);

                // DataTable 내용 채우기
                (new MySqlDataAdapter(command)).Fill(dt);
            }
            finally
            {
                //연결닫기
                ConnectionClose(command);
            }

            return dt;
        }

        /// <summary>
        /// DataRowCollection 데이터를 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="commandType">명령타입</param>
        /// <param name="parameters">쿼리의 실행에 사용될 MySqlParameter 개체</param>
        /// <param name="connectionString">연결정보</param>
        /// <returns>DataRowCollection데이터</returns>
        private DataRowCollection GetDataRowCollection(string sqlQuery, CommandType commandType, MySqlParameter[] parameters, string connectionString)
        {
            DataTable dt = new DataTable();
            DataRowCollection drc = null;

            try
            {
                // SqlCommand 세팅
                command = CommandSetting(sqlQuery, commandType, parameters, connectionString);

                // DataTable 내용 채우기
                (new MySqlDataAdapter(command)).Fill(dt);

                drc = dt.Rows;

                dt.Dispose();
                dt = null;
            }
            finally
            {
                //연결닫기
                ConnectionClose(command);
            }

            return drc;
        }

        /// <summary>
        /// DataRow 데이터를 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="commandType">명령타입</param>
        /// <param name="parameters">쿼리의 실행에 사용될 MySqlParameter 개체</param>
        /// <param name="connectionString">연결정보</param>
        /// <returns>DataRow데이터</returns>
        private DataRow GetDataRow(string sqlQuery, CommandType commandType, MySqlParameter[] parameters, string connectionString)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            try
            {
                // SqlCommand 세팅
                command = CommandSetting(sqlQuery, commandType, parameters, connectionString);

                // DataTable 내용 채우기
                (new MySqlDataAdapter(command)).Fill(dt);

                dr = dt.Rows[0];

                dt.Dispose();
                dt = null;
            }
            finally
            {
                //연결닫기
                ConnectionClose(command);
            }

            return dr;
        }

        /// <summary>
        /// SqlDataReader 데이터를 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="commandType">명령타입</param>
        /// <param name="parameters">쿼리의 실행에 사용될 MySqlParameter 개체</param>
        /// <param name="connectionString">연결정보</param>
        /// <returns>SqlDataReader데이터</returns>
        private MySqlDataReader GetSqlDataReader(string sqlQuery, CommandType commandType, MySqlParameter[] parameters, string connectionString)
        {
            MySqlDataReader reader = null;

            // SqlCommand 세팅
            command = CommandSetting(sqlQuery, commandType, parameters, connectionString);

            // SqlDataReader 내용 채우기
            reader = command.ExecuteReader(CommandBehavior.CloseConnection);

            return reader;
        }

        /// <summary>
        /// 배열 데이터를 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="commandType">명령타입</param>
        /// <param name="parameters">쿼리의 실행에 사용될 MySqlParameter 개체</param>
        /// <param name="connectionString">연결정보</param>
        /// <returns>배열데이터 (string)</returns>
        private string[] GetArray(string sqlQuery, CommandType commandType, MySqlParameter[] parameters, string connectionString)
        {
            DataTable dt = new DataTable();

            string[] result = null;

            try
            {
                // SqlCommand 세팅
                command = CommandSetting(sqlQuery, commandType, parameters, connectionString);

                // DataTable 내용 채우기
                (new MySqlDataAdapter(command)).Fill(dt);

                // 데이터가 있다면 배열에 담는다.
                if (dt.Rows.Count > 0)
                {
                    result = new string[dt.Columns.Count];

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        result[i] = dt.Rows[0][dt.Columns[i].ColumnName].ToString();
                    }
                }
            }
            finally
            {
                //연결닫기
                ConnectionClose(command);
            }

            return result;
        }

        /// <summary>
        /// 단일값을 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="commandType">명령타입</param>
        /// <param name="parameters">쿼리의 실행에 사용될 MySqlParameter 개체</param>
        /// <param name="connectionString">연결정보</param>
        /// <returns>단일값 (int)</returns>
        private int GetScalarInt(string sqlQuery, CommandType commandType, MySqlParameter[] parameters, string connectionString)
        {
            int result = 0;

            try
            {
                // SqlCommand 세팅
                command = CommandSetting(sqlQuery, commandType, parameters, connectionString);

                //실행(단일값리턴)
                result = Convert.ToInt32(command.ExecuteScalar());
            }
            finally
            {
                //연결닫기
                ConnectionClose(command);
            }

            return result;
        }

        /// <summary>
        /// 단일값을 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="commandType">명령타입</param>
        /// <param name="parameters">쿼리의 실행에 사용될 MySqlParameter 개체</param>
        /// <param name="connectionString">연결정보</param>
        /// <returns>단일값 (string)</returns>
        private string GetScalarString(string sqlQuery, CommandType commandType, MySqlParameter[] parameters, string connectionString)
        {
            string result = string.Empty;

            try
            {
                // SqlCommand 세팅
                command = CommandSetting(sqlQuery, commandType, parameters, connectionString);

                //실행(단일값리턴)
                result = Convert.ToString(command.ExecuteScalar());
            }
            finally
            {
                //연결닫기
                ConnectionClose(command);
            }

            return result;
        }

        /// <summary>
        /// 실행후 처리여부를 반환합니다. 
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="commandType">명령타입</param>
        /// <param name="parameters">쿼리의 실행에 사용될 MySqlParameter 개체</param>
        /// <param name="connectionString">연결정보</param>
        /// <returns>처리여부</returns>
        private bool ExecuteSql(string sqlQuery, CommandType commandType, MySqlParameter[] parameters, string connectionString)
        {
            try
            {
                // SqlCommand 세팅
                command = CommandSetting(sqlQuery, commandType, parameters, connectionString);

                //실행
                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                string log = ex.Message;
                return false;
            }
            finally
            {
                //연결닫기
                ConnectionClose(command);
            }           
        }


        /// <summary>
        /// SqlCommand 설정
        /// </summary>
        /// <param name="sqlQuery">Sql문</param>
        /// <param name="commandType">명령타입</param>
        /// <param name="parameters">쿼리의 실행에 사용될 MySqlParameter 개체</param>
        /// <param name="connectionString">연결정보</param>
        private MySqlCommand CommandSetting(string sqlQuery, CommandType commandType, MySqlParameter[] parameters, string connectionString)
        {
            //command 설정
            MySqlCommand command    = new MySqlCommand(sqlQuery, new MySqlConnection(connectionString));
            command.CommandType   = commandType;
            //command.CommandTimeout  = int.MaxValue;
            command.Connection.Open();

            //파라매터세팅
            if (parameters != null)
            {
                foreach (MySqlParameter p in parameters)
                {
                    command.Parameters.Add(p);
                }
            }

            return command;
        }


        /// <summary>
        /// 연결 닫기 
        /// </summary>
        /// <param name="command">명령객체</param>
        private void ConnectionClose(MySqlCommand command)
        {
            if (command != null)
            {
                command.Connection.Close();
                command.Dispose();
                command = null;
            }
        }


        /// <summary>
        /// CommandType을 반환합니다.
        /// </summary>
        /// <param name="isProcedure">프로시저여부</param>
        /// <returns>CommandType</returns>
        private CommandType GetCommandType(bool isProcedure)
        {
            if (isProcedure == true)
            {
                return CommandType.StoredProcedure;
            }
            else
            {
                return CommandType.Text;
            }
        }

        /// <summary>
        /// MySqlParameter[]을 설정합니다. 
        /// </summary>
        /// <param name="ht">파라매터정보</param>
        /// <returns>MySqlParameter 배열</returns>
        private MySqlParameter[] SetSqlParameter(Hashtable ht)
        {
            MySqlParameter[] parameters = null;

            int i = 0;

            if (ht != null)
            {
                if (ht.Count > 0)
                {
                    parameters = new MySqlParameter[ht.Count];

                    foreach (DictionaryEntry item in ht)
                    {
                        if (InjectionCodeCheck(item.Value.ToString()) == false)
                        {
                            parameters[i] = new MySqlParameter(item.Key.ToString(), item.Value);
                        }
                        else
                        {
                            parameters[i] = new MySqlParameter(item.Key.ToString(), "");
                        }

                        i = i + 1;
                    }
                }
            }

            return parameters;
        }


        /// <summary>
        /// 인젝션 코드가 포함되었는지 여부를 반환합니다. 
        /// </summary>
        /// <param name="data">데이터</param>
        /// <returns>SQL 인젝션 코드 여부</returns>
        private bool InjectionCodeCheck(string data)
        {
            bool result = false;

            //null이면 빈값으로 설정
            if (string.IsNullOrEmpty(data) == true)
            {
                data = string.Empty;
            }

            //향후 방어할 필요가 있는 문자가 생기면 추가...
            string[] arrCheck = new string[] {
                                                  "<script"
                                                , "[null]"
                                                , "[return]"
                                                , "[space]"
                                                , "@@variable"
                                                , "or 1=1"
                                                , "insert into"
                                                , "delete from"
                                                , "truncate"
                                                , "drop"
                                                , "xp_cmdshell"
                                                , "cmd"
                                                , "execute"
                                             };

            //내용 체크 : 방어 문자가 포함되어있다면 빈값으로 설정
            for (int i = 0; i < arrCheck.Length; i++)
            {
                if (data.ToLower().IndexOf(arrCheck[i]) > -1)
                {
                    result = true;

                    break;
                }
            }


            return result;
        }

        #endregion
    }
}
