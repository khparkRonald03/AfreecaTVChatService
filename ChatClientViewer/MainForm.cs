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
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Win32;
using CefSharp;
using CefSharp.WinForms;
using System.Net;
using System.ComponentModel;
using System.Configuration;

namespace ChatClientViewer
{
    public partial class Main : Form
    {
        #region Main 클래스 변수

        Controller ChromeDriver;
        Thread BackGroundCrawlingThread;

        System.Timers.Timer GetUserTimer = new System.Timers.Timer();
        System.Timers.Timer DataDisplayTimer = new System.Timers.Timer();

        WebApiCaller webApiCaller = new WebApiCaller();

        string StartupPath { get; set; } = string.Empty;
        string LoginUserID { get; set; } = string.Empty;
        string LoginuserPW { get; set; } = string.Empty;
        ChromiumWebBrowser UserBrowser { get; set; }
        ChromiumWebBrowser ChatBrowser { get; set; }

        BjModel Bj = new BjModel();

        readonly object LockObject = new object();

        List<UserModel> cUsers = new List<UserModel>();

        Queue<List<UserModel>> DelUsersQueue = new Queue<List<UserModel>>();
        Queue<List<UserModel>> OutChatUsersQueue = new Queue<List<UserModel>>();

        Queue<List<UserModel>> TmpNewUsersQueue = new Queue<List<UserModel>>();
        Queue<List<UserModel>> InUsersQueue = new Queue<List<UserModel>>();
        Queue<List<UserModel>> InChatUsersQueue = new Queue<List<UserModel>>();


        List<ChatModel> cChatQueue = new List<ChatModel>();
        List<ChatModel> nChatQueue = new List<ChatModel>();

        Queue<JsonModel> jsonModels = new Queue<JsonModel>();

        delegate void Control_Invoker();
        delegate void Control_Invoker_ParamStr(string s);
        delegate void Control_Invoker_ParamStrs(string s1, string s2, string s3);
        delegate void Control_Invoker_ParamJsonModel(JsonModel jsonModel);
        delegate void Control_Invoker_ParamUserModles(List<UserModel> userModels);

        #endregion

        #region 생성자 / 이벤트

        public Main(string[] args)
        {
            InitializeComponent();
            Cef.Initialize(new CefSettings());
            InitUserBrowser();
            InitChatBrowser();

            if (args != null && args.Length == 2)
            {
                LoginUserID = args[0];
                LoginuserPW = args[1];
            }

#if DEBUG
            // test #####
            if (string.IsNullOrEmpty(LoginUserID))
                LoginUserID = "hur1168";

            if (string.IsNullOrEmpty(LoginuserPW))
                LoginuserPW = "test";
#endif
            Bj.ID = LoginUserID;
        }

        private void Main_Load(object sender, EventArgs e)
        {
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

            StartupPath = Application.StartupPath;

            if (GetConfigData(AppConfigKeys.DisplayInOut.ToString()) == true.ToString())
                ChkDisplayInOut.Checked = true;

            if (GetConfigData(AppConfigKeys.IsTop.ToString()) == true.ToString())
                ChkIsTop.Checked = true;

            var ts1 = new ThreadStart(BackGroundCrawling);
            BackGroundCrawlingThread = new Thread(ts1)
            {
                IsBackground = true
            };
            BackGroundCrawlingThread.Start();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Opacity = 0;
            BackGroundCrawlingThread?.Abort();
            GetUserTimer?.Stop();
            GetUserTimer?.Close();
            DataDisplayTimer?.Stop();
            DataDisplayTimer.Close();
            ChromeDriver?.CloseDriver();
            ChromeDriver?.Close();

            var process = Process.GetProcessesByName("chromedriver.exe");
            foreach (var p in process)
                p.Kill();
        }


        #region Web Browser Load

        public void InitUserBrowser()
        {
            UserBrowser = new ChromiumWebBrowser("")
            {
                Name = "UserBrowser"
            };
            UserBrowser.LoadHtml(HtmlFormat.UserContainerHtml);

            splitContainer1.Panel1.Controls.Add(UserBrowser);
            UserBrowser.Dock = DockStyle.Fill;
        }

        public void InitChatBrowser()
        {
            ChatBrowser = new ChromiumWebBrowser("")
            {
                Name = "ChatBrowser"
            };
            ChatBrowser.LoadHtml(HtmlFormat.ChatHtml);

            splitContainer1.Panel2.Controls.Add(ChatBrowser);
            ChatBrowser.Dock = DockStyle.Fill;
        }

        #endregion

        #endregion

        #region 방송 정보 수집 (채팅, 접속사용자)

        private void BackGroundCrawling()
        {
            // test ###########
            ChromeDriver = new Controller(false, StartupPath);

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
                Thread.Sleep(4000);
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

                GetUserTimer.Interval = 2000;
                GetUserTimer.Elapsed += new ElapsedEventHandler(GetUserTimer_Elapsed);
                GetUserTimer.Start();

                DataDisplayTimer.Interval = 2000;
                DataDisplayTimer.Elapsed += new ElapsedEventHandler(UIRefreshTimer_Elapsed);
                DataDisplayTimer.Start();


                //var ts2 = new ThreadStart(GetUserTimer_Elapsed);
                //BackGround1 = new Thread(ts2)
                //{
                //    IsBackground = true
                //};
                //BackGround1.Start();

                //var ts3 = new ThreadStart(UIRefreshTimer_Elapsed);
                //BackGround2 = new Thread(ts3)
                //{
                //    IsBackground = true
                //};
                //BackGround2.Start();

                break;
            }
        }

        // 접속 사용자 수집
        private void GetUserTimer_Elapsed(object sender, ElapsedEventArgs e)
        //private void GetUserTimer_Elapsed()
        {
            lock (LockObject)
            {
                var userModels = new List<UserModel>();
                
                // 열혈팬 수집
                var bigFans = GetUser("return document.getElementById('lv_ul_topfan').innerHTML", "//a");
                if (bigFans != null && bigFans.Count > 0)
                    userModels.AddRange(bigFans);

                // 매니저 수집
                var managers = GetUser("return document.getElementById('lv_ul_manager').innerHTML", "//a");
                if (managers != null && managers.Count > 0)
                    userModels.AddRange(managers);

                // 팬 수집
                var fans = GetUser("return document.getElementById('lv_ul_fan').innerHTML", "//a");
                if (fans != null && fans.Count > 0)
                    userModels.AddRange(fans);

                // 구독자 수집
                var gudoks = GetUser("return document.getElementById('lv_ul_gudok').innerHTML", "//a");
                if (gudoks != null && gudoks.Count > 0)
                    userModels.AddRange(gudoks);

                // 일반시청자
                var ulUsers = GetUser("return document.getElementById('lv_ul_user').innerHTML", "//a");
                if (ulUsers != null && ulUsers.Count > 0)
                    userModels.AddRange(ulUsers);

                TmpNewUsersQueue.Enqueue(userModels);

                // 채팅 수집
                GetChat("return document.getElementById('chat_memoyo').innerHTML", "//dl");
                

                // 퇴장 사용자 제거 데이터 매칭 하고 받아오기
                RemoveLeaveUserAndWebApiMatching();
            }
                
        }

        /// <summary>
        /// 페이지 상태 초기화
        /// </summary>
        private void InitProc()
        {
            ChromeDriver.SetUrl($"http://play.afreecatv.com/{LoginUserID}");
            ChromeDriver.ExecuteJS("window.resizeTo(1024, 768);");

            Thread.Sleep(400);
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
        private List<UserModel> GetUser(string script, string xPath)
        {
            // ex -> <span>flowerfree1</span><em>난마도특^^</em>
            var result = new List<UserModel>();

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
                        IsNew = true,
                    };

                    result.Add(userModel);
                }
            }

            return result;
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
                    nChatQueue.Add(new ChatModel()
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

        /// <summary>
        /// 퇴장 사용자 제거 및 서버에서 데이터 매칭해서 받아오기
        /// </summary>
        private void RemoveLeaveUserAndWebApiMatching()
        {
            // 여긴 cUsers 이거 안들어간 상황
            //if (cUsers == null || cUsers.Count <= 0)
            //    return;

            if (TmpNewUsersQueue == null || TmpNewUsersQueue.Count <= 0)
                return;

            var nUsers = TmpNewUsersQueue.Dequeue();

            // 입장 사용자 가져오기
            var tmpInUsers = (from nUser in nUsers
                             join cUser in cUsers on nUser.ID equals cUser.ID into users
                             from cUser in users.DefaultIfEmpty()
                             where cUser is null
                             select nUser)?.ToList()?.Distinct()?.ToList();

            if (cUsers != null && cUsers.Count > 0)
            {
                // 퇴장 사용자 가져오기
                var tmpOutUsers = (from cUser in cUsers
                                   join nUser in nUsers on cUser.ID equals nUser.ID into users
                                   from nUser in users.DefaultIfEmpty()
                                   where nUser is null
                                   select cUser).ToList();

                lock (LockObject)
                {
                    DelUsersQueue.Enqueue(tmpOutUsers);
                    OutChatUsersQueue.Enqueue(tmpOutUsers);
                }
            }

            // 퇴장 사용자 제거
            var tmpcUsers = (from cUser in cUsers
                             join nUser in nUsers on cUser.ID equals nUser.ID
                             select cUser).ToList();

            lock (LockObject)
            {
                cUsers = tmpcUsers;
            }

            ;

            // test #####
            //tmpInUsers = nUsers;

            // web api 매칭 요청
            var returnJModel = Task.Run(() => CallWebApi(tmpInUsers)).Result;

            lock (LockObject)
            {
                jsonModels.Enqueue(returnJModel);
            }

            ;
        }

        private async Task<JsonModel> CallWebApi(List<UserModel> userModels)
        {
            var returnJModels = await webApiCaller.RunAsync(new JsonModel()
            {
                BjModel = Bj,
                UserModels = userModels
            }).ConfigureAwait(true);

            return returnJModels;
        }

        #endregion

        #region 데이터 매칭 처리

        private void UIRefreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (LockObject)
            {
                // web api에서 받아온 데이터 현재 사용자모델에 추가
                ApiDataToUserModel();

                // 접속 사용자 화면에 리프레쉬
                UsersRefresh();

                // 수집 된 채팅 정보 필요없는 것 걸러내고 화면에 리프레쉬
                ChatRefresh();
            }
        }

        /// <summary>
        /// web api에서 받아온 데이터 현재 사용자모델에 추가
        /// </summary>
        private void ApiDataToUserModel()
        {
            var tmpUsers = new List<UserModel>();
            var check = new List<UserModel>();
            while (jsonModels.Count > 0)
            {
                try
                {
                    var jModel = jsonModels.Dequeue();
                    var apiUsers = jModel?.UserModels;
                    
                    if (apiUsers != null)
                    {
                        tmpUsers.AddRange(apiUsers);
                        
                        foreach (var tu in tmpUsers)
                        {
                            if (check.Any(c => c.ID == tu.ID))
                            {
                                continue;
                            }

                            check.Add(tu);
                        }
                    }
                }
                catch (Exception e)
                {
                    string log = e.Message;
                    return;
                }
            }

            lock (LockObject)
            {
                InUsersQueue.Enqueue(check);
                InChatUsersQueue.Enqueue(check);
                cUsers.AddRange(check);
                //cUsers = cUsers.Distinct().ToList();
                var cUsercheck = new List<UserModel>();
                foreach (var tu in cUsers)
                {
                    if (cUsercheck.Any(c => c.ID == tu.ID))
                    {
                        continue;
                    }

                    cUsercheck.Add(tu);
                }
                cUsers = cUsercheck;
            }
            
        }

        /// <summary>
        /// 접속 사용자 화면에 리프레쉬
        /// </summary>
        private void UsersRefresh()
        {
            if (InUsersQueue == null || InUsersQueue.Count <= 0)
                return;

            var inUsers = InUsersQueue.Dequeue();

            string bjHtml = string.Empty;
            string kingHtml = string.Empty;
            string bigFanHtml = string.Empty;

            // 입장 사용자 추가 동작
            foreach (var user in inUsers)
            {
                switch (user.Type)
                {
                    case UserType.BJ:

                        var RankingInfoHtml = string.Empty;
                        if (user.RankingInfo != null)
                        {
                            var tt = user.RankingInfo.GetType().GetProperties();

                            foreach (var t in tt)
                            {
                                var ddd = t.GetValue(user.RankingInfo, null);
                                var zzz = t.GetCustomAttributes(false).GetValue(0);
                                var zzzdname = ((DisplayNameAttribute)zzz).DisplayName;
                                var rnaking = string.Empty;
                                if (!string.IsNullOrEmpty(zzzdname))
                                    RankingInfoHtml += string.Format(HtmlFormat.BjInfoBjPopUpContents1, zzzdname, ddd);
                            }
                                
                        }
                        var bjInfoHtml = string.Empty;
                        if (user.BjInfo != null)
                        {
                            foreach (var bjInfo in user.BjInfo)
                            {
                                if (!string.IsNullOrEmpty(bjInfo.Count))
                                    bjInfoHtml += string.Format(HtmlFormat.BjInfoBjPopUpContents2, bjInfo.Name, bjInfo.Count, bjInfo.Unit);
                            }
                        }
                        bjHtml += string.Format(HtmlFormat.BjHtmlChild, user.ID, user.Nic, user.PictureUrl, RankingInfoHtml, bjInfoHtml);
                        break;

                    case UserType.King:

                        string kingBjsHtml = string.Empty;
                        string popupContentsHtml = string.Empty;
                        if (user.BJs != null)
                        {
                            user.BJs = user.BJs.OrderBy(bj => bj.Ranking)?.ToList();
                            foreach (var bj in user.BJs)
                            {
                                kingBjsHtml += string.Format(HtmlFormat.KingHtmlBjChild, bj.Nic, bj.IconUrl);
                                popupContentsHtml += string.Format(HtmlFormat.UserBjPopUpContents, bj.Ranking, bj.Nic, bj.IconUrl);
                            }
                                    
                        }

                        kingHtml += string.Format(HtmlFormat.KingHtmlChild, user.ID, user.Nic, kingBjsHtml, popupContentsHtml);
                        break;

                    case UserType.BigFan:

                        string bingFanBjsHtml = string.Empty;
                        string popupHtml = string.Empty;
                        if (user.BJs != null)
                        {
                            user.BJs = user.BJs.OrderBy(bj => bj.Ranking)?.ToList();
                            foreach (var bj in user.BJs)
                            {
                                bingFanBjsHtml += string.Format(HtmlFormat.BigFanHtmlBjChild, bj.Nic, bj.IconUrl);
                                popupHtml += string.Format(HtmlFormat.UserBjPopUpContents, bj.Ranking, bj.Nic, bj.IconUrl);
                            }
                                    
                        }

                        bigFanHtml += string.Format(HtmlFormat.BigFanHtmlChild, user.ID, user.Nic, bingFanBjsHtml, popupHtml);
                        break;
                }
            }

            AddUserTable(bjHtml, kingHtml, bigFanHtml);

            if (DelUsersQueue == null || DelUsersQueue.Count <= 0)
                return;

            var tempUsers = new List<string>();
            var outUsers = DelUsersQueue.Dequeue();
            foreach (var user in outUsers)
                tempUsers.Add(user.ID);

            var hideUsers = string.Join("|", tempUsers.ToArray());
            DellUserTable(hideUsers);
        }

        private void AddUserTable(string bjHtml, string kingHtml, string bigFanHtml)
        {
            if (ChatBrowser.InvokeRequired)
            {
                var ci = new Control_Invoker_ParamStrs(AddUserTable);
                this.BeginInvoke(ci, bjHtml, kingHtml, bigFanHtml);
            }
            else
            {
                if (!string.IsNullOrEmpty(bjHtml))
                {
                    UserBrowser.ExecuteScriptAsync("AddUserHtml", new object[] { "sTopFanStarBalloon_BJ", bjHtml });
                }

                if (!string.IsNullOrEmpty(kingHtml))
                {
                    UserBrowser.ExecuteScriptAsync("AddUserHtml", new object[] { "sTopFanStarBalloon_King", kingHtml });
                }

                if (!string.IsNullOrEmpty(bigFanHtml))
                {
                    UserBrowser.ExecuteScriptAsync("AddUserHtml", new object[] { "sTopFanStarBalloon_BigFan", bigFanHtml });
                }
            }
        }

        private void DellUserTable(string delHtml)
        {
            if (UserBrowser.InvokeRequired)
            {
                var ci = new Control_Invoker_ParamStr(DellUserTable);
                this.BeginInvoke(ci, delHtml);
            }
            else
            {
                if (!string.IsNullOrEmpty(delHtml))
                {
                    UserBrowser.ExecuteScriptAsync("DelUserHtml", new object[] { delHtml });
                }
            }
        }

        /// <summary>
        /// 수집 된 채팅 정보 필요없는 것 걸러내고 화면에 리프레쉬
        /// </summary>
        private void ChatRefresh()
        {
            if (cUsers == null || cUsers.Count <= 0)
                return;

            if (nChatQueue == null || nChatQueue.Count <= 0)
                return;

            // 하단에 추가
            string html = string.Empty;

            // test #####
            // 접속 사용자 채팅만 가져오기
            var userJoinChats = (from nChat in nChatQueue
                                 join cUser in cUsers on nChat.ID equals cUser.ID
                                 select nChat).ToList();

            // 기존 채팅 데이터 새 채팅에서 제외 후 중복 제거
            var tmpnChats = (from nChat in userJoinChats
                             join cChat in cChatQueue on new { nChat.ID, nChat.Html } equals new { cChat.ID, cChat.Html } into chat
                             from cChat in chat.DefaultIfEmpty()
                             where cChat is null
                             select nChat)?.Distinct()?.ToList();


            // test
            // 기존 채팅 데이터 새 채팅에서 제외 후 중복 제거
            //var tmpnChats = (from nChat in nChatQueue
            //                 join cChat in cChatQueue on new { nChat.ID, nChat.Html } equals new { cChat.ID, cChat.Html } into chat
            //                 from cChat in chat.DefaultIfEmpty()
            //                 where cChat is null
            //                 select nChat)?.Distinct()?.ToList();


            // 하... 중복제거 린큐로 하는거 실패해서 이걸로 다시 체크..
            var check = new List<ChatModel>();
            foreach (var tt in tmpnChats)
            {
                if (check.Any(t => t.ID == tt.ID && t.Html == tt.Html))
                {
                    ;
                    continue;
                }

                check.Add(tt);
            }

            if (ChkDisplayInOut.Checked && InChatUsersQueue != null && InChatUsersQueue.Count > 0)
            {
                var inUsers = InChatUsersQueue.Dequeue();
                foreach (var inUser in inUsers)
                {
                    html += string.Format(HtmlFormat.ChatHtmlInUser, inUser.ID, inUser.Nic);
                }
            }

            if (ChkDisplayInOut.Checked && OutChatUsersQueue != null && OutChatUsersQueue.Count > 0)
            {
                var outUsers = OutChatUsersQueue.Dequeue();
                foreach (var outUser in outUsers)
                {
                    html += string.Format(HtmlFormat.ChatHtmlOutUser, outUser.ID, outUser.Nic);
                }
            }

            // 채팅 추가
            //foreach (var chat in tmpnChats)
            foreach (var chat in check)
            {
                html += chat.Html;
                cChatQueue.Add(chat);
            }

            nChatQueue.Clear();
            cChatQueue = cChatQueue?.Distinct()?.ToList();

            SetChat(html);
        }

        private void SetChat(string html)
        {
            if (ChatBrowser.InvokeRequired)
            {
                var ci = new Control_Invoker_ParamStr(SetChat);
                this.BeginInvoke(ci, html);
            }
            else
            {
                ChatBrowser.ExecuteScriptAsync("AddChatHtml", new object[] { html });
            }

        }

        private List<T> CloneList<T>(List<T> oldList)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, oldList);
            stream.Position = 0;
            return (List<T>)formatter.Deserialize(stream);
        }

        private T CloneModel<T>(T oldModel)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, oldModel);
            stream.Position = 0;
            return (T)formatter.Deserialize(stream);
        }
        #endregion

        private void BtnSetting_Click(object sender, EventArgs e)
        {
            PlSetting.Visible = !PlSetting.Visible;
        }

        private void ChkDisplayInOut_CheckedChanged(object sender)
        {
            SetConfigData(AppConfigKeys.DisplayInOut.ToString(), ChkDisplayInOut.Checked.ToString());
        }

        private void ChkIsTop_CheckedChanged(object sender)
        {
            this.TopMost = ChkIsTop.Checked;
            SetConfigData(AppConfigKeys.IsTop.ToString(), ChkIsTop.Checked.ToString());
        }

        #region App.config 수정 / 가져오기

        /// <summary>
        /// App.config 파일 설정 데이터 가져오기
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetConfigData(string key)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                string value = ConfigurationManager.AppSettings[key];
                return value;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// App.config 파일 설정 데이터 수정
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool SetConfigData(string key, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings[key].Value = value;

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(key);

                return true;
            }
            catch
            {
                return false;
            }
        }


        public enum AppConfigKeys
        {
            /// <summary>
            /// 
            /// </summary>
            DisplayInOut,

            /// <summary>
            /// 
            /// </summary>
            IsTop
        }

        #endregion

        private void BtnSetting_MouseHover(object sender, EventArgs e)
        {
            BtnSetting.ImageIndex = 7;
        }

        private void BtnSetting_MouseLeave(object sender, EventArgs e)
        {
            BtnSetting.ImageIndex = 5;
        }

        private void BtnReStart_MouseHover(object sender, EventArgs e)
        {
            BtnReStart.ImageIndex = 8;
        }

        private void BtnReStart_MouseLeave(object sender, EventArgs e)
        {
            BtnReStart.ImageIndex = 6;
        }
    }
}
