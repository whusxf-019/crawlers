using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace Crawler
{
    public partial class Form1 : Form
    {
        private string showTxt;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < 87; i++)
            {
                String url = "https:" + "//www.nowcoder.com/recommend-intern/list?token=&page=" + Convert.ToString(i) + "&address=北京";
                string str = GetHttpWebRequest(url);
                ArrayList List1 = new ArrayList();
                foreach (Match match in Regex.Matches(str, @"Id"":\d*"))
                {
                    List1.Add(match.Value.Substring(4, match.Value.Length - 4));
                }
                object[] id1 = (object[])List1.ToArray(typeof(object));

                ArrayList List2 = new ArrayList();
                foreach (Match match in Regex.Matches(str, @"""id"":\d*"))
                {
                    List2.Add(match.Value.Substring(5, match.Value.Length - 5));
                }
                object[] id2 = (object[])List2.ToArray(typeof(object));

                for (int j = 0; j < 10; j++)
                {
                    string URL = "https:" + "//www.nowcoder.com/recommend-intern/" + id1[j] + "?jobId=" + id2[j];
                    string information = GetHttpWebRequest(URL);
                    string txt = Regex.Match(information, @"<h2>(.+?)<").Groups[1].Value + "\r\n";
                    txt += GetInformation(URL, "//div [@class='rec-job']/dl");
                    txt += "公司名称：";
                    txt += GetInformation(URL, "//h3 [@class='teacher-name']");
                    txt += "工作地：";
                    txt += GetInformation(URL, "//div [@class='rec-info']/p[3]");
                    txt += GetInformation(URL, "//div [@class='com-detail']/p");
                    showTxt += txt;
                    StorageInformation("F:\\ADT\\Crawler\\data.txt", txt);
                }
            }
            for (int i = 1; i < 9; i++)
            {
                String url = "https:" + "//www.nowcoder.com/recommend-intern/list?token=&page=" + Convert.ToString(i) + "&address=上海";
                string str = GetHttpWebRequest(url);
                ArrayList List1 = new ArrayList();
                foreach (Match match in Regex.Matches(str, @"Id"":\d*"))
                {
                    List1.Add(match.Value.Substring(4, match.Value.Length - 4));
                }
                object[] id1 = (object[])List1.ToArray(typeof(object));

                ArrayList List2 = new ArrayList();
                foreach (Match match in Regex.Matches(str, @"""id"":\d*"))
                {
                    List2.Add(match.Value.Substring(5, match.Value.Length - 5));
                }
                object[] id2 = (object[])List2.ToArray(typeof(object));

                for (int j = 0; j < 10; j++)
                {
                    string URL = "https:" + "//www.nowcoder.com/recommend-intern/" + id1[j] + "?jobId=" + id2[j];
                    string information = GetHttpWebRequest(URL);
                    string txt = Regex.Match(information, @"<h2>(.+?)<").Groups[1].Value + "\r\n";
                    txt += GetInformation(URL, "//div [@class='rec-job']/dl");
                    txt += "公司名称：  ";
                    txt += GetInformation(URL, "//h3 [@class='teacher-name']");
                    txt += "工作地：  ";
                    txt += GetInformation(URL, "//div [@class='rec-info']/p[3]");
                    txt += GetInformation(URL, "//div [@class='com-detail']/p");
                    showTxt += txt;
                    StorageInformation("F:\\ADT\\Crawler\\data.txt", txt);
                }
            }
            MessageBox.Show("下载完成!");
        }

        //只显示10000个字符
        private void button2_Click(object sender, EventArgs e)
        {
            StreamReader str = new StreamReader("F:\\ADT\\Crawler\\data.txt", Encoding.Default);
            string line;
            int length = 0;
            int maxLength = 10000;
            while ((line = str.ReadLine()) != null)
            {
                length += line.ToString().Length;
                if (length <= maxLength)
                    textBox1.Text += line.ToString() + "\r\n";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] address = { "北京", "上海" };
            textBox1.Clear();
            for (int i = 0; i < 2; i++)
            {
                String url = "https:" + "//www.nowcoder.com/recommend-intern/list?token=&page=" + Convert.ToString(1) + "&address=" + address[i];
                string str = GetHttpWebRequest(url);
                ArrayList List1 = new ArrayList();
                foreach (Match match in Regex.Matches(str, @"Id"":\d*"))
                {
                    List1.Add(match.Value.Substring(4, match.Value.Length - 4));
                }
                object[] id1 = (object[])List1.ToArray(typeof(object));

                ArrayList List2 = new ArrayList();
                foreach (Match match in Regex.Matches(str, @"""id"":\d*"))
                {
                    List2.Add(match.Value.Substring(5, match.Value.Length - 5));
                }
                object[] id2 = (object[])List2.ToArray(typeof(object));
                for (int j = 0; j < 10; j++)
                {
                    string URL = "https:" + "//www.nowcoder.com/recommend-intern/" + id1[j] + "?jobId=" + id2[j];
                    string information = GetHttpWebRequest(URL);
                    string txt = Regex.Match(information, @"<h2>(.+?)<").Groups[1].Value + "\r\n";
                    txt += GetInformation(URL, "//div [@class='rec-job']/dl");
                    txt += "公司名称：  ";
                    txt += GetInformation(URL, "//h3 [@class='teacher-name']");
                    txt += "工作地：  ";
                    txt += GetInformation(URL, "//div [@class='rec-info']/p[3]");
                    txt += GetInformation(URL, "//div [@class='com-detail']/p");
                    if (!showTxt.Contains(txt))
                    {                  
                        textBox1.Text += txt;
                    }
                }
            }
        }

        //爬取网页内容，源码网址：https://www.cnblogs.com/Agui520/p/5698996.html
        private string GetHttpWebRequest(string url)
        {
            HttpWebResponse result;
            string strHTML = string.Empty;
            try
            {
                Uri uri = new Uri(url);
                WebRequest webReq = WebRequest.Create(uri);
                WebResponse webRes = webReq.GetResponse();

                HttpWebRequest myReq = (HttpWebRequest)webReq;
                myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
                myReq.Accept = "*/*";
                myReq.KeepAlive = true;
                myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                result = (HttpWebResponse)myReq.GetResponse();
                Stream receviceStream = result.GetResponseStream();
                StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
                strHTML = readerOfStream.ReadToEnd();
                readerOfStream.Close();
                receviceStream.Close();
                result.Close();
            }
            catch
            {
                Uri uri = new Uri(url);
                WebRequest webReq = WebRequest.Create(uri);
                HttpWebRequest myReq = (HttpWebRequest)webReq;
                myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
                myReq.Accept = "*/*";
                myReq.KeepAlive = true;
                myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                //result = (HttpWebResponse)myReq.GetResponse();  
                try
                {
                    result = (HttpWebResponse)myReq.GetResponse();
                }
                catch (WebException ex)
                {
                    result = (HttpWebResponse)ex.Response;
                }
                Stream receviceStream = result.GetResponseStream();
                StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("gb2312"));
                strHTML = readerOfStream.ReadToEnd();
                readerOfStream.Close();
                receviceStream.Close();
                result.Close();
            }
            return strHTML;
        }

        private string GetInformation(string URL, string expression)
        {
            string showMessage = null;
            HtmlWeb WebClient = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = WebClient.Load(URL);
            HtmlNodeCollection nodes = null;
            nodes = doc.DocumentNode.SelectNodes(expression);
            foreach (HtmlNode node in nodes)
            {
                showMessage += string.Format("{0}\r\n", node.InnerText);
            }
            return showMessage;
        }

        private void StorageInformation(String m, string n)
        {
            if (!File.Exists(m))
            {
                StreamWriter strmsave = new StreamWriter(m, true, Encoding.Default);
                strmsave.Write(n);
                strmsave.Close();
            }
            else
            {
                StreamWriter strmsave = new StreamWriter(m, true, Encoding.Default);
                strmsave.Write(n);
                strmsave.Close();
            }
        }

    }
}
