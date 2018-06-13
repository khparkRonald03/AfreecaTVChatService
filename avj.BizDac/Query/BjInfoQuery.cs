﻿namespace avj.BizDac
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