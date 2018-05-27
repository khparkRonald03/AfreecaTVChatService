using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace avj.Common
{
    public class MySql_
    {
        public MySql_()
        {
            //
            // TODO: 여기에 생성자 논리를 추가합니다.
            //
        }

        private string myConnectionString = ConfigurationManager.AppSettings["MySqlConn"].ToString();
        //private string myConnectionString = "server=localhost;port=3306;Database=bratvillage;User Id=bratvillage_user;Password=p@ssw0rd;allow user variables=true;";
        public int nCommandTime = 0;

        #region 파라미터
        /// <summary>
        /// 파라미터 모음
        /// </summary>
        public List<MySqlParameter> SqlParams = new List<MySqlParameter>();

        //internal MySqlParameter AddParam(string v, MySqlDbType varChar, ParameterDirection input, object asset_id)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// 파라미터추가
        /// </summary>
        /// <param name="parametername"></param>
        /// <param name="_dbType"></param>
        /// <param name="iSize"></param>
        /// <param name="pmDirection"></param>
        /// <param name="pmValue"></param>
        /// <returns></returns>
        public MySqlParameter AddParam(string parametername, MySqlDbType _dbType, int iSize, ParameterDirection pmDirection, object pmValue)
        {
            MySqlParameter pm = new MySqlParameter
            {
                ParameterName = parametername,
                MySqlDbType = _dbType,
                Size = iSize,
                Direction = pmDirection,
                Value = pmValue
            };

            return pm;
        }

        /// <summary>
        /// 파라미터추가
        /// </summary>
        /// <param name="parametername"></param>
        /// <param name="_dbType"></param>
        /// <param name="pmDirection"></param>
        /// <param name="pmValue"></param>
        /// <returns></returns>
        public MySqlParameter AddParam(string parametername, MySqlDbType _dbType, ParameterDirection pmDirection, object pmValue)
        {
            MySqlParameter pm = new MySqlParameter
            {
                ParameterName = parametername,
                MySqlDbType = _dbType,
                Direction = pmDirection,
                Value = pmValue
            };

            return pm;
        }
        /// <summary>
        /// 파라미터추가
        /// </summary>
        /// <param name="parametername"></param>
        /// <param name="_dbType"></param>
        /// <param name="pmDirection"></param>
        /// <param name="pmValue"></param>
        /// <returns></returns>
        public MySqlParameter AddParam(string parametername, MySqlDbType _dbType, ParameterDirection pmDirection)
        {
            MySqlParameter pm = new MySqlParameter
            {
                ParameterName = parametername,
                MySqlDbType = _dbType,
                Direction = pmDirection
            };

            return pm;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int GetScalarInt(string query)
        {
            MySqlConnection conn;
            conn = new MySqlConnection
            {
                ConnectionString = myConnectionString
            };

            MySqlCommand com = conn.CreateCommand();

            int result = 0;
            try
            {
                conn.Open();
                com.CommandText = query;
                com.CommandTimeout = 3600;

                result = Convert.ToInt32(com.ExecuteScalar());

                conn.Close();
            }
            finally
            {
                if (com != null)
                {
                    com.Connection.Close();
                    com.Dispose();
                    com = null;
                }
            }
            return result;
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query)
        {
            MySqlConnection conn;

            int result = 0;

            try
            {
                conn = new MySqlConnection
                {
                    ConnectionString = myConnectionString
                };

                MySqlCommand com = conn.CreateCommand();
                conn.Open();
                com.CommandText = query;
                com.CommandTimeout = 3600;

                result = com.ExecuteNonQuery();

                conn.Close();

                return result;
            }
            catch (MySqlException ex)
            {
                string log = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// ExecuteNonQuery (Procedure)
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="pc"></param>
        public void ExecuteNonQuery(string procedure, List<MySqlParameter> pc)
        {
            MySqlConnection conn;

            try
            {
                conn = new MySqlConnection
                {
                    ConnectionString = myConnectionString
                };

                MySqlCommand com = conn.CreateCommand();

                com.CommandText = procedure;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 3600;

                if (pc != null)
                {
                    //인풋 파라미터 설정
                    for (int i = 0; i < pc.Count; i++)
                    {
                        if (pc[i].Direction == ParameterDirection.Input)
                            com.Parameters.Add(pc[i]);
                        else if (pc[i].Direction == ParameterDirection.Output)
                        {
                            com.Parameters.Add(
                                new MySqlParameter(
                                    pc[i].ParameterName,
                                    pc[i].MySqlDbType,
                                    this.GetDbTypeSize(pc[i].DbType),
                                    ParameterDirection.Output,
                                    true,
                                    0,
                                    0,
                                    string.Empty,
                                    DataRowVersion.Default,
                                    DBNull.Value)
                                    );
                        }
                    }
                }

                conn.Open();

                com.ExecuteNonQuery();

                conn.Close();

            }
            catch (MySqlException ex)
            {
                string log = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// 데이터 셋 가져오기
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string query)
        {
            DataSet ds = new DataSet();
            MySqlConnection conn;
            MySqlCommand cmd;
            MySqlDataAdapter myAdapter;

            conn = new MySqlConnection();
            cmd = new MySqlCommand();
            myAdapter = new MySqlDataAdapter();

            conn.ConnectionString = myConnectionString;

            try
            {
                cmd.CommandText = query;
                cmd.Connection = conn;

                myAdapter.SelectCommand = cmd;
                myAdapter.Fill(ds);

                return ds;
            }

            catch (MySqlException ex)
            {
                string log = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// 데이터 셋 가져오기
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string procedure, List<MySqlParameter> pc)
        {
            DataSet ds = new DataSet();
            MySqlConnection conn;
            MySqlCommand cmd;
            MySqlDataAdapter myAdapter;

            conn = new MySqlConnection();
            cmd = new MySqlCommand();
            myAdapter = new MySqlDataAdapter();

            conn.ConnectionString = myConnectionString;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.CommandText = procedure;

            for (int i = 0; i < pc.Count; i++)
            {
                cmd.Parameters.Add(pc[i]);
            }

            myAdapter.SelectCommand = cmd;
            myAdapter.Fill(ds);

            return ds;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string sqlQuery, int pageNo, int pageSize)
        {
            DataSet ds = new DataSet();

            MySqlConnection conn;
            MySqlCommand cmd;
            MySqlDataAdapter myAdapter;

            conn = new MySqlConnection();
            cmd = new MySqlCommand();
            myAdapter = new MySqlDataAdapter();

            try
            {

                if (sqlQuery.Substring(0, 6) == "SELECT")
                {
                    //sqlQuery = string.Format("SELECT TOP {0} ", pageNo * pageSize) + sqlQuery.Substring(7, sqlQuery.Length - 7);
                    sqlQuery = "SELECT " + sqlQuery.Substring(7, sqlQuery.Length - 7) + string.Format(" LIMIT {0}; ", pageNo * pageSize);
                }


                conn.ConnectionString = myConnectionString;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                cmd.CommandText = sqlQuery;
                myAdapter.SelectCommand = cmd;

                myAdapter.Fill(ds, (pageNo - 1) * pageSize, pageSize, "Table");
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                    cmd = null;
                }
            }
            return ds;
        }

        #region GetDbTypeSize

        /// <summary>
        /// GetDbTypeSize
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// <remarks>XML관련 리턴타입추가요망</remarks>
        public int GetDbTypeSize(DbType dt)
        {
            switch (dt)
            {
                case DbType.Int32:
                    return 4;

                case DbType.Byte:
                    return 1;

                case DbType.Int16:
                    return 2;

                case DbType.Decimal:
                    return 8;

                case DbType.Double:
                    return 8;

                case DbType.DateTime:
                    return 4;
                case DbType.String:
                    return 255;
            }
            return 0;
        }

        #endregion
    }
}