using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankCollectorFront
{
    public class Query
    {
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
    }
}
