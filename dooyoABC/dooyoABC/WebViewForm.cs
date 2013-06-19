using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;

namespace dooyoABC
{
    public partial class WebViewForm : Form
    {
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        private UserManager.User mUser;
        String mURLMyOrder = "http://sale.dooyo.cn/tuan/account/myOrder.html?tradeId=queryAccOrderList";
        String mLoginURL = "http://sale.dooyo.cn/tuan/account/login.html";
        public WebViewForm(UserManager.User u)
        {
            InitializeComponent();
            mUser = u;
        }

        private void WebViewForm_Load(object sender, EventArgs e)
        {
            this.axWebBrowser1.RegisterAsBrowser = true;
            this.axWebBrowser1.Silent = true;
            this.axWebBrowser1.NewWindow3 += new AxSHDocVw.DWebBrowserEvents2_NewWindow3EventHandler(axWebBrowser1_NewWindow3);
            this.axWebBrowser1.NewWindow2 += new AxSHDocVw.DWebBrowserEvents2_NewWindow2EventHandler(axWebBrowser1_NewWindow2);
            this.Text = "付款：" + mUser._phone;
            if (mUser._cookies != null && mUser._cookies.Count > 0)
            {
                foreach (Cookie ck in mUser._cookies)
                {
                    InternetSetCookie(mURLMyOrder, ck.Name, ck.Value);                   
                }
                //this.webBrowser1.Navigate(mURLMyOrder);
                this.axWebBrowser1.Navigate(mURLMyOrder);
            }
            else
            {
                this.axWebBrowser1.Navigate(mLoginURL);
            }
        }

        void axWebBrowser1_NewWindow3(object sender, AxSHDocVw.DWebBrowserEvents2_NewWindow3Event e)
        {
            e.cancel = true;
            if (mUser._cookies != null && mUser._cookies.Count > 0)
            {
                foreach (Cookie ck in mUser._cookies)
                {
                    InternetSetCookie(e.bstrUrl, ck.Name, ck.Value);
                }              
            }
            this.axWebBrowser1.Navigate(e.bstrUrl);
        }

        void axWebBrowser1_NewWindow2(object sender, AxSHDocVw.DWebBrowserEvents2_NewWindow2Event e)
        {
            //String url =
            e.cancel = true;
            
        }       
    }
}