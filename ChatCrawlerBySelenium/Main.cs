using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebController;

namespace ChatCrawlerBySelenium
{
    public partial class Main : Form
    {
        int GroupIndex;
        Thread ActionThread = null;
        bool IsStart { get; set; }
        Dictionary<int, string> Urls { get; set; } = new Dictionary<int, string>();

        Controller ChromeDriver;
        delegate void ControlInvoker(string text);

        public Main(string[] args)
        {
            InitializeComponent();

            if (args != null && args.Length > 0)
            {
                IsStart = true;
                int.TryParse(args[0], out GroupIndex);
                for (int Idx = 1; Idx < args.Length; Idx++)
                {
                    Urls.Add(Idx, args[Idx] ?? string.Empty);
                }
            }

        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (Urls.Count > 0)
            {
                this.Text += GroupIndex.ToString();

                for (int Idx = 1; Idx < Urls.Count; Idx++)
                {
                    switch(Idx)
                    {
                        case 1:
                            LblText1.Text += Urls[1];
                            break;
                        case 2:
                            LblText2.Text += Urls[2];
                            break;
                        case 3:
                            LblText3.Text += Urls[3];
                            break;
                    }
                }
                
            }

            Open();
        }

        private void Open()
        {
            ChromeDriver = new Controller();

            var startResult = ChromeDriver.Start();
            if (!startResult.ResultValue)
            {
                //
                return;
            }

            #region
            //test
            //string[] urls = new string[3]
            //{
            //    "http://play.afreecatv.com/kamding/203794313",
            //    "http://play.afreecatv.com/b13246",
            //    "http://play.afreecatv.com/raikos"
            //};

            //ChromeDriver.SetUrl(urls[0]);
            //Thread.Sleep(500);
            //ChromeDriver.ClickTag(ElementsSelectType.Id, "livePlayer");
            //ChromeDriver.ClickTag(ElementsSelectType.XPath, "//*[@id='afreecatv_player']/div[6]/div[2]/div[1]/button");
            //ChromeDriver.ClickTag(ElementsSelectType.XPath, "//*[@id='afreecatv_player']/div[6]/div[2]/div[1]/div/button");
            //ChromeDriver.ClickTag(ElementsSelectType.XPath, "//*[@id='afreecatv_player']/div[6]/div[2]/div[1]/div/ul/li[1]/button");
            ////ChromeDriver.NewTabOpen(urls[1]);
            ////ChromeDriver.NewTabOpen(urls[2]);


            //Action();  return;
            //return;
            #endregion

            foreach (var url in Urls)
            {
                if ( url.Key == 1)
                    ChromeDriver.SetUrl(url.Value);
                else
                    ChromeDriver.NewTabOpen(url.Value);

                Thread.Sleep(500);
                ChromeDriver.ClickTag(ElementsSelectType.Id, "livePlayer");
                ChromeDriver.ClickTag(ElementsSelectType.XPath, "//*[@id='afreecatv_player']/div[6]/div[2]/div[1]/button");
                ChromeDriver.ClickTag(ElementsSelectType.XPath, "//*[@id='afreecatv_player']/div[6]/div[2]/div[1]/div/button");
                ChromeDriver.ClickTag(ElementsSelectType.XPath, "//*[@id='afreecatv_player']/div[6]/div[2]/div[1]/div/ul/li[1]/button");

                //Thread.Sleep(1000);
                //ChromeDriver.ClickTag(ElementsSelectType.Id, "livePlayer");
                //ChromeDriver.ClickTag(ElementsSelectType.Id, "btn_sound");
            }


            // Thread 두개로 나눠서 하나는 탭돌면서 계속 수집하여 리스트에 넣어주고
            // 하나는 리스트에 있는 것을 계속 쏘아주자
            var ts = new ThreadStart(Action);
            ActionThread = new Thread(ts);
            ActionThread.Start();

        }

        private void Action()
        {
            if (ChromeDriver == null)
                return;

            bool isFirst1 = true;
            bool isFirst2 = true;
            bool isFirst3 = true;

            for (int Idx = 0; Idx <= ChromeDriver.Driver.WindowHandles.Count; Idx++)
            {
                if (Idx == ChromeDriver.Driver.WindowHandles.Count)
                    Idx = 0;

                Thread.Sleep(500);
                ChromeDriver.MoveTab(ChromeDriver.Driver.WindowHandles[Idx]);

                // 엘레멘츠 가져오기
                var chatList = ChromeDriver.FindElements(ElementsSelectType.XPath, "//*[@id='chat_memoyo']/dl");
                foreach (var chat in chatList)
                {
                    string chatText = $"{chat.Text}{Environment.NewLine}";

                    ;

                    switch (Idx)
                    {
                        case 0:
                            RemoveTag(chatList, chatText, ref isFirst1);
                            WrightChat1(chatText);
                            break;
                        case 1:
                            RemoveTag(chatList, chatText, ref isFirst2);
                            WrightChat2(chatText);
                            break;
                        case 2:
                            RemoveTag(chatList, chatText, ref isFirst3);
                            WrightChat3(chatText);
                            break;
                    }
                }
            }
            
        }

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

        private void WrightChat1(string text)
        {
            if (ChatTxtBox1.InvokeRequired)
            {
                var ci = new ControlInvoker(WrightChat1);
                this.BeginInvoke(ci, text);
            }
            else
            {
                ChatTxtBox1.Text = text;
            }
        }

        private void WrightChat2(string text)
        {
            if (ChatTxtBox2.InvokeRequired)
            {
                var ci = new ControlInvoker(WrightChat2);
                this.BeginInvoke(ci, text);
            }
            else
            {
                ChatTxtBox2.Text = text;
            }
        }

        private void WrightChat3(string text)
        {
            if (ChatTxtBox3.InvokeRequired)
            {
                var ci = new ControlInvoker(WrightChat3);
                this.BeginInvoke(ci, text);
            }
            else
            {
                ChatTxtBox3.Text = text;
            }
        }
    }
}
