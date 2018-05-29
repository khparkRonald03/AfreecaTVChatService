﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avj.BizDac
{
    public class BjRankQuery
    {
        #region 랭킹 수집

        public static string UpdateInitBjValues = @"
            SET SQL_SAFE_UPDATES = 0; 
               UPDATE `abjchat`.`abj_BjRank` SET `Valid` = 'N';
                ";

        public static string InsertAbjBjRank = @"
        INSERT INTO `abjchat`.`abj_BjRank`
	    (`BjID`,
	    `HistoryDepth`,
	    `BjNick`,
	    `BjImgUrl`,
	    `RookieRanking`,
	    `ViewerRanking`,
	    `FavoriteupRanking`,
	    `UpRanking`,
	    `MobileRanking`,
	    `GameRanking`,
	    `MobilegameRanking`,
	    `SportsBroadcastRanking`,
	    `SportsGeneralRanking`,
	    `TalkcamRanking`,
	    `MukbangRanking`,
	    `MusicRanking`,
	    `PetRanking`,
	    `HobbyRanking`,
	    `StudyRanking`,
	    `DubradioRanking`,
	    `StockRanking`,
	    `EnterRanking`,
	    `VideosRanking`,
	    `FavoriteRanking`,
	    `FanclubRanking`,
	    `AddDate`)
	    VALUES
	    (`@p_BjID`,
	    `@p_HistoryDepth`,
	    `@p_BjNick`,
	    `@p_BjImgUrl`,
	    `@p_RookieRanking`,
	    `@p_ViewerRanking`,
	    `@p_FavoriteupRanking`,
	    `@p_UpRanking`,
	    `@p_MobileRanking`,
	    `@p_GameRanking`,
	    `@p_MobilegameRanking`,
	    `@p_SportsBroadcastRanking`,
	    `@p_SportsGeneralRanking`,
	    `@p_TalkcamRanking`,
	    `@p_MukbangRanking`,
	    `@p_MusicRanking`,
	    `@p_PetRanking`,
	    `@p_HobbyRanking`,
	    `@p_StudyRanking`,
	    `@p_DubradioRanking`,
	    `@p_StockRanking`,
	    `@p_EnterRanking`,
	    `@p_VideosRanking`,
	    `@p_FavoriteRanking`,
	    `@p_FanclubRanking`,
	    NOW());
        ";

        #endregion

        public static string SelectAllValidYBjRank = @"
            SELECT * 
              FROM abjchat.abj_BjRank
              WHERE Valid = 'Y';
        ";
    }
}
