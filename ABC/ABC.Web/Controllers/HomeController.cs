using ABC.BLL.NPOI;
using ABC.BLL.Tabulation;
using SufeiUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABC.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            LoginUser user = Session["CurrentUser"] as LoginUser;
            if (user == null)
            {
                HttpCookie cookie = Request.Cookies["ABCUser"];
                if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                {
                    user = new LoginUser()
                    {
                        UserName = cookie.Values["UserName"],
                        Password = cookie.Values["Password"]
                    };
                }
                else
                {
                    user = new LoginUser()
                    {
                        UserName = "admin",
                        Password = "123456"
                    };
                    cookie = new HttpCookie("ABCUser");
                    cookie.Values["UserName"] = user.UserName;
                    cookie.Values["Password"] = user.Password;
                    Response.Cookies.Add(cookie);
                }
                Session["CurrentUser"] = user;
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public FileResult ExportDayAQIReportExcel(int count)
        {
            Table table = new Table();
            #region thead
            TRow tr = table.Thead.AddTr();
            tr.AddTh(DateTime.Today.ToString("yyyy年MM月dd日"), 1, 21);
            tr = table.Thead.AddTr();
            tr.AddTh("城市名称", 4, 1);
            tr.AddTh("监测点位名称", 4, 1);
            tr.AddTh("污染物浓度及空气质量分指数（IAQI）", 1, 14);
            tr.AddTh("空气质量指数（AQI）", 4, 1);
            tr.AddTh("首要污染物", 4, 1);
            tr.AddTh("空气质量指数级别", 4, 1);
            tr.AddTh("空气质量指数类别", 2, 2);
            tr = table.Thead.AddTr();
            tr.AddTh("二氧化硫（SO2）24小时平均", 2, 2);
            tr.AddTh("二氧化氮（NO2）24小时平均", 2, 2);
            tr.AddTh("颗粒物（粒径小于等于10μm）24小时平均", 2, 2);
            tr.AddTh("一氧化碳（CO）24小时平均", 2, 2);
            tr.AddTh("臭氧（O3）最大1小时平均", 2, 2);
            tr.AddTh("臭氧（O3）最大8小时滑动平均", 2, 2);
            tr.AddTh("颗粒物（粒径小于等于2.5μm）24小时平均", 2, 2);
            tr = table.Thead.AddTr();
            tr.AddTh("类别", 2, 1);
            tr.AddTh("颜色", 2, 1);
            tr = table.Thead.AddTr();
            for (int i = 0; i < 7; i++)
            {
                if (i == 3) tr.AddTh("浓度/（mg/m³）");
                else tr.AddTh("浓度/（μg/m³）");
                tr.AddTh("分指数");
            }
            #endregion
            #region tfoot
            tr = table.Tfoot.AddTr();
            tr.AddTd("注：缺测指标的浓度及分指数均使用NA标识。");
            #endregion
            #region tbody
            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                tr = table.Tbody.AddTr();
                tr.AddTd("广州");
                tr.AddTd("未知点位");
                tr.AddTd(rand.Next(100).ToString());
                tr.AddTd(rand.Next(50).ToString());
                tr.AddTd(rand.Next(100).ToString());
                tr.AddTd(rand.Next(50).ToString());
                tr.AddTd(rand.Next(100).ToString());
                tr.AddTd(rand.Next(50).ToString());
                tr.AddTd(rand.Next(10).ToString());
                tr.AddTd(rand.Next(50).ToString());
                tr.AddTd(rand.Next(100).ToString());
                tr.AddTd(rand.Next(50).ToString());
                tr.AddTd(rand.Next(100).ToString());
                tr.AddTd(rand.Next(50).ToString());
                tr.AddTd(rand.Next(100).ToString());
                tr.AddTd(rand.Next(50).ToString());
                tr.AddTd(rand.Next(50).ToString());
                tr.AddTd("NA");
                tr.AddTd("一级");
                tr.AddTd("优");
                tr.AddTd("绿色");
            }
            #endregion
            return File(ExcelHelper.Export(table, 2).ToArray(), "application / vnd.ms - excel", "空气质量指数日报.xlsx");
        }

        public FileResult ExportDayAQIReportWord(int count)
        {
            Table table = new Table();
            #region thead
            TRow tr = table.Thead.AddTr();
            tr.AddTh(DateTime.Today.ToString("yyyy年MM月dd日"), 1, 21);
            tr = table.Thead.AddTr();
            tr.AddTh("城市名称", 4, 1);
            tr.AddTh("监测点位名称", 4, 1);
            tr.AddTh("污染物浓度及空气质量分指数（IAQI）", 1, 14);
            tr.AddTh("空气质量指数（AQI）", 4, 1);
            tr.AddTh("首要污染物", 4, 1);
            tr.AddTh("空气质量指数级别", 4, 1);
            tr.AddTh("空气质量指数类别", 2, 2);
            tr = table.Thead.AddTr();
            tr.AddTh("二氧化硫（SO2）24小时平均", 2, 2);
            tr.AddTh("二氧化氮（NO2）24小时平均", 2, 2);
            tr.AddTh("颗粒物（粒径小于等于10μm）24小时平均", 2, 2);
            tr.AddTh("一氧化碳（CO）24小时平均", 2, 2);
            tr.AddTh("臭氧（O3）最大1小时平均", 2, 2);
            tr.AddTh("臭氧（O3）最大8小时滑动平均", 2, 2);
            tr.AddTh("颗粒物（粒径小于等于2.5μm）24小时平均", 2, 2);
            tr = table.Thead.AddTr();
            tr.AddTh("类别", 2, 1);
            tr.AddTh("颜色", 2, 1);
            tr = table.Thead.AddTr();
            for (int i = 0; i < 7; i++)
            {
                if (i == 3) tr.AddTh("浓度/（mg/m³）");
                else tr.AddTh("浓度/（μg/m³）");
                tr.AddTh("分指数");
            }
            #endregion
            #region tfoot
            tr = table.Tfoot.AddTr();
            tr.AddTd("注：缺测指标的浓度及分指数均使用NA标识。");
            #endregion
            #region tbody
            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                tr = table.Tbody.AddTr();
                tr.AddTd("广州");
                tr.AddTd("未知点位");
                tr.AddTd(rand.Next(100).ToString());
                tr.AddTd(rand.Next(50).ToString());
                tr.AddTd(rand.Next(100).ToString());
                tr.AddTd(rand.Next(50).ToString());
                tr.AddTd(rand.Next(100).ToString());
                tr.AddTd(rand.Next(50).ToString());
                tr.AddTd(rand.Next(10).ToString());
                tr.AddTd(rand.Next(50).ToString());
                tr.AddTd(rand.Next(100).ToString());
                tr.AddTd(rand.Next(50).ToString());
                tr.AddTd(rand.Next(100).ToString());
                tr.AddTd(rand.Next(50).ToString());
                tr.AddTd(rand.Next(100).ToString());
                tr.AddTd(rand.Next(50).ToString());
                tr.AddTd(rand.Next(50).ToString());
                tr.AddTd("NA");
                tr.AddTd("一级");
                tr.AddTd("优");
                tr.AddTd("绿色");
            }
            #endregion
            return File(WordHelper.Export(table).ToArray(), "application / vnd.ms - word", "空气质量指数日报.docx");
        }

        public ActionResult Roomba()
        {
            return View();
        }

        public ActionResult Table()
        {
            return View();
        }

        public ActionResult RoombaWhole(int? level)
        {
            //HttpHelper hh = new HttpHelper();
            //HttpItem hi = new HttpItem()
            //{
            //    URL = level == null ? "http://www.qlcoder.com/train/autocr" : string.Format("http://www.qlcoder.com/train/autocr?level=", level),
            //    Cookie = "laravel_session=eyJpdiI6IlwvRG5neDVtN1lOSGh2WU82dldoNXZ3PT0iLCJ2YWx1ZSI6ImFLV2tSKzJ4Vlc2RWlCWHltOWU1bHpOQUdZalwvWDNTcSt3UGhlT0lQVWdMMGNLcFROekE0T256UHBZekZRS3QwVFd1MEVic2swaTJ0ZEgwMFpGZUxKQT09IiwibWFjIjoiOGQ3MDkwNmVhYmQ0ZTU0NDQ3ODAxNDQ0YzVkYzg4ZTBiMWE5NGIxNzliYjkyMGUzMmE3NGE3YjQ0NWRiNjJjYyJ9"
            //};
            //HttpResult hr = hh.GetHtml(hi);
            //string html = hr.Html;
            //html = html.Substring(html.IndexOf("level="));
            //html = html.Substring(0, html.IndexOf("<br>"));
            //string[] paramsArray = html.Split('&');
            //level = int.Parse(paramsArray[0].Replace("level=", string.Empty));
            //int x = int.Parse(paramsArray[1].Replace("x=", string.Empty));
            //int y = int.Parse(paramsArray[2].Replace("y=", string.Empty));
            //string mapStr = paramsArray[3].Replace("map=", string.Empty);
            //int X = x + 2;
            //int Y = y + 2;
            //Stack<char> mapArray = new Stack<char>(mapStr);
            //int[][] map = new int[X][];
            //for (int i = x; i > 0; i--)
            //{
            //    map[i] = new int[Y];
            //    for (int j = y; j > 0; j--)
            //    {
            //        if (mapArray.Pop() == '0')
            //        {
            //            map[i][j] = true;
            //            initRestPoints.Push(j);
            //            initRestPoints.Push(i);
            //        }
            //    }
            //}
            //map[0] = new int[Y];
            //map[X - 1] = new int[Y];
            return View();
        }
    }

    public class LoginUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}