using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataModels
{
    [Serializable]
    public class RankBjModel
    {
        /// <summary>
        /// BJ 아이디
        /// </summary>
        [DisplayName("")]
        public string BjID { get; set; }

        /// <summary>
        /// BJ 닉네임
        /// </summary>
        [DisplayName("")]
        public string BjNick { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("")]
        public int? HistoryDepth { get; set; }

        /// <summary>
        /// BJ 사진 url
        /// </summary>
        [DisplayName("")]
        public string BjImgUrl { get; set; }

        /// <summary>
        /// 아프리카 신입bj 랭킹
        /// </summary>
        [DisplayName("신입bj 랭킹")]
        public int RookieRanking { get; set; }

        /// <summary>
        /// 최고시청자 평균
        /// </summary>
        [DisplayName("")]
        public int ViewerRanking { get; set; }

        /// <summary>
        /// 애청자 증가수
        /// </summary>
        [DisplayName("애청자 증가수")]
        public int FavoriteupRanking { get; set; }

        /// <summary>
        /// UP 수
        /// </summary>
        [DisplayName("UP 수")]
        public int UpRanking { get; set; }

        /// <summary>
        /// 모바일
        /// </summary>
        [DisplayName("")]
        public int MobileRanking { get; set; }

        /// <summary>
        /// 게임
        /// </summary>
        [DisplayName("")]
        public int GameRanking { get; set; }

        /// <summary>
        /// 모바일게임
        /// </summary>
        [DisplayName("")]
        public int MobilegameRanking { get; set; }

        /// <summary>
        /// 스포츠중계
        /// </summary>
        [DisplayName("")]
        public int SportsBroadcastRanking { get; set; }

        /// <summary>
        /// 스포츠일반
        /// </summary>
        [DisplayName("")]
        public int SportsGeneralRanking { get; set; }

        /// <summary>
        /// 토크/캠방
        /// </summary>
        [DisplayName("토크/캠방")]
        public int TalkcamRanking { get; set; }

        /// <summary>
        /// 먹방/쿡방
        /// </summary>
        [DisplayName("먹방/쿡방")]
        public int MukbangRanking { get; set; }

        /// <summary>
        /// 음악
        /// </summary>
        [DisplayName("")]
        public int MusicRanking { get; set; }

        /// <summary>
        /// 펫방
        /// </summary>
        [DisplayName("")]
        public int PetRanking { get; set; }

        /// <summary>
        /// 취미
        /// </summary>
        [DisplayName("")]
        public int HobbyRanking { get; set; }

        /// <summary>
        /// 학습
        /// </summary>
        [DisplayName("")]
        public int StudyRanking { get; set; }

        /// <summary>
        /// 더빙/라디오
        /// </summary>
        [DisplayName("")]
        public int DubradioRanking { get; set; }

        /// <summary>
        /// 주식/금융
        /// </summary>
        [DisplayName("")]
        public int StockRanking { get; set; }

        /// <summary>
        /// 엔터테인먼트
        /// </summary>
        [DisplayName("")]
        public int EnterRanking { get; set; }

        /// <summary>
        /// 동영상
        /// </summary>
        [DisplayName("")]
        public int VideosRanking { get; set; }

        /// <summary>
        /// 누적 애청자 수
        /// </summary>
        [DisplayName("")]
        public int FavoriteRanking { get; set; }

        /// <summary>
        /// 누적 팬클럽 수
        /// </summary>
        [DisplayName("")]
        public int FanclubRanking { get; set; }

        /// <summary>
        /// 값 유효성 여부
        /// </summary>
        [DisplayName("")]
        public string Valid { get; set; }

        /// <summary>
        /// 추가된 날짜
        /// </summary>
        [DisplayName("")]
        public DateTime? AddDate { get; set; }

        /// <summary>
        /// Bj 정보
        /// </summary>
        [DisplayName("")]
        public List<BjInfoModel> Bjinfo { get; set; }
    }
}
