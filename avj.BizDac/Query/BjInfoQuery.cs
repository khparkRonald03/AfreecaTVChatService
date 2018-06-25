namespace avj.BizDac
{
    public class BjInfoQuery
    {
        public static string UpdateInitBjInfoValues = @"
            SET SQL_SAFE_UPDATES = 0; 
            UPDATE `abjchat`.`abj_BjInfo` SET `Valid` = 'N';
        ";

        public static string InsertAbjBjInfo = @"
            INSERT INTO `abjchat`.`abj_BjInfo`
            (`BjID`,
            `Name`,
            `Count`,
            `Unit`,
            `HistoryDepth`,
            `AddDate`)
            VALUES
            (@BjID,
            @Name,
            @Count,
            @Unit,
            @HistoryDepth,
            now());
            ";

        public static string SelectAllValidYBjInfo = @"
            SELECT * 
              FROM abjchat.abj_BjInfo
              WHERE Valid = 'Y';
        ";

        public static string SelectAllValidYBjInfoByBjID = @"
            SELECT * 
              FROM abjchat.abj_BjInfo
              WHERE Valid = 'Y'
                AND BjID = '{0}';
        ";
    }
}
