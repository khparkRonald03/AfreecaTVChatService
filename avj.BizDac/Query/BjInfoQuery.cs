namespace avj.BizDac
{
    public class BjInfoQuery
    {
        public static string InsertAbjBjInfo = @"
            INSERT INTO `abjchat`.`abj_BjInfo`
            (`BjID`,
            `Name`,
            `Html`,
            `Text`,
            `HistoryDepth`,
            `AddDate`)
            VALUES
            (@BjID,
            @Name,
            @Html,
            @Text,
            @HistoryDepth,
            now());
            ";
    }
}
