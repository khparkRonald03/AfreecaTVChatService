using BeautifulWeb;
using DataModels;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Forms;
using WebController;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Timers;

namespace ChatClientViewer
{
    public partial class Main : Form
    {
        Controller ChromeDriver;
        Thread BackGroundCrawlingThread;
        System.Timers.Timer timer = new System.Timers.Timer();

        string LoginUserID { get; set; } = string.Empty;
        string LoginuserPW { get; set; } = string.Empty;

        BjModel Bj = new BjModel();

        object LockObject = new object();

        List<UserModel> cUsers = new List<UserModel>();
        List<UserModel> nUsers = new List<UserModel>();
        List<ChatModel> ChatQueue = new List<ChatModel>();

        delegate void Control_Invoker();
        delegate void Control_Invoker_ParamStr(string s);

        public Main(string[] args)
        {
            InitializeComponent();

            if (args != null && args.Length == 2)
            {
                LoginUserID = args[0];
                LoginuserPW = args[1];
            }

#if DEBUG
            // test #####
            if (string.IsNullOrEmpty(LoginUserID))
                LoginUserID = "";

            if (string.IsNullOrEmpty(LoginuserPW))
                LoginuserPW = "test";
#endif
        }

        private void Main_Load(object sender, EventArgs e)
        {
#if DEBUG
            //test
            ;
            TestWebApiForm test = new TestWebApiForm();
            return;

#endif

            if (string.IsNullOrEmpty(LoginUserID))
            {
                this.Close();
                return;
            }

            if (string.IsNullOrEmpty(LoginuserPW))
            {
                this.Close();
                return;
            }

            (new ShowHelper()).ShowDialog();

            WbUser.DocumentText = HtmlFormat.UserContainerHtml;
            WbChat.DocumentText = HtmlFormat.ChatHtml;

            var ts1 = new ThreadStart(BackGroundCrawling);
            BackGroundCrawlingThread = new Thread(ts1)
            {
                IsBackground = true
            };
            BackGroundCrawlingThread.Start();
        }

        #region 방송 정보 수집 (채팅, 접속사용자)

        private void BackGroundCrawling()
        {
            ChromeDriver = new Controller(false);

            // 먹통 됐을때 다시 시작
            reStart:

            var startResult = ChromeDriver.Start();
            if (!startResult.ResultValue)
            {
                // 
                return;
            }

            // 페이지 상태 초기화
            InitProc();
            bool isFirst = true;
            bool isStart = false;
            int startInterval = 10;
            while (true)
            {
                Thread.Sleep(7000);
                //광고스킵
                if (!isStart)
                    ClickPromotionBtnSkip();

                // 채팅 가져오기
                //var ttt0 = GetChatNodes("return document.getElementById('chat_memoyo').innerHTML", "//dl");
                var pageSource = ChromeDriver.GetPageSource();
                var ttt0 = GetChatNodes(pageSource.ResultValue, "//*[@id='chat_memoyo']/dl");


                if (ttt0.Count <= 1)
                {
                    Thread.Sleep(300);
                    startInterval--;
                    if (startInterval == 0)
                    {
                        ChromeDriver.Close();
                        goto reStart;
                    }
                    continue;
                }

                // 현재 방송 bj 수집
                if (isFirst)
                {
                    GetBj();
                    isFirst = false;
                }

                isStart = true;

                // 접속 사용자 버튼 클릭
                if (!ShowSetboxViewer())
                    continue;

                timer.Interval = 2000;
                timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
                timer.Start();

                break;
            }
        }

        // 접속 사용자 수집
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // 열혈팬 수집
            GetUser("return document.getElementById('lv_ul_topfan').innerHTML", "//a");

            // 팬 수집
            GetUser("return document.getElementById('lv_ul_fan').innerHTML", "//a");

            // 일반시청자
            GetUser("return document.getElementById('lv_ul_user').innerHTML", "//a");

            // 채팅 수집
            GetChat("return document.getElementById('chat_memoyo').innerHTML", "//dl");

            // UI 디스플레이
            BackGroundMatchingProc();
        }

        /// <summary>
        /// 페이지 상태 초기화
        /// </summary>
        private void InitProc()
        {
            ChromeDriver.SetUrl($"http://play.afreecatv.com/{LoginUserID}");

            Thread.Sleep(200);
            ChromeDriver.ExecuteJS("$('#afreecatv_player > div.player_ctrlBox > div.right_ctrl > div.setting_box > button').click()");
            Thread.Sleep(200);
            ChromeDriver.ExecuteJS("$('#afreecatv_player > div.player_ctrlBox > div.right_ctrl > div.setting_box > div > button').click()");
            Thread.Sleep(200);
            // html5 변환 버튼
            ChromeDriver.ExecuteJS("$('#afreecatv_player > div.player_ctrlBox > div.right_ctrl > div.setting_box.on > div > ul > li:nth-child(1) > button').click()");
            Thread.Sleep(200);
        }

        private List<BeautifulNode> GetChatNodes(string script, string xPath)
        {
            int interval = 3;
            do
            {
                var page0 = new BeautifulPage(script);

                var ttt0 = page0.SelectNodes(xPath).ToList();
                if (ttt0 != null && ttt0.Count > 0)
                    return ttt0;

                interval--;

                Thread.Sleep(1500);
            }
            while (interval > 0);

            return new List<BeautifulNode>();
        }

        private void GetBj()
        {
            if (Bj != null && !string.IsNullOrEmpty(Bj.ID))
                return;

            var fanNode = GetNode("return document.getElementById('lv_p_bj').innerHTML", "//a");
            if (fanNode == null)
                return;

            var html = fanNode.Html;
            if (html == null)
                return;

            var bfs = html.Split(new string[] { "<span>", "</span>", "<em>", "</em>" }, StringSplitOptions.RemoveEmptyEntries);
            if (bfs != null && bfs.Length == 2)
            {
                Bj.ID = bfs[1];
                Bj.Nic = bfs[0];
            }
        }

        /// <summary>
        /// 열혈팬 수집
        /// </summary>
        private void GetUser(string script, string xPath)
        {
            // ex -> <span>flowerfree1</span><em>난마도특^^</em>
            var bigFanNodes = GetNodes(script, xPath);

            foreach (var bigFan in bigFanNodes)
            {
                var html = bigFan.Html;
                var bfs = html.Split(new string[] { "<span>", "</span>", "<em>", "</em>" }, StringSplitOptions.RemoveEmptyEntries);
                if (bfs != null && bfs.Length == 2)
                {
                    var userModel = new UserModel()
                    {
                        ID = bfs[0],
                        Nic = bfs[1],
                        Type = UserType.King, // test########################
                    };

                    nUsers.Add(userModel);
                }
            }

        }

        private void GetChat(string script, string xPath)
        {
            var ttt00 = GetNodes(script, xPath);
            foreach (var tt in ttt00)
            {
                var NicAndId = tt.Text; // nic(id):대화
                var NicAndIdArray = NicAndId.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
                if (NicAndIdArray != null && NicAndIdArray.Length == 3)
                {
                    ChatQueue.Add(new ChatModel()
                    {
                        ID = NicAndIdArray[1],
                        Nic = NicAndIdArray[0],
                        Html = $"<dl class=''> {tt.Html} </dl>",
                        IsNew = true
                    });
                }
                else
                {
                    // 채팅이 아닌경우
                    // 처음 인사말, 퇴장/강퇴
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
                var page0 = new BeautifulPage(ttttt0.ResultValue);
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
                var page0 = new BeautifulPage(ttttt0.ResultValue);
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
            
            int interval = 3;
            do
            {
                var ttt = GetNode("return document.getElementById('setbox_viewer').innerHTML", "//a");

                // 채팅 클래스 on 인지 확인
                if (ttt.Class == "on")
                    return true;

                var script0 = "$('#setbox_viewer > a').click()";
                var t = ChromeDriver.ExecuteJS(script0);
                var tttt = GetNode("return document.getElementById('setbox_viewer').innerHTML", "//a");
                if (tttt.Class == "on")
                    return true;
                
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

        #region 데이터 매칭 처리

        private void BackGroundMatchingProc()
        {
            // 퇴장 사용자 제거
            RemoveLeaveUser();

            // 데이터 매칭 하고 받아오고 접속 사용자 화면에 리프레쉬
            UsersMatching();

            // 수집 된 채팅 정보 필요없는 것 걸러내고 화면에 리프레쉬
            ChatMatching();
        }

        /// <summary>
        /// 퇴장 사용자 제거
        /// </summary>
        private void RemoveLeaveUser()
        {
            if (cUsers == null || cUsers.Count <= 0)
                return;

            if (nUsers == null || nUsers.Count <= 0)
                return;

            // 퇴장 사용자 제거
            var tmpcUsers = (from cUser in cUsers
                           join nUser in nUsers on cUser.ID equals nUser.ID
                           select cUser).ToList();

            cUsers = tmpcUsers;

            // 기존 사용자 데이터 매칭 사용자에서 제거
            var tmpnUsers = (from nUser in nUsers
                             join cUser in cUsers on nUser.ID equals cUser.ID into users
                             from cUser in users.DefaultIfEmpty()
                             where cUser is null
                             select nUser).ToList();

            nUsers = tmpnUsers;
        }

        /// <summary>
        /// 데이터 매칭하고 받아오고 접속 사용자 화면에 리프레쉬
        /// </summary>
        private void UsersMatching()
        {
            // 기존 항목 모두 최신 추가 항목 지우기
            //foreach (var user in cUsers)
            //    user.IsNew = false;

            // 매칭 서비스에서 데이터 받아오기 ##########

            if (nUsers.Count <= 0)
                return;

            // test ##########################
            //foreach(var i in nUsers)
            //{
            //    i.Type = UserType.King;
            //}

            cUsers.AddRange(nUsers);

            // 사용자 리프레쉬 동작
            SetUsers(); 
        }

        /// <summary>
        /// 사용자 리프레쉬 동작
        /// </summary>
        private void SetUsers()
        {
            if (WbChat.InvokeRequired)
            {
                var ci = new Control_Invoker(SetUsers);
                this.BeginInvoke(ci, null);
            }
            else
            {
                string bjHtml = string.Empty;
                string kingHtml = string.Empty;
                string bigFanHtml = string.Empty;

                // 사용자 리프레쉬 동작
                foreach (var user in cUsers)
                {
                    switch (user.Type)
                    {
                        case UserType.BJ:

                            if (string.IsNullOrEmpty(user.Html))
                                bjHtml += string.Format(HtmlFormat.BjHtmlChild, user.ID, user.Nic, user.PictureUrl);
                            else
                                bjHtml += user.Html;
                            break;

                        case UserType.King:

                            if (string.IsNullOrEmpty(user.Html))
                            {
                                string kingBjsHtml = string.Empty;
                                if (user.BJs != null)
                                {
                                    foreach (var bj in user.BJs)
                                        kingBjsHtml += string.Format(HtmlFormat.KingHtmlBjChild, bj.Nic, bj.IconUrl);
                                }
                                kingHtml += string.Format(HtmlFormat.KingHtmlChild, user.ID, user.Nic, kingBjsHtml);
                            }
                            else
                            {
                                kingHtml += user.Html;
                            }
                            break;

                        case UserType.BigFan:

                            if (string.IsNullOrEmpty(user.Html))
                            {
                                string bingFanBjsHtml = string.Empty;
                                if (user.BJs != null)
                                {
                                    foreach (var bj in user.BJs)
                                        bingFanBjsHtml += string.Format(HtmlFormat.BigFanHtmlBjChild, bj.Nic, bj.IconUrl);
                                }
                                
                                bigFanHtml += string.Format(HtmlFormat.KingHtmlChild, user.ID, user.Nic, bingFanBjsHtml);
                            }
                            else
                            {
                                bigFanHtml += user.Html;
                            }
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(bjHtml))
                {
                    string bjCon = string.Format(HtmlFormat.UserTableHtml, bjHtml);
                    WbUser.Document.InvokeScript("AddUserHtml", new object[] { "sTopFanStarBalloon_BJ", bjCon });
                }
                
                if (!string.IsNullOrEmpty(kingHtml))
                {
                    string kingCon = string.Format(HtmlFormat.UserTableHtml, kingHtml);
                    WbUser.Document.InvokeScript("AddUserHtml", new object[] { "sTopFanStarBalloon_King", kingCon });
                }
                
                if (!string.IsNullOrEmpty(bigFanHtml))
                {
                    string bigFanCon = string.Format(HtmlFormat.UserTableHtml, bigFanHtml);
                    WbUser.Document.InvokeScript("AddUserHtml", new object[] { "sTopFanStarBalloon_BigFan", bigFanCon });
                }
                

                nUsers.Clear();
            }

        }

        /// <summary>
        /// 수집 된 채팅 정보 필요없는 것 걸러내고 화면에 리프레쉬
        /// </summary>
        private void ChatMatching()
        {
            // 하단에 추가
            string html = string.Empty;
            int endIndx = ChatQueue.Count;
            for (int idx = 0; idx < endIndx; idx++)
            {
                if (!ChatQueue[idx].IsNew)
                    continue;

                html = ChatQueue[idx].Html;
                    
                ChatQueue[idx].IsNew = false;
            }

            html = html.Replace("<em class=\"pc\">", "<em class='pc' style='margin-left:-30px;'>");
            SetChat(html);

        }

        private void InitChat()
        {
            if (WbChat.InvokeRequired)
            {
                var ci = new Control_Invoker(InitChat);
                this.BeginInvoke(ci, null);
            }
            else
            {
                string paramHtml = HtmlFormat.ChatHtml;
                WbChat.DocumentText = paramHtml;
            }

        }

        private void SetChat(string html)
        {
            if (WbChat.InvokeRequired)
            {
                var ci = new Control_Invoker_ParamStr(SetChat);
                this.BeginInvoke(ci, html);
            }
            else
            {
                // 처음이면 폼 모두 생성
                if (string.IsNullOrEmpty(WbChat.DocumentText))
                    InitChat();

                WbChat.Document.InvokeScript("AddChatHtml", new object[] { html });
                WbChat.Document.Body.ScrollIntoView(false);

            }

        }

        public List<T> CloneList<T>(List<T> oldList)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, oldList);
            stream.Position = 0;
            return (List<T>)formatter.Deserialize(stream);
        }
        #endregion

    }
}
