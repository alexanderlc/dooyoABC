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
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.NewWindow += new CancelEventHandler(webBrowser1_NewWindow);
            this.Text = "付款：" + mUser._phone;
            if (mUser._cookies != null && mUser._cookies.Count > 0)
            {
                foreach (Cookie ck in mUser._cookies)
                {
                    InternetSetCookie(mURLMyOrder, ck.Name, ck.Value);
                }
                this.webBrowser1.Navigate(mURLMyOrder);
            }
            else
            {
                this.webBrowser1.Navigate(mLoginURL);
            }
        }

        void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            string url = webBrowser1.Document.ActiveElement.GetAttribute("href");
            if (url != string.Empty)
            {
                webBrowser1.Navigate(url);
            }
            e.Cancel = true;
        }


        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //将所有的链接的目标，指向本窗体
            foreach (HtmlElement archor in this.webBrowser1.Document.Links)
            {
                archor.SetAttribute("target", "_self");
            }

            //将所有的FORM的提交目标，指向本窗体
            foreach (HtmlElement form in this.webBrowser1.Document.Forms)
            {
                form.SetAttribute("target", "_self");
            }
        }
    }
}