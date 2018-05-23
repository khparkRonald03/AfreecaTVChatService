﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClientViewer
{
    public static class HtmlFormat
    {
        #region BJ

        /// <summary>
        /// 전체 HTML [파라미터 - 하위 요소]
        /// </summary>
        public static string BjHtml = @"
        <link rel='stylesheet' type='text/css' href='http://res.afreecatv.com/css/global/mybs.css' />
        <div class='fan_rank' style='height:145px;overflow:auto;'>
            <div class='tit_area' style='width:100%;'>
                <div class='title' id='fan_txt'>BJ</div>
            </div>
            <table id='sTopFanStarBalloon' style='table-layout:fixed;'>
                {0}
            </table>
        </div>
        ";

        /// <summary>
        /// 하위요소 [파라미터 - 아이디, 닉네임, 사진url]
        /// </summary>
        public static string BjHtmlChild = @"
                <tr>
                    <td style='width:100px; color: #FF0000 !important;font-weight: bold;'>{0}</td>
                    <td style='width:100px; color: #333 !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;'>{1}</td>
                    <td style='width:100px; color: #0100FF !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;'>
                        <img src='{2}' alt='' style='height:20px;' />
                    </td>
                </tr>
        ";

        #endregion

        #region 회장

        /// <summary>
        ///  - 하위요소
        /// </summary>
        public static string KingHtml = @"
        <link rel='stylesheet' type='text/css' href='http://res.afreecatv.com/css/global/mybs.css' />
        <div class='fan_rank' style='height:145px;overflow:auto;'>
            <div class='tit_area' style='width:100%;'>
                <div class='title' id='fan_txt'>회장</div>
            </div>
            <table id='sTopFanStarBalloon' style='height:100px; overflow-y:scroll;'>
                {0}
            </table>
        </div>
        ";

        /// <summary>
        /// 하위요소 [파라미터 - 아이디, 닉네임, 포함된 bj정보]
        /// </summary>
        public static string KingHtmlChild = @"
                <tr>
                    <td style='width:100px; color: #FF0000 !important;font-weight: bold;'>{0}</td>
                    <td style='width:100px; color: #333 !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;'>{1}</td>
                    <td style='width:100px; color: #0100FF !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;'>
                    {2}
                    </td>
                </tr>
        ";

        /// <summary>
        /// Bj 하위요소 [파라미터 - 아이디, 회장 아이콘Url]
        /// </summary>
        public static string KingHtmlBjChild = @"
                    <span>{0}</span>
                    <img src='{1}' alt='' />
        ";

        #endregion

        #region 열혈팬

        /// <summary>
        /// 
        /// </summary>
        public static string BigFanHtml = @"
        <link rel='stylesheet' type='text/css' href='http://res.afreecatv.com/css/global/mybs.css' />
        <div class='fan_rank' style='height:145px;overflow:auto;'>
            <div class='tit_area' style='width:100%;'>
                <div class='title' id='fan_txt'>열혈팬</div>
            </div>
            <table id='sTopFanStarBalloon' style='height:100px; overflow-y:scroll;'>
                {0}
            </table>
        </div>
        ";

        /// <summary>
        ///  - 아이디, 닉네임, BJ정보
        /// </summary>
        public static string BigFanHtmlChild = @"
            <tr>
                <td style='width:100px; color: #FF0000 !important;font-weight: bold;'>{0}</td>
                <td style='width:100px; color: #333 !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;'>{1}</td>
                <td style='width:100px; color: #0100FF !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;'>
                    {2}
                </td>
            </tr>
        ";

        /// <summary>
        /// - 닉네임, 하트 Url
        /// </summary>
        public static string BigFanHtmlBjChild = @"
                <span>{0}</span>
                <img src='{1}' alt='' />
        ";

        #endregion

        #region 채팅

        public static string ChatHtml = @"
        < link rel='stylesheet' type='text/css' href='http://res-cf.afreecatv.com/css/global/flashplayer/main.css' />
        <link rel='stylesheet' type='text/css' href='http://res.afreecatv.com/css/global/mybs.css' />
        <!--<div id='chatbox' class='chatbox' style='width:100%;'>-->
        <div class='fan_rank' style='height:360px;overflow:auto;'>
            <div class='tit_area' style='width:100%; display:block;'>
                <div class='title' id='fan_txt'>채팅</div>
            </div>
            <div id='chat_area' class='chat_area' style=''>
                <div id='chat_memoyo' class='chat_memoyo'>
                    {0}
                </div>
            </div>
        </div>
        ";

        /// <summary>
        /// 그냥 셀레니움에서 다 가져올까??
        /// </summary>
        public static string ChatHtmlChild = @"
            <dl class=''>
                <dt class='user_m'>
                    <em class='pc' style='margin-left:-30px;'></em>
                    <span>
                        <img src='http://res-cf.afreecatv.com/images/new_app/chat/ic_mobile.png' title='모바일 사용자'>
                    </span>
                    <a href='javascript:;' user_id='akvmflzk12' user_nick='한글싸랑' userflag='81920' is_mobile='true' grade='user'>한글싸랑<em>(akvmflzk12)</em></a> :
                </dt>
                <dd id='2320'>상대 못하는척 ㅈㄴ하네 ㅋㅋ</dd>
            </dl>
        ";

        #endregion
    }
}