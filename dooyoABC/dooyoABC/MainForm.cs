using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Utils;
using System.Text.RegularExpressions;
using System.Reflection;
using mshtml;
namespace dooyoABC
{
    public partial class MainForm : Form
    {
        UserManager mUserManager;
        CheckCodeParser mParser = new CheckCodeParser();
        CookieCollection mCookieCollection = new CookieCollection();
        String loginurl = "http://sale.dooyo.cn/tuan/account/login.html";
        String mCurrentPhone="18601196685";
        public MainForm()
        {
            InitializeComponent();
            mUserManager = new UserManager();
        }

        private void backgroundWorkerLoadUsers_DoWork(object sender, DoWorkEventArgs e)
        {
            String path = Application.StartupPath + "\\config.txt";          
            if (mUserManager.loadUsers(path))
            {      
                e.Result = "OK:读取成功";
            }
            else
            {
                e.Result = "ER:读取失败";
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate(loginurl);
            this.webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
            this.backgroundWorkerLoadUsers.RunWorkerAsync();

        }

        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //mCurrentPhone
            try
            {
                if (webBrowser1.Url.ToString().StartsWith("http://sale.dooyo.cn/tuan/account/login.html"))
                {
                    if (!mCurrentPhone.Equals(""))
                    {
                        dooyoABC.UserManager.User u = mUserManager.mMapUser[mCurrentPhone];

                        HtmlElement user_id = this.webBrowser1.Document.GetElementById("user_id");
                        HtmlElement web_login_pwd = this.webBrowser1.Document.GetElementById("web_login_pwd");
                        HtmlElement verify_code = this.webBrowser1.Document.GetElementById("verify_code");
                        HtmlElement imgHE = this.webBrowser1.Document.GetElementById("verifyCodeImg");
                        HtmlElement login_form = this.webBrowser1.Document.GetElementById("login_form");
                        
                        user_id.InnerText = u._phone;
                        web_login_pwd.InnerText = u._pwd;
                        Image numPic = GetWebImage(webBrowser1, imgHE); // 得到验证码图片                
                        String checkCode = mParser.parse(numPic);
                        verify_code.InnerText = checkCode;
                        user_id.InvokeMember("onblur");
                        web_login_pwd.InvokeMember("onblur");
                        verify_code.InvokeMember("onblur");
                        login_form.InvokeMember("submit");

                    }
                }
                else if (webBrowser1.Url.ToString().StartsWith("http://sale.dooyo.cn/tuan/account/myAccInfo.html?tradeId=toMyAccInfo"))
                {
                    if (!mCurrentPhone.Equals(""))
                    {
                        dooyoABC.UserManager.User u = mUserManager.mMapUser[mCurrentPhone];
                        string cookieStr = this.webBrowser1.Document.Cookie;
                        string[] cookstr = cookieStr.Split(';');
                        String name = "";
                        foreach (string str in cookstr)
                        {
                            string[] cookieNameValue = str.Split('=');
                            Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
                            ck.Domain = "sale.dooyo.cn";
                            if (ck.Name.Equals("AUTO_LOGIN_ID"))
                            {
                                this.Text = "用户：" + ck.Value;
                                name = "用户：" + ck.Value;
                            }
                            u._cookies.Add(ck);
                        }
                        u._status = UserManager.STATUS_COOKIE_READY;
                    }//end if (!mCurrentPhone.Equals(""))

                }

            }
            catch (Exception ex)
            {

            }


            //try
            //{

            //    if (webBrowser1.Url.ToString().StartsWith("http://sale.dooyo.cn/tuan/account/login.html"))
            //    {
            //        string cookieStr = this.webBrowser1.Document.Cookie;                   
            //        string[] cookstr = cookieStr.Split(';');
            //        String name = "";
            //        foreach (string str in cookstr)
            //        {
            //            string[] cookieNameValue = str.Split('=');
            //            Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
            //            ck.Domain = "sale.dooyo.cn";
            //            if (ck.Name.Equals("AUTO_LOGIN_ID"))
            //            {
            //                this.Text = "用户：" + ck.Value;
            //                name = "用户：" + ck.Value;                           
            //            }
            //            mCookieCollection.Add(ck);
            //        }
            //        HtmlElement imgHE = this.webBrowser1.Document.GetElementById("verifyCodeImg");
            //        Image numPic = GetWebImage(webBrowser1, imgHE); // 得到验证码图片                
            //        String checkCode = mParser.parse(numPic);
            //        HtmlElement verify_code = this.webBrowser1.Document.GetElementById("verify_code");
            //        verify_code.InnerText = checkCode;


            //    }                
            //}
            //catch (Exception ex)
            //{
            //    String er = ex.ToString();
            //}
        }
           
       
        /// <summary>
        /// 返回指定WebBrowser中图片<IMG></IMG>中的图内容
        /// </summary>
        /// <param name="WebCtl">WebBrowser控件</param>
        /// <param name="ImgeTag">IMG元素</param>
        /// <returns>IMG对象</returns>
        private Image GetWebImage(WebBrowser WebCtl, HtmlElement ImgeTag)
        {
            HTMLDocument doc = (HTMLDocument)WebCtl.Document.DomDocument;
            HTMLBody body = (HTMLBody)doc.body;
            IHTMLControlRange rang = (IHTMLControlRange)body.createControlRange();
            IHTMLControlElement Img = (IHTMLControlElement)ImgeTag.DomElement; //图片地址

            Image oldImage = Clipboard.GetImage();
            rang.add(Img);
            rang.execCommand("Copy", false, null);  //拷贝到内存
            Image numImage = Clipboard.GetImage();
            try
            {
                Clipboard.SetImage(oldImage);
            }
            catch
            {
            }

            return numImage;
        }
        private void backgroundWorkerLoadUsers_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorkerLoadUsers_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
        //private void getCookies(dooyoABC.UserManager.User u)
        //{
        //    //访问登录界面获取cookies
        //    HttpWebResponse response = HttpWebResponseUtility.CreateGetHttpResponse(loginurl, null, null, null);
        //    if (response.StatusCode == HttpStatusCode.OK)
        //    {
        //        if (response.Headers["Set-Cookie"] != null)
        //        {
        //            String setCookie = response.Headers["Set-Cookie"];
        //            String jseesionid = setCookie.Replace("; path=/", "").Replace("JSESSIONID=", "");
        //            u._cookies = mCookieCollection;
        //            mUserManager.AddCookie(u._phone, "JSESSIONID", jseesionid);
        //        }
        //    }
        //    //登录
        //    u._cookies = mCookieCollection;
        //    Random ra = new Random();
        //    String getCodeURL = "http://sale.dooyo.cn/tuan/code.html?tradeId=getMiaoShaCode&t=" + ra.Next();
        //    HttpWebResponse responseCode = HttpWebResponseUtility.CreateGetHttpResponse(getCodeURL, null, mUserAgent, u._cookies);
        //    if (responseCode.StatusCode == HttpStatusCode.OK)
        //    {
        //        //获取验证码
        //        System.IO.Stream resStream = responseCode.GetResponseStream();
        //        System.Drawing.Image i = Image.FromStream(resStream);
        //        String checkCode = mParser.parse(i);
        //        //登录
        //        IDictionary<string, string> parameters = new Dictionary<string, string>();
        //        parameters.Add("autoLogin", "false");
        //        parameters.Add("tradeId", "webLogin");
        //        parameters.Add("user_id", u._phone);
        //        parameters.Add("verify_code", checkCode);
        //        parameters.Add("web_login_pwd", u._pwd);
        //        HttpWebResponse responseLogin = HttpWebResponseUtility.CreatePostHttpResponse(
        //            loginurl,
        //            parameters,
        //            null,
        //           mUserAgent,
        //            Encoding.Default,
        //            u._cookies);
        //        Console.WriteLine(checkCode);
        //        if (responseLogin.StatusCode == HttpStatusCode.OK)
        //        {
        //            if (responseLogin.Headers["Set-Cookie"] != null)
        //            {
        //                String loginCookie = responseLogin.Headers["Set-Cookie"];
        //            }
        //            // u._cookies = responseLogin.Cookies;
        //            System.IO.StreamReader sr3 = new System.IO.StreamReader(responseLogin.GetResponseStream());
        //            String content3 = sr3.ReadToEnd(); //这里的content就是网页内容了 
        //            sr3.Close();
        //        }
        //        else
        //        {
        //            //登录联网失败
        //        }
        //    }
        //    else
        //    {
        //        //获取验证码失败
        //    }
        //}
       

    }
}
