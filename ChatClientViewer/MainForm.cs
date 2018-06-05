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

        string LoginUserID { get; set; } = string.Empty;
        string LoginuserPW { get; set; } = string.Empty;

        BjModel Bj = new BjModel();

        bool AddNewUserFlag = false;

        readonly object LockObject = new object();

        List<UserModel> cUsers = new List<UserModel>();

        List<UserModel> N_Users;
        List<UserModel> nUsers
        {
            get
            {
                if (N_Users == null)
                {
                    lock (LockObject)
                    {
                        if (N_Users == null)
                            N_Users = new List<UserModel>();
                    }
                }

                lock (LockObject)
                {
                    N_Users = N_Users.FindAll(n => n.IsNew);
                    return N_Users;
                }
            }
            set
            {
                N_Users = value;
            }
        }

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

            if (args != null && args.Length == 2)
            {
                LoginUserID = args[0];
                LoginuserPW = args[1];
            }

#if DEBUG
            // test #####
            if (string.IsNullOrEmpty(LoginUserID))
                LoginUserID = "sby1087";

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

            WbUser.DocumentText = HtmlFormat.UserContainerHtml;
            WbChat.DocumentText = HtmlFormat.ChatHtml;

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
        }

        #endregion

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
                // 열혈팬 수집
                GetUser("return document.getElementById('lv_ul_topfan').innerHTML", "//a");

                // 매니저 수집
                GetUser("return document.getElementById('lv_h3_manager').innerHTML", "//a");

                // 팬 수집
                GetUser("return document.getElementById('lv_ul_fan').innerHTML", "//a");

                // 구독자 수집
                GetUser("return document.getElementById('lv_h3_gudok').innerHTML", "//a");

                // 일반시청자
                GetUser("return document.getElementById('lv_ul_user').innerHTML", "//a");

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
                        IsNew = true,
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

            if (nUsers == null || nUsers.Count <= 0)
                return;

            // 퇴장사용자 저장
            var tmpnUsers = (from nUser in nUsers
                             join cUser in cUsers on nUser.ID equals cUser.ID into users
                             from cUser in users.DefaultIfEmpty()
                             where cUser is null
                             select nUser).ToList();

            // 퇴장 사용자 제거
            var tmpcUsers = (from cUser in cUsers
                             join nUser in nUsers on cUser.ID equals nUser.ID
                             select cUser).ToList();

            cUsers = tmpcUsers;

            var cloneUsers = CloneList(nUsers);

            // 기존 사용자 데이터 매칭 사용자에서 제거 ################
            //var tmpnUsers = (from nUser in cloneUsers
            //                 join cUser in cUsers on nUser.ID equals cUser.ID into users
            //                 from cUser in users.DefaultIfEmpty()
            //                 where cUser is null
            //                 select nUser).ToList();

            foreach (var tUser in tmpnUsers)
                tUser.IsNew = true;

            foreach (var user in nUsers)
            {
                if (cloneUsers.Any(c => c.ID == user.ID))
                    user.IsNew = false;
            }

            ;
            // web api 매칭 요청
            var returnJModel = Task.Run(() => CallWebApi(tmpnUsers)).Result;

            lock (LockObject)
            {
                var cloneModel = CloneModel(returnJModel);
                jsonModels.Enqueue(cloneModel);
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
            while (jsonModels.Count > 0)
            {
                try
                {
                    var jModel = jsonModels.First();
                    var apiUsers = jModel?.UserModels;
                    if (apiUsers != null)
                    {
                        tmpUsers.AddRange(apiUsers);
                        jsonModels.Dequeue();
                    }
                }
                catch(Exception e)
                {
                    string log = e.Message;
                    return;
                }
            }

            cUsers.AddRange(tmpUsers);
            cUsers = cUsers.Distinct().ToList();
            AddNewUserFlag = true;
        }

        /// <summary>
        /// 접속 사용자 화면에 리프레쉬
        /// </summary>
        private void UsersRefresh()
        {
            if (!AddNewUserFlag)
                return;

            AddNewUserFlag = false;
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

            SetUserHtml(bjHtml, kingHtml, bigFanHtml);
        }

        private void SetUserHtml(string bjHtml, string kingHtml, string bigFanHtml)
        {
            if (WbUser.InvokeRequired)
            {
                var ci = new Control_Invoker_ParamStrs(SetUserHtml);
                this.BeginInvoke(ci, bjHtml, kingHtml, bigFanHtml);
            }
            else
            {
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

            // 기존 채팅 데이터 새 채팅에서 제외
            var tmpnChats = (from nChat in userJoinChats
                             join cChat in cChatQueue on new { nChat.ID, nChat.Html } equals new { cChat.ID, cChat.Html } into chat
                             from cChat in chat.DefaultIfEmpty()
                             where cChat is null
                             select nChat).ToList();

            // 중복 제거
            tmpnChats = tmpnChats?.Distinct()?.ToList() ?? new List<ChatModel>();

            // test
            //var tmpnChats = nChatQueue;

            foreach (var chat in tmpnChats)
            {
                html += chat.Html;
                cChatQueue.Add(chat);
            }

            nChatQueue.Clear();

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
                //WbChat.Document.Body.ScrollIntoView(false);

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
    }
}
