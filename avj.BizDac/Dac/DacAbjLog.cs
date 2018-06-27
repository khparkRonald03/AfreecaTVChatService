using avj.Common;
using System.Collections.Generic;

namespace avj.BizDac
{
    public class DacAbjLog : MySql
    {
        public void SetApiLog(string query, string log)
        {
            var mysqlParams = new List<MysqlParam>
            {
                // Log
                new MysqlParam("@log", MysqlDbType.VarChar, log)
            };

            ExecuteNonQuery(query, mysqlParams, System.Data.CommandType.Text);
        }
    }
}
