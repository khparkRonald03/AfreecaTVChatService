using System;

namespace DataModels
{
    [Serializable]
    public class RankBjModel
    {
        /// <summary>
        /// BJ 아이디
        /// </summary>
        public string BjID { get; set; }

        /// <summary>
        /// BJ 닉네임
        /// </summary>
        public string BjNick { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? HistoryDepth { get; set; }

        /// <summary>
        /// BJ 사진 url
        /// </summary>
        public string BjImgUrl { get; set; }

        /// <summary>
        /// 아프리카 신입bj 랭킹
        /// </summary>
        public int RookieRanking { get; set; }

        /// <summary>
        /// 최고시청자 평균
        /// </summary>
        public int ViewerRanking { get; set; }

        /// <summary>
        /// 애청자 증가수
        /// </summary>
        public int FavoriteupRanking { get; set; }

        /// <summary>
        /// UP 수
        /// </summary>
        public int UpRanking { get; set; }

        /// <summary>
        /// 모바일
        /// </summary>
        public int MobileRanking { get; set; }

        /// <summary>
        /// 게임
        /// </summary>
        public int GameRanking { get; set; }

        /// <summary>
        /// 모바일게임
        /// </summary>
        public int MobilegameRanking { get; set; }

        /// <summary>
        /// 스포츠중계
        /// </summary>
        public int SportsBroadcastRanking { get; set; }

        /// <summary>
        /// 스포츠일반
        /// </summary>
        public int SportsGeneralRanking { get; set; }

        /// <summary>
        /// 토크/캠방
        /// </summary>
        public int TalkcamRanking { get; set; }

        /// <summary>
        /// 먹방/쿡방
        /// </summary>
        public int MukbangRanking { get; set; }

        /// <summary>
        /// 음악
        /// </summary>
        public int MusicRanking { get; set; }

        /// <summary>
        /// 펫방
        /// </summary>
        public int PetRanking { get; set; }

        /// <summary>
        /// 취미
        /// </summary>
        public int HobbyRanking { get; set; }

        /// <summary>
        /// 학습
        /// </summary>
        public int StudyRanking { get; set; }

        /// <summary>
        /// 더빙/라디오
        /// </summary>
        public int DubradioRanking { get; set; }

        /// <summary>
        /// 주식/금융
        /// </summary>
        public int StockRanking { get; set; }

        /// <summary>
        /// 엔터테인먼트
        /// </summary>
        public int EnterRanking { get; set; }

        /// <summary>
        /// 동영상
        /// </summary>
        public int VideosRanking { get; set; }

        /// <summary>
        /// 누적 애청자 수
        /// </summary>
        public int FavoriteRanking { get; set; }

        /// <summary>
        /// 누적 팬클럽 수
        /// </summary>
        public int FanclubRanking { get; set; }

        /// <summary>
        /// 값 유효성 여부
        /// </summary>
        public string Valid { get; set; }

        /// <summary>
        /// 추가된 날짜
        /// </summary>
        public DateTime? AddDate { get; set; }
    }
}
