using BeautifulWeb;
using DataModels;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Forms;
using WebController;

namespace ChatClientViewer
{
    public partial class Main : Form
    {
        Controller ChromeDriver;
        Thread BackGroundThread;
        string LoginUserID { get; set; } = string.Empty;

        List<BjModel> Bjs = new List<BjModel>();
        List<KingModel> Kings = new List<KingModel>();
        List<BigFanModel> BigFans = new List<BigFanModel>();
        List<ChatModel> Chats = new List<ChatModel>();

        Queue<BjModel> BjQueue = new Queue<BjModel>();
        Queue<KingModel> KingQueue = new Queue<KingModel>();
        Queue<BigFanModel> BigFanQueue = new Queue<BigFanModel>();
        Queue<ChatModel> ChatQueue = new Queue<ChatModel>();

        public Main()
        {
            InitializeComponent();
            
            //webBrowser1.DocumentText = HtmlFormat.BjHtml;
            //webBrowser2.DocumentText = HtmlFormat.KingHtml;
            //webBrowser3.DocumentText = HtmlFormat.BigFanHtml;
            //webBrowser4.DocumentText = HtmlFormat.ChatHtml;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            (new ShowHelper()).ShowDialog();

            var ts = new ThreadStart(BackGroundProc);
            BackGroundThread = new Thread(ts);
            BackGroundThread.IsBackground = true;
            BackGroundThread.Start();
        }

        private void BackGroundProc()
        {
            ChromeDriver = new Controller(false);

            var startResult = ChromeDriver.Start();
            if (!startResult.ResultValue)
            {
                //
                return;
            }

            
            ChromeDriver.SetUrl($"http://play.afreecatv.com/{LoginUserID}");

            Thread.Sleep(500);
            //ChromeDriver.ClickTag(ElementsSelectType.Id, "livePlayer");
            //ChromeDriver.ClickTag(ElementsSelectType.XPath, "//*[@id='afreecatv_player']/div[6]/div[2]/div[1]/button");
            //ChromeDriver.ClickTag(ElementsSelectType.XPath, "//*[@id='afreecatv_player']/div[6]/div[2]/div[1]/div/button");
            //ChromeDriver.ClickTag(ElementsSelectType.XPath, "//*[@id='afreecatv_player']/div[6]/div[2]/div[1]/div/ul/li[1]/button");

            Thread.Sleep(200);
            ChromeDriver.ExecuteJS("$('#afreecatv_player > div.player_ctrlBox > div.right_ctrl > div.setting_box > button').click()");
            Thread.Sleep(200);
            ChromeDriver.ExecuteJS("$('#afreecatv_player > div.player_ctrlBox > div.right_ctrl > div.setting_box > div > button').click()");
            Thread.Sleep(200);
            // html5 변환 버튼
            ChromeDriver.ExecuteJS("$('#afreecatv_player > div.player_ctrlBox > div.right_ctrl > div.setting_box.on > div > ul > li:nth-child(1) > button').click()");
            Thread.Sleep(500);

            while (true)
            {
                //광고스킵
                ClickPromotionBtnSkip();

                // 채팅 가져오기
                var script0 = "return document.getElementById('chat_memoyo').innerHTML";
                var t = ChromeDriver.ExecuteJSReturnHtml(script0);
                var page0 = new BeautifulPage(t.ResultValue);
                var ttt0 = page0.SelectNodes("//dl") as List<BeautifulNode>;

                if (ttt0.Count > 0)
                {
                    // 접속 사용자 버튼 클릭
                    ShowSetboxViewer();

                    // 접속 사용자 수집

                    // 열혈팬
                    var ttt2 = GetNodes("return document.getElementById('lv_ul_topfan').innerHTML", "//li/a");
                    // <span>flowerfree1</span><em>난마도특^^</em>

                    // 팬
                    var ttt3 = GetNodes("return document.getElementById('lv_ul_fan').innerHTML", "//li/a");

                    // 일반시청자
                    var ttt4 = GetNodes("return document.getElementById('lv_ul_user').innerHTML", "//li/a");


                    // 채팅 수집
                    var ttt00 = GetNodes("return document.getElementById('chat_memoyo').innerHTML", "//dl");
                    foreach (var tt in ttt00)
                    {
                        var html = tt.Html;
                        var NicAndId = tt.Text; // nic(id):
                    }

                }

                Thread.Sleep(1500);
            }
        }

        /// <summary>
        /// 광고 스킵버튼 클릭
        /// </summary>
        /// <returns></returns>
        private bool ClickPromotionBtnSkip()
        {
            var result = ChromeDriver.ExecuteJS("$('#promotion_btn_skip').click()");
            return result.ResultValue;
        }

        private BeautifulNode GetNode(string script, string xPath)
        {
            var ttttt0 = ChromeDriver.ExecuteJSReturnHtml(script);
            var page0 = new BeautifulPage(ttttt0.ToString());
            var ttt0 = page0.SelectNode(xPath);

            return ttt0;
        }

        private IEnumerable<BeautifulNode> GetNodes(string script, string xPath)
        {
            //int interval = 3;
            //do
            //{
                var ttttt0 = ChromeDriver.ExecuteJSReturnHtml(script);
                var page0 = new BeautifulPage(ttttt0.ToString());
                var ttt0 = page0.SelectNodes(xPath);

                //interval--;
                //Thread.Sleep(300);
            //}
            //while (interval > 0);
            return ttt0;
        }

        /// <summary>
        /// 접속 사용자 명단 펼치기
        /// </summary>
        private bool ShowSetboxViewer()
        {
            // 채팅 클래스 on 인지 확인
            // //*[@id="setbox_viewer"]/a    
            int interval = 3;
            do
            {
                var ttt = GetNode("return document.getElementById('setbox_viewer').innerHTML", "//a");
                if (ttt.Class == "on")
                    return true;
                
                var script0 = "$('#setbox_viewer').click()";
                var t = ChromeDriver.ExecuteJS(script0);

                interval--;
                Thread.Sleep(300);
            }
            while (interval > 0);

            return false;
        }

        /// <summary>
        /// 굳이 안없애도 되면 없애지 않기
        /// </summary>
        /// <param name="chatList"></param>
        /// <param name="chatText"></param>
        /// <param name="isFirst"></param>
        private void RemoveTag(ReadOnlyCollection<IWebElement> chatList, string chatText, ref bool isFirst)
        {
            if (chatList.Count > 4 && !string.IsNullOrEmpty(chatText) && isFirst)
            {
                var videobox = ChromeDriver.FindElement(ElementsSelectType.ClassName, "videobox");
                ChromeDriver.ExecuteJS(@"
                                        var element = arguments[0];
                                        element.parentNode.removeChild(element);", videobox);

                var listbox = ChromeDriver.FindElement(ElementsSelectType.ClassName, "listbox");
                ChromeDriver.ExecuteJS(@"
                                        var element = arguments[0];
                                        element.parentNode.removeChild(element);", listbox);

                isFirst = false;
            }
        }
    }
}
