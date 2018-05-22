using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebController
{
    public static class ExtentionIWebDriver
    {
        /// <summary>
        /// Ajax 요청 기다리기
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="timeoutSecs"></param>
        /// <param name="throwException"></param>
        public static void WaitForAjax(this IWebDriver driver, int timeoutSecs = 10, bool throwException = false)
        {
            for (var i = 0; i < timeoutSecs; i++)
            {
                var ajaxIsComplete = (bool)(driver as IJavaScriptExecutor).ExecuteScript(" return jQuery.active == 0 ");
                if (ajaxIsComplete)
                    return;

                Thread.Sleep(1000);
            }
            if (throwException)
            {
                throw new Exception(" WebDriver timed out waiting for AJAX call to complete ");
            }
        }
    }
}
