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
        `BjNic`,
        `HistoryDepth`,
        `UserID`,
        `UserNick`,
        `BigFanRanking`,
        `SupportRanking`,
        `AddDate`)
        VALUES
        (@BjID,
        @BjNic,
        @HistoryDepth,
        @UserID,
        @UserNick,
        @BigFanRanking,
        @SupportRanking,
        NOW());
        ";

        #endregion

        #region 랭킹 매칭

        public static string SelectAllValidYUserRank = @"
            SELECT * 
              FROM abjchat.abj_UserRank
             WHERE Valid = 'Y'
               AND BigFanRanking != 0;
        ";

        public static string SelectAllValidYUserRankByUserID = @"
            SELECT * 
              FROM abjchat.abj_UserRank
             WHERE Valid = 'Y'
               AND BigFanRanking != 0
               AND UserID = '{0}';
        ";

        public static string SelectFirstCharAndValidYUserRank = @"
            SELECT *
              FROM abjchat.abj_UserRank 
             WHERE valid = 'Y'
               AND BigFanRanking != 0
               AND LEFT(userid, 1) = '{0}';
        ";

        public static string SelectFirstCharList = @"
            SELECT LEFT(userid, 1) as FirstChar
              FROM abjchat.abj_UserRank 
             WHERE valid = 'Y'
               AND LEFT(userid, 1) != ''
             GROUP BY LEFT(userid, 1);
        ";

        #endregion
    }
}
