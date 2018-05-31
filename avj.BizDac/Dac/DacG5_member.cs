using avj.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels;

namespace avj.BizDac
{
    public class DacG5_member : MySql
    {
        public List<G5_memberModel> GetAllG5_memberModels(string query)
        {
            var result = GetDataModel<List<G5_memberModel>>(query);
            return result;
        }
    }
}
