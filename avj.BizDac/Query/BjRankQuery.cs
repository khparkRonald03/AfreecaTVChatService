using System;
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
	    (`@BjID`,
	    `@HistoryDepth`,
	    `@BjNick`,
	    `@BjImgUrl`,
	    `@RookieRanking`,
	    `@ViewerRanking`,
	    `@FavoriteupRanking`,
	    `@UpRanking`,
	    `@MobileRanking`,
	    `@GameRanking`,
	    `@MobilegameRanking`,
	    `@SportsBroadcastRanking`,
	    `@SportsGeneralRanking`,
	    `@TalkcamRanking`,
	    `@MukbangRanking`,
	    `@MusicRanking`,
	    `@PetRanking`,
	    `@HobbyRanking`,
	    `@StudyRanking`,
	    `@DubradioRanking`,
	    `@StockRanking`,
	    `@EnterRanking`,
	    `@VideosRanking`,
	    `@FavoriteRanking`,
	    `@FanclubRanking`,
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
