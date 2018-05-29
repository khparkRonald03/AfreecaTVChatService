using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avj.BizDac
{
    public class RankCollectorSettingsQuery
    {
        #region 랭킹 수집

        public static string SelectSettings = @"SELECT Idx
                                              , LastHistoryDepth 
                                           FROM abjchat.RankCollectorSettings 
                                          WHERE Idx = 1;
                                        ";
        #endregion
    }
}
