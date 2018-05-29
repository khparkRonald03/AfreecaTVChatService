using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avj.BizDac
{
    public class RankUserQuery
    {
        #region 랭킹 수집

        public static string UpdateInitUserValues = @"
            SET SQL_SAFE_UPDATES = 0; 
               UPDATE `abjchat`.`abj_UserRank` SET `Valid` = 'N';
                ";

        public static string InsertAbjUserRank = @"
        INSERT INTO `abjchat`.`abj_UserRank`
        (`BjID`,
        `HistoryDepth`,
        `UserID`,
        `UserNick`,
        `BigFanRanking`,
        `SupportRanking`,
        `AddDate`)
        VALUES
        (@BjID,
        @HistoryDepth,
        @UserID,
        @UserNick,
        @BigFanRanking,
        @SupportRanking,
        NOW());
        ";

        #endregion

        public static string SelectAllValidYUserRank = @"
            SELECT * 
              FROM abjchat.abj_UserRank
             WHERE Valid = 'Y';
        ";
    }
}
