using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avj.BizDac
{
    public class BizG5_member
    {
        DacG5_member Dac { get; set; }

        public BizG5_member()
        {
            Dac = new DacG5_member();
        }

        public List<G5_memberModel> GetAllG5_memberModels()
        {
            string query = G5_memberQuery.SelectAllG5_member;

            var result = Dac.GetAllG5_memberModels(query);
            return result;
        }
    }
}
