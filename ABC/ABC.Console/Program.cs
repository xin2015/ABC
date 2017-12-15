using ABC.BLL.DataCenterService;
using ABC.BLL.Helpers;
using ABC.BLL.Neuro;
using Ivony.Html;
using Ivony.Html.Parser;
using SufeiUtil;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Console
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    HttpHelper hh = new HttpHelper();
        //    HttpItem hi = new HttpItem()
        //    {
        //        URL = "http://106.37.198.82:81/Home/Index?UserAccount=denggq&Password=123456",
        //        Method = "post",
        //        UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:55.0) Gecko/20100101 Firefox/55.0",
        //    };
        //    HttpResult hr = hh.GetHtml(hi);
        //    hi.Cookie = hr.Cookie;
        //    hi.URL = "http://106.37.198.82:81/DataDistribution/DataPackageInfoList?DataLevel=3002&ProvinceCode=430000&CityCode=430100";
        //    hr = hh.GetHtml(hi);
        //    JumonyParser jp = new JumonyParser();
        //    IHtmlDocument html = jp.Parse(hr.Html);
        //    if (html != null)
        //    {
        //        IHtmlElement tr = html.FindFirst("tbody>tr");
        //        foreach (IHtmlElement td in tr.Elements())
        //        {
        //            string text = td.InnerText();
        //            if (text.Contains(".zip"))
        //            {
        //                string path = "E:\\";
        //                using (WebClient wc = new WebClient())
        //                {
        //                    wc.Encoding = Encoding.UTF8;
        //                    wc.Headers.Add(HttpRequestHeader.Cookie, "ASP.NET_SessionId=eusvjecbutkhdzlwrldpabji");
        //                    wc.BaseAddress = "http://106.37.198.82:81";
        //                    string address = "/TransferSource/430100/TransferPackage/" + text;
        //                    string fileName = path + text;
        //                    Task task = wc.DownloadFileTaskAsync(address, fileName);
        //                    task.Wait();
        //                }
        //            }
        //        }
        //    }
        //    //int start = hr.Html.IndexOf("downLoad(");
        //    //int end = hr.Html.IndexOf(")", start);
        //    //string id = hr.Html.Remove(0, start + 9);
        //    //id = id.Remove(end - start - 9);
        //    //hi.URL = "http://106.37.198.82:81/DataDistribution/FileExsit?id=" + id;
        //    //hr=hh.GetHtml(hi);
        //    using (WebClient wc = new WebClient())
        //    {
        //        wc.Encoding = Encoding.UTF8;
        //        wc.Headers.Add(HttpRequestHeader.Cookie, "ASP.NET_SessionId=eusvjecbutkhdzlwrldpabji");
        //        Task task = wc.DownloadFileTaskAsync("http://106.37.198.82:81/TransferSource/430100/TransferPackage/Z_ENV_EWFS_L4_430100_20170908081929_DBB_CNEMC_SH2_BK1_201709072000_008.zip", "E:\\temp.zip");

        //    }
        //}

        static void Main(string[] args)
        {
            DateTime startTime = new DateTime(2017, 1, 1);
            DateTime endTime = startTime.AddMonths(3);
            List<CityDayData> pollutantMonitorList = DataCenterServiceHelper.GetCityDayDataList(startTime, endTime);
            List<double> list = new List<double>();
            double value;
            foreach (CityDayData data in pollutantMonitorList)
            {
                if (double.TryParse(data.SO2_24h, out value))
                {
                    list.Add(value);
                }
            }
            BackPropagationLearningTimeSeriesModel model = new BackPropagationLearningTimeSeriesModel(list.ToArray(), 6, new int[] { 12, 6, 1 }, 5000, 10);
            model.Run();
        }
    }
}
