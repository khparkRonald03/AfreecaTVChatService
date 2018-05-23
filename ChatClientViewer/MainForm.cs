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

        BjModel Bj = new BjModel();

        List<BjModel> cBjs = new List<BjModel>();
        List<KingModel> cKings = new List<KingModel>();
        List<UsersModel> cBigFans = new List<UsersModel>();

        List<BjModel> nBjs = new List<BjModel>();
        List<KingModel> nKings = new List<KingModel>();
        List<UsersModel> nBigFans = new List<UsersModel>();

        List<ChatModel> TempChatQueue = new List<ChatModel>();
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
            BackGroundThread = new Thread(ts)
            {
                IsBackground = true
            };
            BackGroundThread.Start();
        }

        #region 방송 정보 수집 (채팅, 접속사용자)

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

                    // 현재 방송 bj 수집
                    GetBj();

                    // 열혈팬 수집
                    GetBigFan();

                    // 팬
                    var fanNodes = GetNodes("return document.getElementById('lv_ul_fan').innerHTML", "//li/a");

                    // 일반시청자
                    var userNodes = GetNodes("return document.getElementById('lv_ul_user').innerHTML", "//li/a");


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

        private void GetBj()
        {
            if (Bj != null && !string.IsNullOrEmpty(Bj.ID))
                return;

            var fanNode = GetNode("return document.getElementById('lv_p_bj').innerHTML", "//li/a");
            if (fanNode == null)
                return;

            fanNode.Html;
        }

        /// <summary>
        /// 빅팬 수집 (현재 방송의 빅팬이므로 바로 목록에 추가)
        /// </summary>
        private void GetBigFan()
        {
            // <span>flowerfree1</span><em>난마도특^^</em>
            var bigFanNodes = GetNodes("return document.getElementById('lv_ul_topfan').innerHTML", "//li/a");

            foreach (var bigFan in bigFanNodes)
            {
                var html = bigFan.Html;
                var bfs = html.Split(new string[] { "<span>", "</span>", "<em>", "</em>" }, StringSplitOptions.None);
                if (bfs != null && bfs.Length == 2)
                {
                    var bigFanModel = new UsersModel()
                    {
                        ID = bfs[0],
                        Nic = bfs[1]
                    };
                    bigFanModel.BJs = new List<BjModel>
                    {
                        new BjModel()
                        {
                            ID = Bj.ID,
                            Nic = Bj.Nic,
                            IconUrl = string.Empty,
                            PictureUrl = string.Empty
                        }
                    };
                }


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
            int interval = 3;
            do
            {
                var ttttt0 = ChromeDriver.ExecuteJSReturnHtml(script);
                var page0 = new BeautifulPage(ttttt0.ToString());
                var ttt0 = page0.SelectNode(xPath);

                if (ttt0 != null)
                    return ttt0;

                interval--;
                Thread.Sleep(300);
            }
            while (interval > 0);

            return null;
        }

        private IEnumerable<BeautifulNode> GetNodes(string script, string xPath)
        {
            int interval = 3;
            do
            {
                var ttttt0 = ChromeDriver.ExecuteJSReturnHtml(script);
                var page0 = new BeautifulPage(ttttt0.ToString());
                var ttt0 = page0.SelectNodes(xPath);

                if (ttt0 != null)
                    return ttt0;

                interval--;
                Thread.Sleep(300);
            }
            while (interval > 0);

            return null;
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

        #endregion

        #region 매칭정보

        private void UsersMatching()
        {

        }

        private void ChatMatching()
        {

        }

        #endregion
    }
}
