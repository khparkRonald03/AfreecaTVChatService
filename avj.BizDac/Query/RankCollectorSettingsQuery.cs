using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avj.BizDac
{
    public class RankCollectorSettingsQuery
    {
        public static string SelectSettings = @"SELECT Idx
                                              , LastHistoryDepth 
                                           FROM abjchat.RankCollectorSettings 
                                          WHERE Idx = 1;
                                        ";
    }
}
