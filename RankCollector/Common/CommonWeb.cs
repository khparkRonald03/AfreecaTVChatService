using DataModels;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace RankCollector
{
    public class CommonWeb
    {
        protected CategoryRank GetCategoryRank(RankingType rankingType, int page)
        {
            var encoding = new ASCIIEncoding();

            var postData = $"szWhich={rankingType.ToString()}&nPage={page.ToString()}&szGender=A";

            byte[] data = encoding.GetBytes(postData);

            var myRequest = (HttpWebRequest)WebRequest.Create("http://afevent2.afreecatv.com:8120/app/rank/api.php");
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;

            var newStream = myRequest.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            var response = myRequest.GetResponse();
            var responseStream = response.GetResponseStream();
            var responseReader = new StreamReader(responseStream);
            var result = responseReader.ReadToEnd();

            var categoryRank = JsonConvert.DeserializeObject<CategoryRank>(result);

            responseReader.Close();
            response.Close();

            return categoryRank;
        }

        protected string GetUserRank(string bjID, string url, bool isUTF8 = true, string cookie = "")
        {
            //3번시도
            WebClient client = new WebClient();
            client.Headers.Add("Accept", "*/*");
            client.Headers.Add("Referer", $"http://live.afreecatv.com:8079/app/index.cgi?szBjId={bjID}"); // bj아이디 들어감
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36");
            client.Headers.Add("Host", "live.afreecatv.com:8057");

            client.Headers.Add("Cookie", cookie);

            if (isUTF8)
                client.Encoding = Encoding.GetEncoding("utf-8");
            else
                client.Encoding = Encoding.GetEncoding("euc-kr");

            //3번 접속시도
            int tryCount = 0;
            string result = string.Empty;
            do
            {
                try
                {
                    tryCount++;
                    result = client.DownloadString(url);
                }
                catch (Exception ex)
                {
                    var log = ex.Message;
                    result = string.Empty;
                }
            }
            while (result == string.Empty && tryCount < 3);

            return result;
        }
    }
}
