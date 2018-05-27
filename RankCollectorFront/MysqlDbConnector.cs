using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using avj.Common;

namespace RankCollectorFront
{
    public class MysqlDbConnector : MySqlModelObject
    {
        //public bool CategoryList(int pageNo, int pageSize, Hashtable search, out int totalCount, out string queryText)
        //{
        //    sqlFilter = "";

            
        //    //sqlQuery 설정
        //    sqlQuery = @"SELECT *
        //                 FROM T_category 
        //                 WHERE 1=1 [@sqlFilter@] 
        //                 ORDER BY CategoryCode desc
        //                ";

        //    //조건 적용
        //    sqlQuery = sqlQuery.Replace("[@sqlFilter@]", sqlFilter);

        //    //쿼리 텍스트 설정
        //    queryText = GetQueryText(sqlQuery, sqlParameters);


        //    //목록 반환
        //    return sql.GetDataTable(sqlQuery, sqlParameters, pageNo, pageSize);
        //}

    }
}
