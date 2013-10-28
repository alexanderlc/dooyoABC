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
using System.Threading;
using System.IO;
using System.Configuration;
using System.Diagnostics;
namespace dooyoABC
{
    public partial class MainForm : Form
    {

        public volatile String mStr;
        static private string stringDateFormat = "yyyy-MM-dd HH:mm:ss";
        private StreamWriter logFileWriter;             //写日志


        UserManager mUserManager;

        String mLoginUrl = "http://sale.dooyo.cn/tuan/account/login.html";
        //String mAccountUrl = "http://sale.dooyo.cn/tuan/account/myAccInfo.html?tradeId=toMyAccInfo";
        String mBuyURL = "http://sale.dooyo.cn/tuan/miao/orderMiao.html?tradeId=miaoSha";
        ///tuan/index.html?tradeId=payOrder&order_id=080063620N
        String mURLMyOrder = "http://sale.dooyo.cn/tuan/account/myOrder.html?tradeId=queryAccOrderList";
        String mProductID = "SZ1080010400366";
        Dictionary<String, BackgroundWorker> mWorkers;
        public MainForm()
        {
            InitializeComponent();
            mUserManager = new UserManager();
            mWorkers = new Dictionary<string, BackgroundWorker>();
        }
        //写日志，并显示
        public void WriteLog(String str)
        {
            DateTime dt = DateTime.Now;
            string sDt = dt.ToString(stringDateFormat);
            ListViewItem item = new ListViewItem(sDt, 0);
            item.SubItems.Add(str);
            this.listViewLog.Items.AddRange(new ListViewItem[] { item });
            this.listViewLog.Items[(this.listViewLog.Items.Count) - 1].EnsureVisible();   //滚到该行
            logFileWriter.WriteLine(sDt + "    " + str);
            logFileWriter.Flush();

        }
     
        private String CurrentPhone()
        {
            return mUserManager.getCurrentKey();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            //this.Text = "秒杀器 v2.2" + " 2013-07-31版";
            this.mProductID = ConfigurationManager.AppSettings["pid"];
            this.Text = "商品ID"+mProductID+"，上次保存日期：" + ConfigurationManager.AppSettings["dt"];
            logFileWriter = File.AppendText(".\\log.txt");
            WriteLog("加载用户...");
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

            while (u._status != UserManager.STATUS_COOKIE_READY)
            {
                // Monitor.Enter(oLock);
                String msg = miaoshaWork(u);

                // Monitor.Exit(oLock);
                if (msg.StartsWith("DONE"))
                {
                    if (u._status == UserManager.STATUS_ONE)
                    {
                        u._status = UserManager.STATUS_TWO;
                        return;
                    }
                    else
                    {
                        u._status = UserManager.STATUS_ONE;
                    }
                }
                count = count + 1;
                u._msg = "开始..." + count + " " + msg;
                String logMsg = "用户" + u._phone + " 第" + count + "次尝试，" + msg;
                Thread.Sleep(100);
                bw.ReportProgress(1, logMsg);
            }
        }
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ResetUserListView();
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int per = e.ProgressPercentage;
            String msg = e.UserState.ToString();
            WriteLog(msg);
            ResetUserListView();
        }
        Object oLock = new Object();
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
                    CheckCodeParser mParser = u._parser;
                    String checkCode = mParser.parse(i);
                    response.Close();
                    //log("获取验证码：" + checkCode);

                    try
                    {
                        //验证验证码
                        // url:"/tuan/miao/miaoindex.html?tradeId=checkCodeForImmediately",
                        String checkCodeURL = "http://sale.dooyo.cn/tuan/miao/miaoindex.html?tradeId=checkCode";
                        //checkCodeURL = "http://sale.dooyo.cn/tuan/miao/miaoindex.html?tradeId=checkCodeForImmediately";
                        IDictionary<string, string> parameters = new Dictionary<string, string>();
                        parameters.Add("code", checkCode);
                        parameters.Add("product_id", mProductID);
                        HttpWebResponse response2 = HttpWebResponseUtility.CreatePostHttpResponse(
                            checkCodeURL, parameters, null, null, encoding, cookieCollection, true);
                        if (response2.StatusCode == HttpStatusCode.OK)
                        {
                            // log("验证验证码：" + checkCode);
                            System.IO.StreamReader sr = new System.IO.StreamReader(response2.GetResponseStream());
                            String content = sr.ReadToEnd(); //这里的content就是网页内容了 
                            sr.Close();
                            response2.Close();
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
                                parameters2.Add("userOrdersCount", "0");
                                parameters2.Add("maxOrdersCount", "2");//2
                                parameters2.Add("payTypeRadio", "ABC2");
                                //parameters2.Add("activityCode", "1");//2
                                //parameters2.Add("max_purchase_profit", "0.0");//2	
                                //parameters2.Add("mobile", u._phone);//2	
                                //parameters2.Add("order_sizecolor", "");//2	
                                parameters2.Add("product_id", this.mProductID);//2	
                                // parameters2.Add("product_props", "");//2	
                                //parameters2.Add("quantity", "1");//2		
                                // parameters2.Add("repeatOrderId", "");//2		
                                // parameters2.Add("token", "1375190998404");//2	

                                HttpWebResponse response3 = HttpWebResponseUtility.CreatePostHttpResponse(
                                    submitURL, parameters2, null, null, encoding, cookieCollection, true);
                                if (response3.StatusCode == HttpStatusCode.OK)
                                {
                                    //log("提交form");
                                    System.IO.StreamReader sr3 = new System.IO.StreamReader(response3.GetResponseStream());
                                    String content3 = sr3.ReadToEnd(); //这里的content就是网页内容了 
                                    sr3.Close();
                                    response3.Close();
                                    if (content3.Contains("抱歉"))
                                    {
                                        msg = "ER:提交表单失败啦" ;
                                    }
                                    else
                                    {
                                    
                                            //下单                                              
                                            IDictionary<string, string> buyParams = new Dictionary<string, string>();
                                            buyParams.Add("code", checkCode);
                                            buyParams.Add("vCode", checkCode);
                                            buyParams.Add("mobile", u._phone);
                                            //buyParams.Add("payTypeRadio", "1");
                                            buyParams.Add("product_id", this.mProductID);
                                            buyParams.Add("product_props", "");
                                            buyParams.Add("product_type", "0");
                                            buyParams.Add("payTypeRadio", "ABC2");
                                            buyParams.Add("quantity", "1");
                                            buyParams.Add("userOrdersCount", "1");
                                            buyParams.Add("maxOrdersCount", "10");
                                            HttpWebResponse responseBuy = HttpWebResponseUtility.CreatePostHttpResponse(
                                                   mBuyURL, buyParams, null, null, encoding, cookieCollection, true);
                                            if (responseBuy.StatusCode == HttpStatusCode.OK)
                                            {
                                                //log("下单");
                                                System.IO.StreamReader srBuy = new System.IO.StreamReader(responseBuy.GetResponseStream());
                                                String contentBuy = srBuy.ReadToEnd(); //这里的content就是网页内容了 
                                                srBuy.Close();
                                                //成功啦
                                                if (contentBuy.Contains("抱歉") || contentBuy.Contains("未开始秒杀"))
                                                {
                                                    msg = "ER:下单失败啦" ;
                                                }
                                                else
                                                {
                                                    msg = "DONE:快付款吧";
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
                        str = "ER:" + str;
                    }
                }
                mStr = msg;
                return msg;
            }
            catch (Exception ex)
            {
                String str = "ER:出错了" + ex.ToString();
                return str;
            }
        }
        //void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        //{
        //    //mCurrentPhone
        //    try
        //    {
        //        if (webBrowser1.Url.ToString().StartsWith(mLoginUrl))
        //        {
        //            if (!CurrentPhone().Equals(""))
        //            {
        //                dooyoABC.UserManager.User u = mUserManager.mMapUser[CurrentPhone()];

        //                HtmlElement user_id = this.webBrowser1.Document.GetElementById("user_id");
        //                HtmlElement web_login_pwd = this.webBrowser1.Document.GetElementById("web_login_pwd");
        //                HtmlElement verify_code = this.webBrowser1.Document.GetElementById("verify_code");
        //                HtmlElement imgHE = this.webBrowser1.Document.GetElementById("verifyCodeImg");
        //                HtmlElement login_form = this.webBrowser1.Document.GetElementById("login_form");
        //                user_id.InnerText = u._phone;
        //                web_login_pwd.InnerText = u._pwd;
        //                Image numPic = GetWebImage(webBrowser1, imgHE); // 得到验证码图片                
        //                String checkCode = mParser.parse(numPic);
        //                verify_code.InnerText = checkCode;
        //                user_id.InvokeMember("onblur");
        //                web_login_pwd.InvokeMember("onblur");
        //                verify_code.InvokeMember("onblur");
        //                login_form.InvokeMember("submit");
        //            }
        //        }
        //        else if (webBrowser1.Url.ToString().StartsWith(mAccountUrl))
        //        {
        //            if (!CurrentPhone().Equals(""))
        //            {
        //                dooyoABC.UserManager.User u = mUserManager.mMapUser[CurrentPhone()];
        //                String cookieStr = this.webBrowser1.Document.Cookie;
        //                String[] cookstr = cookieStr.Split(';');
        //                foreach (String str in cookstr)
        //                {
        //                    string[] cookieNameValue = str.Split('=');
        //                    Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
        //                    ck.Domain = "sale.dooyo.cn";
        //                    u._cookies.Add(ck);
        //                }
        //                u._status = UserManager.STATUS_COOKIE_READY;
        //                WriteLog("获取用户 " + u._phone + " 的cookie成功");
        //                ResetUserListView();
        //                if (mUserManager.NextKey() != -1)
        //                {
        //                    this.webBrowser1.Navigate(mLoginUrl);
        //                }
        //            }//end if (!mCurrentPhone.Equals(""))
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
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
            String status = "";
            if (u._status == UserManager.STATUS_INIT)
            {
                status = "初始化";
            }
            else if (u._status == UserManager.STATUS_COOKIE_READY)
            {
                status = "就绪";
            }
            else if (u._status == UserManager.STATUS_ONE)
            {
                status = "抢到一个";
            }
            else if (u._status == UserManager.STATUS_TWO)
            {
                status = "抢到两个";
            }
            else if (u._status == UserManager.STATUS_START)
            {
                status = "开始秒杀";
            }
            lvSubItem.Text = status;
            lvItem.SubItems.Add(lvSubItem);
            //未付款数
            lvSubItem = new ListViewItem.ListViewSubItem();
            lvSubItem.Text = "" + u._unpayCount;
            lvItem.SubItems.Add(lvSubItem);
            //结果
            lvSubItem = new ListViewItem.ListViewSubItem();
            lvSubItem.Text = "" + u._resultCount;
            lvItem.SubItems.Add(lvSubItem);
            //密码
            lvSubItem = new ListViewItem.ListViewSubItem();
            lvSubItem.Text = "未获取";
            if (u._cookies.Count > 0)
            {
                Cookie ck = u._cookies["AUTO_LOGIN_PWD"];
                if (ck != null)
                {
                    lvSubItem.Text = ck.Value;
                }
            }
            lvItem.SubItems.Add(lvSubItem);
            //消息
            lvSubItem = new ListViewItem.ListViewSubItem();
            lvSubItem.Text = u._msg;
            lvItem.SubItems.Add(lvSubItem);

            lvItem.Tag = u;
            if (u._unpayCount > 0)
            {
                lvItem.BackColor = Color.Red;
            }
            this.listViewUser.Items.Add(lvItem);


        }


        private void backgroundWorkerLoadUsers_DoWork(object sender, DoWorkEventArgs e)
        {
            String path = Application.StartupPath + "\\config.txt";
            if (mUserManager.loadUsers(path))
            {
                List<String> phones = mUserManager.getKeys();
                for (int i = 0; i < phones.Count; i++)
                {
                    UserManager.User u = mUserManager.mMapUser[phones[i]];
                    u._cookies = login(u._phone, u._pwd);
                    if (u._cookies.Count > 0)
                    {
                        u._status = UserManager.STATUS_COOKIE_READY;
                    }
                    int per = i * 100 / phones.Count;
                    this.backgroundWorkerLoadUsers.ReportProgress(per, u._phone);
                }
                e.Result = "OK:读取成功";
            }
            else
            {
                e.Result = "ER:读取失败";
            }
        }
        private void backgroundWorkerLoadUsers_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                String phone = e.UserState.ToString();
                String jid = mUserManager.mMapUser[phone]._cookies["JSESSIONID"].Value;
                WriteLog("用户:" + phone + " 登录成功," + jid);

            }
            catch (Exception ex)
            {
            }
        }

        private void backgroundWorkerLoadUsers_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.mUserManager.ResetNext();
            WriteLog("完成用户加载");
            ResetUserListView();
            if (!this.backgroundWorkerUnpay.IsBusy)
            {
                this.backgroundWorkerUnpay.RunWorkerAsync();
            }
            if (!this.backgroundWorkerResult.IsBusy)
            {
                this.backgroundWorkerResult.RunWorkerAsync();
            }
            this.ManulToolStripMenuItem.Enabled = true;
            //foreach (KeyValuePair<String, UserManager.User> kv in mUserManager.mMapUser)
            //{
            //    startWorker(kv.Value);
            //}
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // mWorkers
            foreach (KeyValuePair<String, UserManager.User> kv in mUserManager.mMapUser)
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
            WriteLog("手动停止");
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetUserListView();
        }

        private void ConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigForm cf = new ConfigForm();
            cf.PID = mProductID;
            if (cf.ShowDialog() == DialogResult.OK)
            {
                mProductID = cf.PID;
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationElement kvc = cfa.AppSettings.Settings["pid"];
                if (kvc != null)
                {
                    kvc.Value = mProductID;
                }
                else
                {
                    cfa.AppSettings.Settings.Add("pid", mProductID);
                    cfa.AppSettings.Settings.Add("dt", DateTime.Now.ToString("yyyy年MM月dd日"));
                }
                cfa.Save();
                WriteLog("更换商品ID：" + mProductID);
                String url = "http://sale.dooyo.cn/tuan/miao/miaoindex.html?tradeId=webViewMiao&product_id=" + mProductID;
                Process.Start(url);
            }
        }
        public CookieCollection login(String phone, String pwd)
        {
            try
            {
                Encoding encoding = Encoding.GetEncoding("gb2312");
                Random ra = new Random();
                String getCodeURL = "http://sale.dooyo.cn//tuan/code.html?tradeId=getAdCode&t=0." + ra.Next();
                HttpWebResponse responseCode = HttpWebResponseUtility.CreateGetHttpResponse(getCodeURL, null, null, null);
                if (responseCode.StatusCode == HttpStatusCode.OK)
                {
                    System.IO.Stream resStream = responseCode.GetResponseStream();
                    System.Drawing.Image i = Image.FromStream(resStream);
                    CheckCodeParser _parser = new CheckCodeParser();
                    String checkCode = _parser.parse(i);
                    responseCode.Close();
                    IDictionary<string, string> loginParams = new Dictionary<string, string>();
                    loginParams.Add("autoLogin", "true");
                    loginParams.Add("tradeId", "webLogin");
                    loginParams.Add("user_id", phone);
                    loginParams.Add("verify_code", checkCode);
                    loginParams.Add("web_login_pwd", pwd);
                    String set_cookie = responseCode.Headers["Set-Cookie"];
                    String[] sub = set_cookie.Split(new Char[] { ';' });
                    String[] cookies = sub[0].Split(new Char[] { '=' });
                    Cookie ck0 = new Cookie(cookies[0], cookies[1]);
                    ck0.Domain = "sale.dooyo.cn";
                    CookieCollection cc0 = new CookieCollection();
                    cc0.Add(ck0);
                    Cookie ck1 = new Cookie("activeBankName", "abc");
                    ck1.Domain = "sale.dooyo.cn";
                    cc0.Add(ck1);
                    Cookie ck2 = new Cookie("path", "/tuan/miao/");
                    ck2.Domain = "sale.dooyo.cn";
                    cc0.Add(ck2);

                    HttpWebResponse responseLogin = HttpWebResponseUtility.CreatePostHttpResponse(
                                               mLoginUrl, loginParams, null, null, encoding, cc0, false);
                    if (responseLogin.StatusCode == HttpStatusCode.Found)
                    {
                        CookieCollection cc = responseLogin.Cookies;
                        responseLogin.Close();
                        cc.Add(ck1);
                        return cc;
                    }

                }
                return new CookieCollection();
            }
            catch (Exception ex)
            {
                String er = ex.ToString();
                return new CookieCollection();
            }
        }

        private void backgroundWorkerUnpay_DoWork(object sender, DoWorkEventArgs e)
        {
            String url = "http://sale.dooyo.cn/tuan/account/myOrder.html?tradeId=queryAccOrderList";
            String keyword = "未支付";
            List<String> phones = mUserManager.getKeys();
            for (int i = 0; i < phones.Count; i++)
            {
                UserManager.User u = mUserManager.mMapUser[phones[i]];
                //查看订单页面              
                HttpWebResponse response = HttpWebResponseUtility.CreateGetHttpResponse(url, null, null, u._cookies);
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());
                String content = sr.ReadToEnd(); //这里的content就是网页内容了 
                sr.Close();
                response.Close();
                u._unpayCount = Regex.Matches(content, keyword).Count;
                //if (content.Contains(keyword))
                //{
                //    u._unpayCount = 1;
                //}
                //2013年06月19日

            }
        }

        private void backgroundWorkerUnpay_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.CheckPayToolStripMenuItem.Enabled = true;
            ResetUserListView();
        }

        private void CheckPayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.backgroundWorkerUnpay.IsBusy)
            {
                this.CheckPayToolStripMenuItem.Enabled = false;
                this.backgroundWorkerUnpay.RunWorkerAsync();
            }
        }

        private void listViewUser_DoubleClick(object sender, EventArgs e)
        {

            if (this.listViewUser.SelectedItems.Count > 0)
            {
                ListViewItem lvi = this.listViewUser.SelectedItems[0];
                UserManager.User u = lvi.Tag as UserManager.User;
                //if (u._unpayCount>0)
                {
                    WebViewForm wvf = new WebViewForm(u);
                    wvf.Show();
                }
            }
        }

        private void ResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ResultToolStripMenuItem.Enabled = false;
            this.ExportToolStripMenuItem.Enabled = false;
            if (!this.backgroundWorkerResult.IsBusy)
            {
                this.backgroundWorkerResult.RunWorkerAsync();
            }
        }

        private void backgroundWorkerResult_DoWork(object sender, DoWorkEventArgs e)
        {
            String url = "http://sale.dooyo.cn/tuan/account/myVoucher.html?tradeId=queryVoucherListAcc";
            String keyword = DateTime.Now.ToString("yyyy年MM月dd日");
            List<String> phones = mUserManager.getKeys();
            for (int i = 0; i < phones.Count; i++)
            {
                UserManager.User u = mUserManager.mMapUser[phones[i]];
                //查看订单页面              
                HttpWebResponse response = HttpWebResponseUtility.CreateGetHttpResponse(url, null, null, u._cookies);
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());
                String content = sr.ReadToEnd(); //这里的content就是网页内容了 
                sr.Close();
                response.Close();
                MatchCollection mc = GetValue(content, keyword, "<!");
                u._resultCount = mc.Count;
                for (int k = 0; k < mc.Count; k++)
                {
                    String code = mc[k].Value.Replace("<td>", "").Replace("</td>", "").Replace("\r\n", "").Trim();
                    if (!u._codes.Contains(code))
                    {
                        u._codes.Add(code);
                    }
                }
            }
        }

        private void backgroundWorkerResult_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ResultToolStripMenuItem.Enabled = true;
            this.ExportToolStripMenuItem.Enabled = true;
            ResetUserListView();
        }
        public static MatchCollection GetValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection mc = rg.Matches(str, 0);
            return mc;
        }

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ExportToolStripMenuItem.Enabled = false;
            if (!backgroundWorkerExport.IsBusy)
            {
                backgroundWorkerExport.RunWorkerAsync();
            }

        }

        private void backgroundWorkerExport_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                String fileName = DateTime.Now.ToString("yyyy年MM月dd日_HHmmss")+".txt";
                FileStream aFile = new FileStream(fileName, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(aFile);
                List<String> phones = mUserManager.getKeys();
                for (int i = 0; i < phones.Count; i++)
                {
                    UserManager.User u = mUserManager.mMapUser[phones[i]];
                    List<String> codes = u._codes;
                    if (codes.Count > 0)
                    {
                        for (int j = 0; j < codes.Count; j++)
                        {
                            String str = u._phone + "  " + DateTime.Now.ToString("yyyy年MM月dd日") + "  " + codes[j];
                            sw.WriteLine(str);
                        }
                    }
                }
                sw.Close();
                e.Result = fileName;
            }
            catch (IOException ex)
            {
                e.Result = "ER:失败";
                return;
            }
        }

        private void backgroundWorkerExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ExportToolStripMenuItem.Enabled = true;

            String fileName = e.Result.ToString();
            if(!fileName.StartsWith("ER:"))
            {
                if (DialogResult.OK == MessageBox.Show("输出到：" + fileName + ",是否打开文件？", "提示", MessageBoxButtons.OKCancel))
                {
                    String path=Application.StartupPath+"/"+fileName;
                    Process.Start(path);
                }

            }
        }

        private void ManulToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManualMiaoForm mmf = new ManualMiaoForm(mUserManager,mProductID);
            mmf.MsgEvent += new ManualMiaoForm.MsgEventDelegate(mmf_MsgEvent);
            mmf.Show();
            
        }

        void mmf_MsgEvent(string msg)
        {
            WriteLog(msg);
        }
    }
}