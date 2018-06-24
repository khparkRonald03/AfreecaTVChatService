namespace avj.BizDac
{
    public class RankCollectorSettingsQuery
    {
        #region 랭킹 수집

        public static string SelectSettings = @"
            SELECT *
              FROM abjchat.abj_RankCollectorSettings
             WHERE Idx = 1;
        ";
        #endregion

        #region 랭킹 매칭

        public static string UpdateLastActionDate = @"
            UPDATE `abjchat`.`abj_RankCollectorSettings`
               SET `LastActionDate` = now(),
                   `LastHistoryDepth` = LastHistoryDepth + 1
             WHERE `Idx` = 1;
            ";

        #endregion
    }
}
