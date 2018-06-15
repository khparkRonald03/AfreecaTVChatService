using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using System.Threading;

namespace WebController
{
    public class Controller : BaseController
    {
        public string Url { get; private set; }

        public bool IsSilentMode { get; private set; } = false;


        /// <summary>
        /// 생성자 : 크롬 브라우저 보이도록 초기화
        /// </summary>
        public Controller()
            : base()
        {   
        }

        /// <summary>
        /// 생성자 : 크롬 브라우저 보이지 않게 초기화
        /// </summary>
        /// <param name="isSilentMode"></param>
        public Controller(bool isSilentMode, string startupPath)
            : base(isSilentMode, startupPath)
        {
            IsSilentMode = isSilentMode;
        }

        /// <summary>
        /// url 초기화
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ResultModel<bool> SetUrl(string url)
        {
            var result = new ResultModel<bool>();
            try
            {
                Url = url;
                Driver.Url = Url;

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
        /// Url 가져오기
        /// </summary>
        /// <returns></returns>
        public ResultModel<string> GetUrl()
        {
            var result = new ResultModel<string>();
            try
            {
                result.ResultValue = Driver.Url;
            }
            catch (Exception e)
            {
                result.ResultValue = string.Empty;
                result.Err = $"오류가 발생하였습니다. : [{e.Message}]";
            }

            return result;
        }

        /// <summary>
        /// 사용할 태그가 있는지 확인 (단일 선택자 용)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="selectText"></param>
        /// <returns></returns>
        public ResultModel<bool> ExistsElement(ElementsSelectType type, string selectText)
        {
            var result = new ResultModel<bool>();
            try
            {
                var tag = FindElement(type, selectText);
                result.ResultValue = tag == null ? false : true;
            }
            catch (Exception e)
            {
                result.ResultValue = false;
                result.Err = $"오류가 발생하였습니다. : [{e.Message}]";
            }

            return result;
        }

        /// <summary>
        /// 사용할 태그가 있는지 확인 (다중 선택자 용)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="selectText"></param>
        /// <returns></returns>
        public ResultModel<bool> ExistsElements(ElementsSelectType type, string selectText)
        {
            var result = new ResultModel<bool>();
            try
            {
                var tags = FindElements(type, selectText);
                result.ResultValue = tags == null && tags.Count < 1 ? false : true;
            }
            catch (Exception e)
            {
                result.ResultValue = false;
                result.Err = $"오류가 발생하였습니다. : [{e.Message}]";
            }

            return result;
        }

        /// <summary>
        /// 현재 페이지 소스 가져오기
        /// </summary>
        /// <returns></returns>
        public ResultModel<string> GetPageSource()
        {
            var result = new ResultModel<string>();
            try
            {
                Driver.WaitForAjax();
                result.ResultValue = Driver.PageSource;
            }
            catch (Exception e)
            {
                result.ResultValue = string.Empty;
                result.Err = $"오류가 발생하였습니다. : [{e.Message}]";
            }

            return result;
        }

        /// <summary>
        /// 태그의 text 값 가져오기
        /// </summary>
        /// <param name="type"></param>
        /// <param name="selectText"></param>
        /// <param name="tagText"></param>
        /// <returns></returns>
        public ResultModel<string> GetTagText(ElementsSelectType type, string selectText)
        {
            var result = new ResultModel<string>();
            try
            {
                var tag = FindElement(type, selectText);
                result.ResultValue = tag.Text;
            }
            catch (Exception e)
            {
                result.ResultValue = string.Empty;
                result.Err = $"오류가 발생하였습니다. : [{e.Message}]";
            }

            return result;
        }

        /// <summary>
        /// input value값 가져오기
        /// </summary>
        /// <param name="type"></param>
        /// <param name="selectText"></param>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public ResultModel<string> GetValueInputTag(ElementsSelectType type, string selectText)
        {
            var result = new ResultModel<string>();
            try
            {
                var input = FindElement(type, selectText);
                result.ResultValue = input.GetAttribute("value");
                
            }
            catch (Exception e)
            {
                result.ResultValue = string.Empty;
                result.Err = $"오류가 발생하였습니다. : [{e.Message}]";
            }

            return result;
        }

        /// <summary>
        /// input에 텍스트 넣기
        /// </summary>
        /// <param name="type"></param>
        /// <param name="selectText"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public ResultModel<bool> SetTextInputTag(ElementsSelectType type, string selectText, string text)
        {
            var result = new ResultModel<bool>();
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    result.ResultValue = false;
                    result.Err = "입력 값이 없습니다.";
                    return result;
                }
                
                var input = FindElement(type, selectText);
                if (input == null)
                {
                    result.ResultValue = false;
                    result.Err = $"다음과 일치하는 태그가 없습니다. [{selectText}]";
                    return result;
                }

                input.Clear();
                input.SendKeys(text);

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
        /// 클릭 동작 수행 (버튼, 테이블, 태그 등)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="selectText"></param>
        /// <returns></returns>
        public ResultModel<bool> ClickTag(ElementsSelectType type, string selectText)
        {
            var result = new ResultModel<bool>();
            try
            {
                var element = FindElement(type, selectText);
                if (element == null)
                {
                    result.ResultValue = false;
                    result.Err = $"다음과 일치하는 태그가 없습니다. [{selectText}]";
                    return result;
                }

                for (int i = 0; i <= 10; i++)
                {
                    try
                    {
                        Thread.Sleep(200);
                        element.Click();
                    }
                    catch (Exception e)
                    {
                        if (i == 10)
                        {
                            result.ResultValue = false;
                            result.Err = $"오류가 발생하였습니다. : [{e.Message}]";
                            return result;
                        }

                        continue;
                    }

                    break;
                }

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
        /// 태그 더블클릭
        /// </summary>
        /// <param name="type"></param>
        /// <param name="selectText"></param>
        /// <returns></returns>
        public ResultModel<bool> DoubleClickTag(ElementsSelectType type, string selectText)
        {
            var result = new ResultModel<bool>();
            try
            {
                var element = FindElement(type, selectText);
                if (element == null)
                {
                    result.ResultValue = false;
                    result.Err = $"다음과 일치하는 태그가 없습니다. [{selectText}]";
                    return result;
                }

                element.Click();

                var actions = new Actions(Driver);
                actions.DoubleClick(element).Perform();

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
        /// alert 클릭
        /// </summary>
        /// <param name="alertText">alert 메시지 텍스트</param>
        /// <returns></returns>
        public ResultModel<bool> ClickAlert(out string alertText)
        {
            var result = new ResultModel<bool>();
            try
            {
                if (Driver.SwitchTo().Alert() == null)
                {
                    alertText = string.Empty;
                    result.ResultValue = false;
                    result.Err = $"alert닫기 동작 중지 : 화면에 표시 된 alert 메시지가 없습니다.";
                }

                alertText = Driver.SwitchTo().Alert().Text;
                Driver.SwitchTo().Alert().Accept();

                result.ResultValue = true;
            }
            catch (Exception e)
            {
                alertText = string.Empty;
                result.ResultValue = false;
                result.Err = $"alert 닫기 동작 수행중 오류가 발생하였습니다. : [{e.Message}]";
            }

            return result;
        }

        public ResultModel<bool> NewTabOpen(string url)
        {
            var result = new ResultModel<bool>();
            try
            {
                var windowHandles = Driver.WindowHandles;
                ((IJavaScriptExecutor)Driver).ExecuteScript(string.Format("window.open('{0}', '_blank');", url));
                Driver.WaitForAjax();
                var newWindowHandles = Driver.WindowHandles;
                var openedWindowHandle = newWindowHandles.Except(windowHandles).Single();
                Thread.Sleep(500);
                Driver.SwitchTo().Window(openedWindowHandle);

                result.ResultValue = true;
            }
            catch (Exception e)
            {
                result.ResultValue = false;
                result.Err = $"새탭을 여는 동작 수행중 오류가 발생하였습니다. : [{e.Message}]";
            }

            return result;
        }

        /// <summary>
        /// 종료
        /// </summary>
        public ResultModel<bool> CloseDriver()
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
                catch { }
            }

            return new ResultModel<bool>()
            {
                ResultValue = true
            };
        }
    }
}
