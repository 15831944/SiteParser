using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
namespace ConsoleApplication3
{
    class CSite
    {
        string Url;
        string ParseStr;
        string DeletedSubstr;
        public CSite(string _Url, string _ParseStr = "", string _DeletedSubstr = "")
        {
            this.Url = _Url;
            this.ParseStr = _ParseStr;
            this.DeletedSubstr = _DeletedSubstr;
        }
        public void CheckPrice()
        {
            WebRequest webReq = WebRequest.Create(Url);
            WebResponse webRes = webReq.GetResponse();
            Stream st = webRes.GetResponseStream();
            StreamReader sr = new StreamReader(st);
            string str = sr.ReadToEnd();
            sr.Close();

            StreamWriter sw = new StreamWriter("data.html");
            sw.WriteLine(str);
            sw.Close();

            Regex regex = new Regex(@ParseStr);
            MatchCollection matches = regex.Matches(str);
            if (matches.Count != 0)
            {
                double price = Convert.ToDouble((matches[0].ToString().Replace(DeletedSubstr, "")).Trim());
                if (price > 4000)
                    System.Windows.Forms.MessageBox.Show(("Сегодня низкая цена на полку " + price.ToString() + " по адресу: " + Url.Split('/')[2]));
            }

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            CSite skoroto = new CSite("http://skoroto.ru/search?pcode=849069U000", "resultPrice \">\\n(\\t*)(.*) ", "resultPrice \">");
            skoroto.CheckPrice();
            CSite Zap7 = new CSite("http://www.7zap.ru/search.html?article=849069U000&sort___search_results_by=final_price&x=0&y=0&g=1&smode=A2&term=", @"<nobr>. ...", "<nobr>");
            Zap7.CheckPrice();
            //CSite emex = new CSite("http://www.emex.ru/f?detailNum=849069U000&packet=-1");
            //emex.CheckPrice();
            CSite autodoc = new CSite("http://www.autodoc.ru/Price/Index?Article=849069U000", "....,..</span>", "</span>");
            autodoc.CheckPrice();
        }
    }
}