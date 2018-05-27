using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace avj.Common
{
    public class DacCoreSqlServer
    {
        //private string SqlServerConnectionString = ConfigurationManager.AppSettings["SqlServerConn"].ToString();
        private readonly string SqlServerConnectionString = "Server=ronald03s-mssql-01.cfav9hfzfs7y.ap-northeast-2.rds.amazonaws.com;Database=Ronald03s;Persist Security Info=True;User ID=ronald03;Password=ky850224";

        public Tresult GetDataModel<Tresult>(string procedure, List<DacSqlServerParam> dacParams)
        {
            var ds = GetDataSet(procedure, dacParams);
            return GetDataToModel<Tresult>(ds);
        }

        /// <summary>
        /// 데이터 셋 가져오기
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string procedure, List<DacSqlServerParam> dacParams)
        {
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

        private List<SqlParameter> TransferParamsType(List<DacSqlServerParam> dacParams)
        {
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
