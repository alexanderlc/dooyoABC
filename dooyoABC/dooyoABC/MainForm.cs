﻿using System;
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
using System.Threading;
namespace dooyoABC
{
    public partial class MainForm : Form
    {
        
        UserManager mUserManager;
        CheckCodeParser mParser = new CheckCodeParser();       
        String mLoginUrl = "http://sale.dooyo.cn/tuan/account/login.html";
        String mAccountUrl = "http://sale.dooyo.cn/tuan/account/myAccInfo.html?tradeId=toMyAccInfo";
        String mBuyURL = "http://sale.dooyo.cn/tuan/miao/orderMiao.html?tradeId=miaoSha";
        String mProductID = "SZ1080010400348";
        Dictionary<String ,BackgroundWorker> mWorkers;
        public MainForm()
        {
            InitializeComponent();
            mUserManager = new UserManager();
            mWorkers=new Dictionary<string,BackgroundWorker>();
        }
        private String CurrentPhone()
        {
            return mUserManager.getCurrentKey();
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
            this.webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
            this.backgroundWorkerLoadUsers.RunWorkerAsync();

        }
        private void startWorker(UserManager.User u)
        {

            if (mWorkers.ContainsKey(u._phone))
            {
                BackgroundWorker bw = mWorkers[u._phone];
                if (!bw.IsBusy)
                {
                    u._status = UserManager.STATUS_START;
                    bw.RunWorkerAsync(u);
                }
            }
            else
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                bw.WorkerReportsProgress = true;
                bw.WorkerSupportsCancellation = true;
                bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                mWorkers.Add(u._phone, bw);
                u._status = UserManager.STATUS_START;
                bw.RunWorkerAsync(u);
            }
        }
        private void stopWorker(UserManager.User u)
        {           
            if (mWorkers.ContainsKey(u._phone))
            {
                u._status = UserManager.STATUS_COOKIE_READY;                            
                BackgroundWorker bw = mWorkers[u._phone];
                bw.CancelAsync();                
            }           
        }
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            UserManager.User u = e.Argument as UserManager.User;
            u._msg = "开始...";
            int count = 0;
            
            while (u._status !=UserManager.STATUS_COOKIE_READY)
            {
                String msg =miaoshaWork(u);
                count = count + 1;
                u._msg = "开始..." + count+" "+msg;
                Thread.Sleep(100);
                bw.ReportProgress(1);
            }
        }
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ResetUserListView();
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int per = e.ProgressPercentage;
            ResetUserListView();
        }

        String miaoshaWork(UserManager.User u)
        {
            try
            {
                String msg = "";
                Encoding encoding = Encoding.GetEncoding("gb2312");
                CookieCollection cookieCollection = u._cookies;
                //获取验证码
                Random ra = new Random();
                String getCodeURL = "http://sale.dooyo.cn/tuan/code.html?tradeId=getMiaoShaCode&t=" + ra.Next();
                HttpWebResponse response = HttpWebResponseUtility.CreateGetHttpResponse(getCodeURL, null, null, cookieCollection);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    System.IO.Stream resStream = response.GetResponseStream();
                    System.Drawing.Image i = Image.FromStream(resStream);
                    String checkCode = mParser.parse(i);
                    //log("获取验证码：" + checkCode);

                    try
                    {
                        //验证验证码
                        String checkCodeURL = "http://sale.dooyo.cn/tuan/miao/miaoindex.html?tradeId=checkCode";
                        IDictionary<string, string> parameters = new Dictionary<string, string>();
                        parameters.Add("code", checkCode);
                        parameters.Add("product_id", mProductID);
                        HttpWebResponse response2 = HttpWebResponseUtility.CreatePostHttpResponse(
                            checkCodeURL, parameters, null, null, encoding, cookieCollection);
                        if (response2.StatusCode == HttpStatusCode.OK)
                        {
                            // log("验证验证码：" + checkCode);
                            System.IO.StreamReader sr = new System.IO.StreamReader(response2.GetResponseStream());
                            String content = sr.ReadToEnd(); //这里的content就是网页内容了 
                            sr.Close();
                            //log("验证验证码：" + content);
                            if (content.Contains("false"))
                            {
                                //验证失败
                                msg = "ER:验证码错误";
                            }
                            else
                            {
                                msg = "验证成功";
                                //提交form
                                String submitURL = "http://sale.dooyo.cn/tuan/miao/miaoConfirmOrder.html?tradeId=toMiaoOrder&product_id="
                                    + mProductID;
                                IDictionary<string, string> parameters2 = new Dictionary<string, string>();
                                parameters2.Add("code", checkCode);
                                parameters2.Add("vCode", checkCode);
                                HttpWebResponse response3 = HttpWebResponseUtility.CreatePostHttpResponse(
                                    submitURL, parameters2, null, null, encoding, cookieCollection);
                                if (response3.StatusCode == HttpStatusCode.OK)
                                {
                                    //log("提交form");
                                    System.IO.StreamReader sr3 = new System.IO.StreamReader(response3.GetResponseStream());
                                    String content3 = sr3.ReadToEnd(); //这里的content就是网页内容了 
                                    sr3.Close();
                                    if (content3.Contains("抱歉"))
                                    {
                                        msg = "很抱歉，提交失败啦";
                                    }
                                    else
                                    {
                                        //下单                                              
                                        IDictionary<string, string> buyParams = new Dictionary<string, string>();
                                        buyParams.Add("code", checkCode);
                                        buyParams.Add("vCode", checkCode);
                                        buyParams.Add("mobile", u._phone);
                                        buyParams.Add("payTypeRadio", "1");
                                        buyParams.Add("product_id", this.mProductID);
                                        buyParams.Add("product_props", "");
                                        buyParams.Add("product_type", "0");
                                        buyParams.Add("quantity", "1");
                                        HttpWebResponse responseBuy = HttpWebResponseUtility.CreatePostHttpResponse(
                                               mBuyURL, buyParams, null, null, encoding, cookieCollection);
                                        if (responseBuy.StatusCode == HttpStatusCode.OK)
                                        {
                                            //log("下单");
                                            System.IO.StreamReader srBuy = new System.IO.StreamReader(responseBuy.GetResponseStream());
                                            String contentBuy = srBuy.ReadToEnd(); //这里的content就是网页内容了 
                                            srBuy.Close();
                                            //成功啦
                                            if (contentBuy.Contains("抱歉") || contentBuy.Contains("未开始秒杀"))
                                            {
                                                msg = "很抱歉，下单失败啦";
                                            }
                                            else
                                            {
                                                msg = "快付款吧";
                                            }
                                        }
                                        else
                                        {
                                            //log("下单错误，返回code：" + responseBuy.StatusCode);
                                        }
                                    }//if(!content3.Contains("抱歉"))
                                }//if (response3.StatusCode == HttpStatusCode.OK)
                                else
                                {
                                    //log("提交form错误，返回code：" + response3.StatusCode);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        String str = ex.ToString();
                        str = "错误：" + str;
                    }
                }
                return msg;
            }
            catch (Exception ex)
            {
                return "出错了" + ex.ToString();
            }
        }
        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //mCurrentPhone
            try
            {
                if (webBrowser1.Url.ToString().StartsWith(mLoginUrl))
                {
                    if (!CurrentPhone().Equals(""))
                    {
                        dooyoABC.UserManager.User u = mUserManager.mMapUser[CurrentPhone()];

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
                else if (webBrowser1.Url.ToString().StartsWith(mAccountUrl))
                {
                    if (!CurrentPhone().Equals(""))
                    {
                        dooyoABC.UserManager.User u = mUserManager.mMapUser[CurrentPhone()];
                        String cookieStr = this.webBrowser1.Document.Cookie;
                        String[] cookstr = cookieStr.Split(';');                       
                        foreach (String str in cookstr)
                        {
                            string[] cookieNameValue = str.Split('=');
                            Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
                            ck.Domain = "sale.dooyo.cn";                        
                            u._cookies.Add(ck);
                        }
                        u._status = UserManager.STATUS_COOKIE_READY;                    
                        mUserManager.NextKey();
                        ResetUserListView();
                        this.webBrowser1.Navigate(mLoginUrl);
                    }//end if (!mCurrentPhone.Equals(""))
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void ResetUserListView()
        {
            this.listViewUser.Items.Clear();
            foreach (KeyValuePair<String, dooyoABC.UserManager.User> kv in mUserManager.mMapUser)
            {
                this.addUserToListView(kv.Value);
            }
        }
        private void addUserToListView(dooyoABC.UserManager.User u)
        {
            ListViewItem lvItem;
            ListViewItem.ListViewSubItem lvSubItem;
            lvItem = new ListViewItem();
            lvItem.Text = u._phone;

            lvSubItem = new ListViewItem.ListViewSubItem();
            String status="";
            if(u._status== UserManager.STATUS_INIT){
                status="初始化";
            }else if(u._status== UserManager.STATUS_COOKIE_READY){
                status="就绪";
            }else if(u._status== UserManager.STATUS_ONE){
                 status="抢到一个";
            }else if(u._status== UserManager.STATUS_TWO){
                 status="抢到两个";
            }
            else if (u._status == UserManager.STATUS_START)
            {
                status = "开始秒杀";
            }
            lvSubItem.Text = status;
            lvItem.SubItems.Add(lvSubItem);
            //密码
            lvSubItem = new ListViewItem.ListViewSubItem();
            lvSubItem.Text = "未获取";
            if (u._cookies.Count > 0)
            {
                Cookie ck = u._cookies["AUTO_LOGIN_PWD"];
                if(ck!=null){
                    lvSubItem.Text = ck.Value;
                }
            }
            lvItem.SubItems.Add(lvSubItem);
            //消息
            lvSubItem = new ListViewItem.ListViewSubItem();
            lvSubItem.Text = u._msg;
            lvItem.SubItems.Add(lvSubItem);
            lvItem.Tag =u;
            this.listViewUser.Items.Add(lvItem);
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
            this.mUserManager.ResetNext();
            ResetUserListView();
            this.webBrowser1.Navigate(mLoginUrl);
            
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // mWorkers
            foreach (KeyValuePair<String,UserManager.User> kv in mUserManager.mMapUser)
            {
                startWorker(kv.Value);
            }
        }

        private void StopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<String, UserManager.User> kv in mUserManager.mMapUser)
            {
                stopWorker(kv.Value);
            }
            ResetUserListView();
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetUserListView();
        }  
    }
}
