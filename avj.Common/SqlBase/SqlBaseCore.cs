using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace avj.Common
{
    public class DacCore
    {

        private readonly string myConnectionString = ConfigurationManager.AppSettings["MySqlConn"].ToString();
        public int nCommandTime = 0;

        #region 파라미터
        
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
        public void ExecuteNonQuery(string procedure, List<DacMysqlParam> dacParams)
        {
            MySqlConnection conn;

            try
            {
                var pc = TransferParamsType(dacParams);

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

        public Tresult GetDataModel<Tresult>(string query)
        {
            var ds = GetDataSet(query);
            return GetDataToModel<Tresult>(ds);
        }

        /// <summary>
        /// 데이터 셋 가져오기
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string procedure, List<DacMysqlParam> dacParams)
        {
            var pc = TransferParamsType(dacParams);

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

        public Tresult GetDataModel<Tresult>(string procedure, List<DacMysqlParam> dacParams)
        {
            var ds = GetDataSet(procedure, dacParams);
            return GetDataToModel<Tresult>(ds);
        }

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

        public Tresult GetDataModel<Tresult>(string sqlQuery, int pageNo, int pageSize)
        {
            var ds = GetDataSet(sqlQuery, pageNo, pageSize);
            return GetDataToModel<Tresult>(ds);
        }

        private Tresult GetDataToModel<Tresult>(DataSet ds)
        {
            var obj = Activator.CreateInstance<Tresult>();
            obj = (Tresult)ds.Tables[0].ToDataObject(typeof(Tresult));
            return obj;
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

        private List<MySqlParameter> TransferParamsType(List<DacMysqlParam> dacParams)
        {
            var mySqlParams = new List<MySqlParameter>();
            foreach(var dacParam in dacParams)
            {
                mySqlParams.Add(AddParam(dacParam.ParamName, (MySqlDbType)dacParam.DbType, ParameterDirection.Input, dacParam.ParamValue));
            }

            return mySqlParams;
        }
    }
}
