using ABC.BLL.CryptoTransverters;
using ABC.BLL.DataCenterService;
using ABC.BLL.Helpers;
using ABC.BLL.Neuro;
using Ivony.Html;
using Ivony.Html.Parser;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
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

        //static void Main(string[] args)
        //{
        //    DateTime startTime = new DateTime(2017, 1, 1);
        //    DateTime endTime = startTime.AddMonths(3);
        //    List<CityDayData> pollutantMonitorList = DataCenterServiceHelper.GetCityDayDataList(startTime, endTime);
        //    List<double> list = new List<double>();
        //    double value;
        //    foreach (CityDayData data in pollutantMonitorList)
        //    {
        //        if (double.TryParse(data.AQI, out value))
        //        {
        //            list.Add(value);
        //        }
        //    }
        //    BackPropagationLearningTimeSeriesModel model = new BackPropagationLearningTimeSeriesModel(list.ToArray(), 6, new int[] { 12, 6, 1 }, 5000, 10);
        //    model.Run();
        //    System.Console.ReadLine();
        //}

        static void Main(string[] args)
        {
            //RSA密钥对的构造器 
            RsaKeyPairGenerator keyGenerator = new RsaKeyPairGenerator();

            //RSA密钥构造器的参数 
            RsaKeyGenerationParameters param = new RsaKeyGenerationParameters(
                Org.BouncyCastle.Math.BigInteger.ValueOf(3),
                new Org.BouncyCastle.Security.SecureRandom(),
                1024,   //密钥长度 
                25);
            //用参数初始化密钥构造器 
            keyGenerator.Init(param);
            //产生密钥对 
            AsymmetricCipherKeyPair keyPair = keyGenerator.GenerateKeyPair();
            //获取公钥和密钥 
            AsymmetricKeyParameter publicKey = keyPair.Public;
            AsymmetricKeyParameter privateKey = keyPair.Private;
            if (((RsaKeyParameters)publicKey).Modulus.BitLength < 1024)
            {
                System.Console.WriteLine("failed key generation (1024) length test");
            }
            //一个测试…………………… 
            //输入，十六进制的字符串，解码为byte[] 
            //string input = "4e6f77206973207468652074696d6520666f7220616c6c20676f6f64206d656e"; 
            //byte[] testData = Org.BouncyCastle.Utilities.Encoders.Hex.Decode(input);            
            string input = "popozh RSA test";
            byte[] testData = Encoding.UTF8.GetBytes(input);
            System.Console.WriteLine("明文:" + input + Environment.NewLine);
            //非对称加密算法，加解密用 
            IAsymmetricBlockCipher engine = new RsaEngine();
            //公钥加密 
            engine.Init(true, publicKey);
            try
            {
                testData = engine.ProcessBlock(testData, 0, testData.Length);
                System.Console.WriteLine("密文（base64编码）:" + Convert.ToBase64String(testData) + Environment.NewLine);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("failed - exception " + Environment.NewLine + ex.ToString());
            }
            //私钥解密 
            engine.Init(false, privateKey);
            try
            {
                testData = engine.ProcessBlock(testData, 0, testData.Length);

            }
            catch (Exception e)
            {
                System.Console.WriteLine("failed - exception " + e.ToString());
            }
            if (input.Equals(Encoding.UTF8.GetString(testData)))
            {
                System.Console.WriteLine("解密成功");
            }
            System.Console.Read();
        }
    }
}
