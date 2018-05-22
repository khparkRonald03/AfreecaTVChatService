using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebController
{
    public class BaseController
    {
        public ChromeDriverService Service = null;

        public ChromeOptions Options = null;

        public IWebDriver Driver;

        private bool IsSilentMode { get; set; } = true;

        /// <summary>
        /// 생성자 : 크롬 브라우저 보이도록 초기화
        /// </summary>
        public BaseController()
        {
            Service = ChromeDriverService.CreateDefaultService();
            Service.HideCommandPromptWindow = true;
        }

        /// <summary>
        /// 생성자 : 크롬 브라우저 보이기/안보이기 설정 후 초기화
        /// </summary>
        /// <param name="isSilentMode"></param>
        public BaseController(bool isSilentMode)
        {
            IsSilentMode = isSilentMode;
            if (isSilentMode)
            {
                Options = new ChromeOptions();
                Options.AddArgument("headless");

                Service = ChromeDriverService.CreateDefaultService();
                Service.HideCommandPromptWindow = true;
            }
            else
            {
                Service = ChromeDriverService.CreateDefaultService();
                Service.HideCommandPromptWindow = true;
            }
        }

        /// <summary>
        /// 소멸자 : 드라이버 종료 및 해제
        /// </summary>
        ~BaseController()
        {
            Close();
        }

        #region 기본기능

        /// <summary>
        /// 드라이브 시작
        /// </summary>
        /// <returns></returns>
        public ResultModel<bool> Start()
        {
            var result = new ResultModel<bool>();
            try
            {
                if (IsSilentMode)
                    Driver = new ChromeDriver(Service, Options);
                else
                    Driver = new ChromeDriver(Service);

                result.ResultValue = true;
            }
            catch (Exception e)
            {
                string err = $"오류가 발생하였습니다. [{e.Message}]";
                result.ResultValue = false;
                result.Err = err;
            }

            return result;
        }

        /// <summary>
        /// 드라이브 종료
        /// </summary>
        /// <returns></returns>
        public ResultModel<bool> Close()
        {
            var result = new ResultModel<bool>();
            if (Driver != null)
            {
                try
                {
                    Driver.Close();
                    Driver.Dispose();
                    Driver = null;
                }
                catch (Exception e)
                {
                    string err = $"오류가 발생하였습니다. [{e.Message}]";
                    result.ResultValue = false;
                    result.Err = err;
                }
            }

            return result;
        }

        /// <summary>
        /// 싱글 엘레멘트 가져오기
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public IWebElement FindElement(ElementsSelectType type, string name)
        {
            try
            {
                Driver.WaitForAjax();
                var by = GetBy(type, name);
                var webElement = Driver.FindElement(by);

                return webElement;
            }
            catch
            {
                return null;
            }
        }
        
        /// <summary>
        /// 멀티 엘레먼트 가져오기
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ReadOnlyCollection<IWebElement> FindElements(ElementsSelectType type, string name)
        {
            try
            {
                Driver.WaitForAjax();
                var by = GetBy(type, name);
                var webElements = Driver.FindElements(by);

                return webElements;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 엘레멘트의 하위 멀티 엘레멘트 가져오기
        /// </summary>
        /// <param name="element"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ReadOnlyCollection<IWebElement> FindDefailElements(IWebElement element, ElementsSelectType type, string name)
        {
            try
            {
                Driver.WaitForAjax();
                var by = GetBy(type, name);
                var webElements = element.FindElements(by);

                return webElements;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 엘레멘트의 하위 싱글 엘레멘트 가져오기
        /// </summary>
        /// <param name="element"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public IWebElement FindDefailElement(IWebElement element, ElementsSelectType type, string name)
        {
            try
            {
                Driver.WaitForAjax();
                var by = GetBy(type, name);
                var webElement = element.FindElement(by);

                return webElement;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 탭 이동
        /// </summary>
        /// <param name="winHandle"></param>
        /// <returns></returns>
        public ResultModel<bool> MoveTab(string winHandle)
        {
            var result = new ResultModel<bool>();
            try
            {
                Driver.SwitchTo().Window(winHandle);
                Thread.Sleep(500);

                result.ResultValue = true;
            }
            catch (Exception e)
            {
                result.ResultValue = false;
                result.Err = $"탭 이동 중 오류가 발생하였습니다. : [{e.Message}]";
            }

            return result;
        }
        

        /// <summary>
        /// 자바스크립트 실행
        /// </summary>
        /// <param name="javascript"></param>
        /// <returns></returns>
        public ResultModel<bool> ExecuteJS(string javascript, params object[] args)
        {
            // ex1) ->  ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 150)"); // 스크롤 이동
            // ex2) -> SearchListPage.GoPage(0)
            // (자바스크립트 코드 중 javascript: 는 제외해야 함)

            var result = new ResultModel<bool>();
            try
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript(javascript, args);
                Driver.WaitForAjax();
                result.ResultValue = true;
            }
            catch (Exception e)
            {
                result.ResultValue = false;
                result.Err = $"오류가 발생하였습니다. : [{e.Message}]";
            }

            return result;
        }

        /// <summary>
        /// 자바스크립트 실행 후 HTML 리턴
        /// </summary>
        /// <param name="javascript"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public ResultModel<string> ExecuteJSReturnHtml(string javascript, params object[] args)
        {
            // ex1) ->  ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 150)"); // 스크롤 이동
            // ex2) -> SearchListPage.GoPage(0)
            // (자바스크립트 코드 중 javascript: 는 제외해야 함)

            var result = new ResultModel<string>();
            try
            {
                var resultObject = ((IJavaScriptExecutor)Driver).ExecuteScript(javascript, args);
                result.ResultValue = resultObject.ToString();
                Driver.WaitForAjax();
            }
            catch (Exception e)
            {
                result.ResultValue = string.Empty;
                result.Err = $"오류가 발생하였습니다. : [{e.Message}]";
            }

            return result;
        }

        /// <summary>
        /// 선택자 객체 가져오기
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public By GetBy(ElementsSelectType type, string name)
        {
            switch (type.ToString())
            {
                case "ClassName":
                    return By.ClassName(name);
                case "CssSelector":
                    return By.CssSelector(name);
                case "Id":
                    return By.Id(name);
                case "LinkText":
                    return By.LinkText(name);
                case "Name":
                    return By.Name(name);
                case "PartialLinkText":
                    return By.PartialLinkText(name);
                case "TagName":
                    return By.TagName(name);
                case "XPath":
                    return By.XPath(name);
                default:
                    return null;
            }
        }

        #endregion

    }
}
