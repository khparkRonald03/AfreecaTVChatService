using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClientViewer
{
    public static class HtmlFormat
    {

        #region 시청자리스트 컨테이너

        public static string UserContainerHtml = @"
        <!doctype html>
        <html>
        <head>
            <meta charset='utf-8'>
            <script src='http://code.jquery.com/jquery-latest.min.js'></script>
            <script src='https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js'></script>
            <link rel='stylesheet' type='text/css' href='http://res-cf.afreecatv.com/css/global/flashplayer/main.css' />
            <link rel='stylesheet' type='text/css' href='http://res.afreecatv.com/css/global/mybs.css' />
            <style>
                * { margin: 0;padding: 0; }
                .pop-layer .pop-container { padding: 20px 25px; }
                .pop-layer p.ctxt { color: #666;line-height: 25px; }
                .pop-layer .btn-r { width: 100%; margin: 10px 0 20px; padding-top: 10px; border-top: 1px solid #DDD; text-align: right; }
                .pop-layer { display: none; position: absolute; top: inherit; left: 10px; width: 220px; height: auto; background-color: #fff; border: 5px solid #3571B5; z-index: 10; }
                .dim-layer { display: none;position: fixed;_position: absolute;top: 0;left: 0;width: 100%;height: 100%;z-index: 100; }
                .dim-layer .dimBg { position: absolute;top: 0;left: 0;width: 100%;height: 100%;background: #000;opacity: .5;filter: alpha(opacity=50); }
                .dim-layer .pop-layer { display: block; }
                a.btn-layerClose { display: inline-block; height: 25px; padding: 0 14px 0; border: 1px solid #304a8a; background-color: #3f5a9d; font-size: 13px; color: #fff; line-height: 25px; }
                a.btn-layerClose:hover { border: 1px solid #091940; background-color: #1f326a; color: #fff; }
            </style>
        </head>
        <body oncontextmenu='return false'onselectstart='return false'>
            <div style='height:100%;width:100%;overflow-y:auto;'>
                <div class='fan_rank' style='height:auto; border:0px;'>
                    <div class='tit_area' style='width:100%;'>
                        <div class='title' id='fan_txt'>BJ</div>
                    </div>
                    <div id='sTopFanStarBalloon_BJ'>
                        <table id='bjTable'>
                            <tr></tr>
                        </table>
                    </div>

                </div>
                <div class='fan_rank' style='height:auto; border:0px;'>
                    <div class='tit_area' style='width:100%;'>
                        <div class='title' id='fan_txt'>회장</div>
                    </div>
                    <div id='sTopFanStarBalloon_King'>
                        <table id='kingTable'>
                            <tr></tr>
                        </table>
                    </div>
                </div>
                <div class='fan_rank' style='height:auto; border:0px;'>
                    <div class='tit_area' style='width:100%;'>
                        <div class='title' id='fan_txt'>열혈팬</div>
                    </div>
                    <div id='sTopFanStarBalloon_BigFan'>
                        <table id='bigFanTable'>
                            <tr></tr>
                        </table>
                    </div>
                </div>
            </div>

            <script>

                function AddUserHtml(id, html) {

                    $('#' + id + ' tr:last').after(html);
                }

                function DelUserHtml(idStr) {

                    var ids = idStr.split('|');
                    for (var i = 0; i < ids.length; i++) {

                        $('#' + ids[i]).css('display', 'none');
                        $('#' + ids[i]).attr('id', 'del_' + $('#' + ids[i]).attr('id'));
                    }

                }

                function showPopup(tr) {

                    var $id = '#' + $(tr).attr('id') + '_layer';
                    layer_popup($id);
                }

                var beforeTag = null;

                function layer_popup(el) {
                    if (beforeTag != null) {
                        beforeTag.fadeOut();
                    }

                    var $el = $(el);                            // 레이어의 id를 $el 변수에 저장
                    var isDim = $el.prev().hasClass('dimBg');   // dimmed 레이어를 감지하기 위한 boolean 변수

                    isDim ? $('.dim-layer').fadeIn() : $el.fadeIn();

                    beforeTag = $el;
                    
                    $el.find('a.btn-layerClose').click(function (event) {
                        event.stopPropagation();
                        isDim ? $('.dim-layer').fadeOut() : $el.fadeOut(); // 닫기 버튼을 클릭하면 레이어가 닫힌다.
                    });
                }
            </script>

        </body>
        </html>
        ";

        #endregion

        #region BJ

        private static readonly string BjHtmlChild_Test = @"
        <tr height='20' id='{0}' onclick='javascript:showPopup(this)'>
            <td style='width:100px; color: #FF0000 !important;font-weight: bold;'>{0}</td>
            <td style='width:100px; color: #333 !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;'>{1}</td>
            <td style='color: #0100FF !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;'>
                <img src='{2}' alt='' style='height:20px;' />
                <div id='{0}_layer' class='pop-layer'>
                    <div class='pop-container'>
                        <div class='pop-conts'>
                            <!--content //-->
                            <ul style='' class='vlist'>
                                {3}
                            </ul>
                            <hr style='text-align:center;width:80%;border:1px;solid:#EAEAEA;border-bottom:0px;'>
                            <ul style='border-top:1px solid gray;' class='vlist'>
                                {4}
                            </ul>
                            <div class='btn-r'>
                                <a href='#' class='btn-layerClose'>닫기</a>
                            </div>
                            <!--// content-->
                        </div>
                    </div>
                </div>
            </td>
        </tr>
        ";

        /// <summary>
        /// 하위요소 [파라미터 - 아이디, 닉네임, 사진url, 팝업 display html]
        ///</summary>
        public static string BjHtmlChild = "<tr height='20' id='{0}' onclick='javascript:showPopup(this)'><td style='width:100px; color: #FF0000 !important;font-weight: bold;'>{0}</td><td style = 'width:100px; color: #333 !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;' >{1}</td><td style = 'color: #0100FF !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;' ><img src='{2}' alt='' style='height:20px;' /><div id = '{0}_layer' class='pop-layer'><div class='pop-container'><div class='pop-conts'><ul style='' class='vlist'>{3}</ul><hr style='text-align:center;width:80%;border:1px;solid:#EAEAEA;border-bottom:0px;'><ul style = 'border-top:1px solid gray;' class='vlist'>{4}</ul><div class='btn-r'><a href = '#' class='btn-layerClose'>닫기</a></div></div></div></div></td></tr>";

        /// <summary>
        /// BJ 정보 팝업 하위요소 1
        /// </summary>
        public static string BjInfoBjPopUpContents1 = "<li><span style='display:inline-block; width:70px;'>{0}:</spann><spann style='color: #FF5E00'>{1}위</spann></li>";

        /// <summary>
        /// BJ 정보 팝업 하위요소 2
        /// </summary>
        public static string BjInfoBjPopUpContents2 = "<li><span style='display:inline-block; width:70px;'>{0}</spann>:<spann style='color: #FF5E00'>{1}{2}</spann></li>";

        #endregion

        #region 회장

        private static readonly string KingHtmlChild_Test = @"
        <tr height='20' id='{0}' onclick='javascript:showPopup(this)'>
            <td style='width:100px; color: #FF0000 !important;font-weight: bold;'>{0}</td>
            <td style='width:100px; color: #333 !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;white-space:nowrap;'>{1}</td>
            <td style='color: #0100FF !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;white-space:nowrap;'>
                {2}
                <div id='{0}_layer' class='pop-layer'>
                    <div class='pop-container'>
                        <div class='pop-conts'>
                            <!--content //-->
                            <ul style='' class='vlist'>
                                {3}
                            </ul>
                            <div class='btn-r'>
                                <a href='#' class='btn-layerClose'>닫기</a>
                            </div>
                            <!--// content-->
                        </div>
                    </div>
                </div>

            </td>
        </tr>
        ";

        /// <summary>
        /// 하위요소 [파라미터 - 아이디, 닉네임, 포함된 bj정보, BJ 랭킹 팝업 콘텐츠]
        ///</summary>
        public static string KingHtmlChild = "<tr height='20' id='{0}' onclick='javascript:showPopup(this)'><td style='width:100px; color: #FF0000 !important;font-weight: bold;'>{0}</td><td style='width:100px; color: #333 !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;white-space:nowrap;'>{1}</td><td style='color: #0100FF !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;white-space:nowrap;'>{2}<div id='{0}_layer' class='pop-layer'><div class='pop-container'><div class='pop-conts'><ul style='' class='vlist'>{3}</ul><div class='btn-r'><a href='#' class='btn-layerClose'>닫기</a></div></div></div></div></td></tr>";

        /// <summary>
        /// Bj 하위요소 [파라미터 - 닉네임, 회장 아이콘Url]
        ///</summary>
        public static string KingHtmlBjChild = "<span>{0}</span><img src='{1}' alt='' />";

        #endregion

        #region 열혈팬

        /// <summary>
        /// 팝업 - Bj 하위요소 [파라미터 - 순위, 닉네임, 회장 아이콘Url]
        ///</summary>
        //public static string BjPopUpContents = "<tr><td width='40px'>{0}위</td><td>{1}</td><td><img src='{2}' alt='' /></td></tr>";
        public static string UserBjPopUpContents = "<li><span width='40px'>{0}위 </span><span>{1}</span><em><img src='{2}' alt='' /></em></li>";

        private static readonly string BigFanHtmlChild_Test = @"
        <tr height='20' id='{0}' onclick='javascript:showPopup(this);'>
            <td style='width:100px; color: #FF0000 !important;font-weight: bold;'>{0}</td>
            <td style='width:100px; color: #333 !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;white-space:nowrap;'>{1}</td>
            <td style='color: #0100FF !important;letter-spacing: -1px;font-weight: bold;font-size: 11px!important;white-space: nowrap;'>
                {2}
                <div id='{0}_layer' class='pop-layer'>
                    <div class='pop-container'>
                        <div class='pop-conts'>
                            <!--content //-->
                            <ul style='' class='vlist'>
                                {3}
                            </ul>
                            <div class='btn-r'>
                                <a href='#' class='btn-layerClose'>닫기</a>
                            </div>
                            <!--// content-->
                        </div>
                    </div>
                </div>
            </td>
        </tr>
        ";

        /// <summary>
        ///  - 아이디, 닉네임, BJ정보, BJ 랭킹 팝업 콘텐츠
        ///</summary>
        public static string BigFanHtmlChild = "<tr height='20' id='{0}' onclick='javascript:showPopup(this);'><td style='width:100px; color: #FF0000 !important;font-weight: bold;'>{0}</td><td style='width:100px; color: #333 !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;white-space:nowrap;'>{1}</td><td style='color: #0100FF !important;letter-spacing: -1px;font-weight: bold;font-size: 11px!important;white-space: nowrap;'>{2}<div id='{0}_layer' class='pop-layer'><div class='pop-container'><div class='pop-conts'><ul style='' class='vlist'>{3}</ul><div class='btn-r'><a href='#' class='btn-layerClose'>닫기</a></div></div></div></div></td></tr>";

        /// <summary>
        /// - 닉네임, 하트 Url
        ///</summary>
        public static string BigFanHtmlBjChild = "<span>{0}</span><img src='{1}' alt='' />";

        #endregion

        #region 채팅

        public static string ChatHtml = @"
        <!doctype html>
        <html>
        <head>
            <meta charset='utf-8'>
            <script src='http://code.jquery.com/jquery-latest.min.js'></script>
            <script src='https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js'></script>
            <link rel='stylesheet' type='text/css' href='http://res-cf.afreecatv.com/css/global/flashplayer/main.css' />
            <link rel='stylesheet' type='text/css' href='http://res.afreecatv.com/css/global/mybs.css' />
        </head>
        <body oncontextmenu='return false'onselectstart='return false'>

                <div id='fan_rank' class='fan_rank' style='height:100%;overflow:auto; margin-top:15px;'>
                <div class='tit_area' style='width:100%; display:block;'>
                    <div class='title' id='fan_txt'>채팅</div>
                </div>
                <div id='chat_area' class='chat_area' style='margin-top:90px'>
                    <div id='chat_memoyo' class='chat_memoyo'>

                    </div>
                </div>
            </div>
            <div id='chat_scroll_down' class='chat_scroll_down on' style='float:none' onclick='ClickChatScrollDown()'>
                <button type='button' style='margin-top:100px'>채팅 아래로 스크롤</button>
            </div>

            <script>
                $(function () {
                    $('#fan_rank').bind('scroll', UserMoveScroll);
                    $('#chat_scroll_down').hide();
                });

                function UserMoveScroll() {
                    if ($('#chat_scroll_down').css('display') == 'none')
                        $('#chat_scroll_down').show();
                }

                function ClickChatScrollDown() {
                    
                    document.getElementById('chat_area').scrollIntoView(false);
                    setTimeout(function () { $('#chat_scroll_down').hide(); }, 200);
                }

                function AddChatHtml(html) {

                    document.getElementById('chat_memoyo').insertAdjacentHTML('beforeend', html);

                    if ($('#chat_scroll_down').css('display') != 'block') {

                        $('#fan_rank').unbind('scroll');
                        document.getElementById('chat_area').scrollIntoView(false);
                        setTimeout(function () { $('#fan_rank').bind('scroll', UserMoveScroll); }, 1000);
                    }
                    
                }

            </script>
        </body>
        </html>
        ";

        /// <summary>
        /// 셀레니움에서 이부분은 그냥 긁어옴
        ///</summary>
        private static readonly string ChatHtmlChild = @"
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
        ///</summary>
        public static string ChatHtmlInUser = "<p class='notice in fanclub'><a user_id='{0}' user_nick='{1}'>{1}<em>({0})</em></a>님이 대화방에 참여했습니다.</p>";

        /// <summary>
        /// 유저 퇴장
        /// ex_ <p class='notice in fanclub'><a href='javascript:;' user_id='kfiop' user_nick='무쌍.'>팬클럽 무쌍.<em>(kfiop)</em></a>님이 대화방에서 나가셨습니다.</p>
        /// 유저아이디, 닉네임
        ///</summary>
        public static string ChatHtmlOutUser = "<p class='notice out fanclub' style='color:red !important'><a style='color:red !important' user_id='{0}' user_nick='{1}'>{1}<em>({0})</em></a>님이 대화방에서 나가셨습니다.</p>";

        #endregion
    }
}
