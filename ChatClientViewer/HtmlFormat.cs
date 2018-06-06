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
        <!doctype html>
        <html>
        <head>
            <meta charset='utf-8'>
            <script src='http://code.jquery.com/jquery-latest.min.js'></script>

            <link rel='stylesheet' type='text/css' href='http://res-cf.afreecatv.com/css/global/flashplayer/main.css' />
            <link rel='stylesheet' type='text/css' href='http://res.afreecatv.com/css/global/mybs.css' />
            <style>
                * { margin: 0;padding: 0; }
                .pop-layer .pop-container { padding: 20px 25px; }
                .pop-layer p.ctxt { color: #666;line-height: 25px; }
                .pop-layer .btn-r { width: 100%; margin: 10px 0 20px; padding-top: 10px; border-top: 1px solid #DDD; text-align: right; }
                .pop-layer { display: none; position: absolute; top: 50%; left: 50%; width: 60%; height: auto; background-color: #fff; border: 5px solid #3571B5; z-index: 10; }
                .dim-layer { display: none;position: fixed;_position: absolute;top: 0;left: 0;width: 100%;height: 100%;z-index: 100; }
                .dim-layer .dimBg { position: absolute;top: 0;left: 0;width: 100%;height: 100%;background: #000;opacity: .5;filter: alpha(opacity=50); }
                .dim-layer .pop-layer { display: block; }
                a.btn-layerClose { display: inline-block; height: 25px; padding: 0 14px 0; border: 1px solid #304a8a; background-color: #3f5a9d; font-size: 13px; color: #fff; line-height: 25px; }
                a.btn-layerClose:hover { border: 1px solid #091940; background-color: #1f326a; color: #fff; }
            </style>
        </head>
        <body>
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

                    document.getElementById('sTopFanStarBalloon_BigFan').insertAdjacentHTML('beforeend', html);                    
                    //document.getElementById(id).insertAdjacentHTML('beforeend', html);                    
                }

                function DelUserHtml(idStr) {

                    var ids = idStr.split('|');
                    for (var i = 0; i < ids.length; i++) {

                        $('#' + ids[i]).css('display', 'none');
                        $('#' + ids[i]).attr('id', 'del_' + $('#' + ids[i]).attr('id'));
                    }

                }

                function showPopup(id) {
                    
                    //var $id = '#' + $(this).attr('id') + '_layer';
                    var id_ = '#' + id + '_layer';
                    layer_popup(id_);
                }

                function layer_popup(el){

                    var $el = $(el);                            // 레이어의 id를 $el 변수에 저장
                    var isDim = $el.prev().hasClass('dimBg');   // dimmed 레이어를 감지하기 위한 boolean 변수

                    isDim ? $('.dim-layer').fadeIn() : $el.fadeIn();

                    var $elWidth = ~~($el.outerWidth()),
                        $elHeight = ~~($el.outerHeight()),
                        docWidth = $(document).width(),
                        docHeight = $(document).height();

                    // 화면의 중앙에 레이어를 띄운다.
                    if ($elHeight < docHeight || $elWidth < docWidth) {
                        $el.css({
                            marginTop: -$elHeight /2,
                            marginLeft: -$elWidth/2
                        })
                    } else {
                        $el.css({top: 0, left: 0});
                    }

                    $el.find('a.btn-layerClose').click(function(){
                        isDim ? $('.dim-layer').fadeOut() : $el.fadeOut(); // 닫기 버튼을 클릭하면 레이어가 닫힌다.
                        return false;
                    });

                    $('.layer .dimBg').click(function(){
                        $('.dim-layer').fadeOut();
                        return false;
                    });

                    $('body').click(function () {

                        isDim ? $('.dim-layer').fadeOut() : $el.fadeOut(); // 닫기 버튼을 클릭하면 레이어가 닫힌다.
                        return false;
                    });

                }
            </script>
            
        </body>
        </html>
        ";

        #region BJ

        /// <summary>
        /// 하위요소 [파라미터 - 아이디, 닉네임, 사진url, 팝업 display html]
        /// </summary>
        public static string BjHtmlChild = @"
                    <div id='{0}'>
                        <div style='display:inline; width:100px; color: #FF0000 !important;font-weight: bold;'>{0}</div>
                        <div style='display:inline; width:100px; color: #333 !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;'>{1}</div>
                        <div style='display:inline; color: #0100FF !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;'>
                            <img src='{2}' alt='' style='height:20px;' />
                        </div>
                    </div>
        ";

        #endregion

        #region 회장

        /// <summary>
        /// 하위요소 [파라미터 - 아이디, 닉네임, 포함된 bj정보, BJ 랭킹 팝업 콘텐츠]
        /// </summary>
        public static string KingHtmlChild = @"
                    <div id='{0}' onclick='javascript:showPopup('{0}')'>
                        <div style='display:inline; width:100px; color: #FF0000 !important;font-weight: bold;'>{0}</div>
                        <div style='display:inline; color: #333 !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;white-space:nowrap;'>{1}</div>
                        <div style='display:inline; color: #0100FF !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;white-space:nowrap;'>
                            {2}
                            <div id='{0}_layer' class='pop-layer'>
                                <div class='pop-container'>
                                    <div class='pop-conts'>
                                        <!--content //-->
                                        {3}
                                        <div class='btn-r'>
                                            <a href = '#' class='btn-layerClose'>닫기</a>
                                        </div>
                                        <!--// content-->
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
        ";

        /// <summary>
        /// Bj 하위요소 [파라미터 - 닉네임, 회장 아이콘Url]
        /// </summary>
        public static string KingHtmlBjChild = @"
                        <span>{0}</span>
                        <img src='{1}' alt='' />
        ";

        /// <summary>
        /// Bj 하위요소 [파라미터 - 순위, 닉네임, 회장 아이콘Url]
        /// </summary>
        public static string BjPopUpContents = @"
                        <span>{0}위</span>
                        <span>{1}</span>
                        <img src='{2}' alt='' /><br/>
        ";

        #endregion

        #region 열혈팬

        /// <summary>
        ///  - 아이디, 닉네임, BJ정보, BJ 랭킹 팝업 콘텐츠
        /// </summary>
        public static string BigFanHtmlChild = @"
                <tr id='{0}' onclick='javascript:showPopup('{0}');'>
                    <td style='width:100px; color: #FF0000 !important;font-weight: bold;'><a href='javascript:showPopup(this);'>{0}</a></td>
                    <td style='color: #333 !important;letter-spacing: -1px;font-weight: bold;font-size: 11px !important;white-space:nowrap;'>{1}</td>
                    <td style='color: #0100FF !important;letter-spacing: -1px;font-weight: bold;font-size: 11px!important;white-space: nowrap;'>
                        {2}
                        <div id='{0}_layer' class='pop-layer'>
                            <div class='pop-container'>
                                <div class='pop-conts'>
                                    <!--content //-->
                                    {3}
                                    <div class='btn-r'>
                                        <a href = '#' class='btn-layerClose'>닫기</a>
                                    </div>
                                    <!--// content-->
                                </div>
                            </div>
                        </div>
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
        <!doctype html>
        <html>
            <head>
                <meta charset='utf-8'>
                <script src='http://code.jquery.com/jquery-latest.min.js'></script>
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

                        $('body').scrollTop($(document).height());
                        //var objDiv = document.getElementById('fan_rank'); 
                        //objDiv.scrollTop = objDiv.scrollHeight;

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
            <p class='notice in fanclub'><a user_id='{0}' user_nick='{1}'>{1}<em>({0})</em></a>님이 대화방에 참여했습니다.</p>
            ";

        /// <summary>
        /// 유저 퇴장
        /// ex_ <p class='notice in fanclub'><a href='javascript:;' user_id='kfiop' user_nick='무쌍.'>팬클럽 무쌍.<em>(kfiop)</em></a>님이 대화방에서 나가셨습니다.</p>
        /// 유저아이디, 닉네임
        /// </summary>
        public static string ChatHtmlOutUser = @"
            <p class='notice in fanclub'><a user_id='{0}' user_nick='{1}'>{1}<em>({0})</em></a>님이 대화방에서 나가셨습니다.</p>
            ";

        #endregion
    }
}
