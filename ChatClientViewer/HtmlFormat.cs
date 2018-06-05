using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClientViewer
{
    public static class HtmlFormat
    {
        public static string UserContainerHtml = @"
        <link rel='stylesheet' type='text/css' href='http://res-cf.afreecatv.com/css/global/flashplayer/main.css' />
        <link rel='stylesheet' type='text/css' href='http://res.afreecatv.com/css/global/mybs.css' />
        <div  style='height:100%;width:100%;overflow:auto;'>
            <div class='fan_rank' style='height:33%;'>
                <div class='tit_area' style='width:100%;'>
                    <div class='title' id='fan_txt'>BJ</div>
                </div>
                <div id='sTopFanStarBalloon_BJ'>
                </div>
                
            </div>
            <div class='fan_rank' style='height:33%;'>
                <div class='tit_area' style='width:100%;'>
                    <div class='title' id='fan_txt'>회장</div>
                </div>
                <div id='sTopFanStarBalloon_King'>
                </div>
            </div>
            <div class='fan_rank' style='height:33%;'>
                <div class='tit_area' style='width:100%;'>
                    <div class='title' id='fan_txt'>열혈팬</div>
                </div>
                <div id='sTopFanStarBalloon_BigFan'>
                </div>
            </div>
        </div>

        <script>
                function AddUserHtml(id, html) {
                    // DelUserHtml(id);
                    document.getElementById(id).innerHTML = html;
                }

                function DelUserHtml(id) {
                    var lo_table = document.getElementById(id);

                    for (var i=0; i < lo_table.rows.length; i++) {
                        lo_table.deleteRow(i);
                    }
                }
        </script>
        ";

        public static string UserTableHtml = @"
                    <table>
                        {0}
                    </table>";

        #region BJ

        /// <summary>
        /// 하위요소 [파라미터 - 아이디, 닉네임, 사진url]
        /// </summary>
        public static string BjHtmlChild = @"
                    <tr>
                        <td style='width:100px; color: #FF0000 !important;font-weight: bold;'>{0}</td>
                        <td style='width:100px; color: #333 !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;'>{1}</td>
                        <td style='color: #0100FF !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;'>
                            <img src='{2}' alt='' style='height:20px;' />
                        </td>
                    </tr>
        ";

        #endregion

        #region 회장

        /// <summary>
        /// 하위요소 [파라미터 - 아이디, 닉네임, 포함된 bj정보]
        /// </summary>
        public static string KingHtmlChild = @"
                    <tr>
                        <td style='width:100px; color: #FF0000 !important;font-weight: bold;'>{0}</td>
                        <td style='color: #333 !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;white-space:nowrap;'>{1}</td>
                        <td style='color: #0100FF !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;white-space:nowrap;'>
                        {2}
                        </td>
                    </tr>
        ";

        /// <summary>
        /// Bj 하위요소 [파라미터 - 닉네임, 회장 아이콘Url]
        /// </summary>
        public static string KingHtmlBjChild = @"
                        <span>{0}</span>
                        <img src='{1}' alt='' />
        ";

        #endregion

        #region 열혈팬

        /// <summary>
        ///  - 아이디, 닉네임, BJ정보
        /// </summary>
        public static string BigFanHtmlChild = @"
                <tr>
                    <td style='width:100px; color: #FF0000 !important;font-weight: bold;'>{0}</td>
                    <td style='color: #333 !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;white-space:nowrap;'>{1}</td>
                    <td style='color: #0100FF !important;letter-spacing: -1px;font-weight: bold;font-size: 11px!important;white-space: nowrap;'>
                        {2}
                    </td>
                </tr>
        ";
        // 

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
        <html>
            <head>
                <link rel='stylesheet' type='text/css' href='http://res-cf.afreecatv.com/css/global/flashplayer/main.css' />
                <link rel='stylesheet' type='text/css' href='http://res.afreecatv.com/css/global/mybs.css' />
            </head>
            <body>
                <div class='fan_rank' style='height:100%;overflow:auto;'>
                    <div class='tit_area' style='width:100%; display:block;'>
                        <div class='title' id='fan_txt'>채팅</div>
                    </div>
                    <div id='chat_area' class='chat_area' style=''>
                        <div id='chat_memoyo' class='chat_memoyo'>
                    
                        </div>
                    </div>
                </div>

                <script>
                    function AddChatHtml(html) {
                        var currentScroll = GetScrollXY();

                        document.getElementById('chat_memoyo').insertAdjacentHTML('beforeend', html);

                        //var element = document.getElementById('fan_rank');
                        //element.scrollTop = element.scrollHeight - element.clientHeight;

                        //document.getElementById('fan_rank').scrollTop = document.getElementById('fan_rank').scrollHeight;

                        var objDiv = document.getElementById('fan_rank'); 
                        objDiv.scrollTop = objDiv.scrollHeight;

                        // document.documentElement.scrollTop = currentScroll.x // X 좌표
                        // document.documentElement.scrollLeft = currentScroll.y // Y 좌표
                    } 

                    function GetScrollXY() {
                        
                        var scrOfX = 0, scrOfY = 0;
                        if( typeof( window.pageYOffset ) == 'number' ) {
                            //Netscape compliant
                            scrOfY = window.pageYOffset;
                            scrOfX = window.pageXOffset;
                        } else if( document.body && ( document.body.scrollLeft || document.body.scrollTop ) ) {
                            //DOM compliant
                            scrOfY = document.body.scrollTop;
                            scrOfX = document.body.scrollLeft;
                        } else if( document.documentElement && ( document.documentElement.scrollLeft || document.documentElement.scrollTop ) ) {
                            //IE6 standards compliant mode
                            scrOfY = document.documentElement.scrollTop;
                            scrOfX = document.documentElement.scrollLeft;
                        } 

                        return { x:scrOfX, y:scrOfY };
                    }
                </script>
            </body>
        </html>
        ";

        /// <summary>
        /// 셀레니움에서 이부분은 그냥 긁어옴
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

        /// <summary>
        /// 유저 입장
        /// ex_ <p class='notice in fanclub'><a href='javascript:;' user_id='kfiop' user_nick='무쌍.'>팬클럽 무쌍.<em>(kfiop)</em></a>님이 대화방에 참여했습니다.</p>
        /// 유저아이디, 닉네임
        /// </summary>
        public static string ChatHtmlInUser = @"
            <p class='notice in fanclub'><a href='javascript:;' user_id='{0}' user_nick='{1}'>{1}<em>({0})</em></a>님이 대화방에 참여했습니다.</p>
            ";

        /// <summary>
        /// 유저 퇴장
        /// ex_ <p class='notice in fanclub'><a href='javascript:;' user_id='kfiop' user_nick='무쌍.'>팬클럽 무쌍.<em>(kfiop)</em></a>님이 대화방에서 나가셨습니다.</p>
        /// 유저아이디, 닉네임
        /// </summary>
        public static string ChatHtmlOutUser = @"
            <p class='notice in fanclub'><a href='javascript:;' user_id='{0}' user_nick='{1}'>{1}<em>({0})</em></a>님이 대화방에서 나가셨습니다.</p>
            ";

        #endregion
    }
}
