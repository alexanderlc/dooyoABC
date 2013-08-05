using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Utils;

namespace dooyoABC
{
    public partial class MiaoUserControl : UserControl
    {
        public delegate void MsgEventDelegate(string msg);
        public event MsgEventDelegate MsgEvent;
        /// <summary>
        /// 信息事件
        /// </summary>
        protected void OnMsg(string msg)
        {
            if (MsgEvent != null)
            {
                MsgEvent(msg);
            }
        }
        private UserManager.User mUser;
        private String mProductID;
        private int mTryCnt;
        private int mSuccCnt;
        private int mTabIndex;
        public MiaoUserControl(UserManager.User u,String pid,int tabindex)
        {
            mUser = u;
            mProductID = pid;
            mTryCnt = 0;
            mSuccCnt = 0;
            mTabIndex = tabindex;
            InitializeComponent();
        }

        private void MiaoUserControl_Load(object sender, EventArgs e)
        {
            this.groupBoxUser.Text = mUser._phone;
            this.textBoxCode.TabIndex = mTabIndex;
            this.backgroundWorkerGetCode.RunWorkerAsync();
            
        }

        private void backgroundWorkerGetCode_DoWork(object sender, DoWorkEventArgs e)
        {
            String msg = "";
            Encoding encoding = Encoding.GetEncoding("gb2312");
            CookieCollection cookieCollection = mUser._cookies;
            //获取验证码
            Random ra = new Random();
            String getCodeURL = "http://sale.dooyo.cn/tuan/code.html?tradeId=getMiaoShaCode&t=" + ra.Next();
            getCodeURL = "http://sale.dooyo.cn/tuan/code.html?tradeId=getMiaoShaCodeForImmediately&t=" + ra.Next();
            
            HttpWebResponse response = HttpWebResponseUtility.CreateGetHttpResponse(getCodeURL, null, null, cookieCollection);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                System.IO.Stream resStream = response.GetResponseStream();
                System.Drawing.Image i = Image.FromStream(resStream);
                response.Close();
                e.Result = i;
            }
        }

        private void backgroundWorkerGetCode_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Drawing.Image i = e.Result as System.Drawing.Image;
            if (i != null)
            {
                this.pictureBoxCode.Image = i;
                //String code=CheckCodeParser.parseCode(i);
                this.buttonMiao.Enabled = true;
            }
        }

        private void buttonMiao_Click(object sender, EventArgs e)
        {
            if (!backgroundWorkerMiao.IsBusy)
            {
                this.buttonMiao.Enabled = false;
                this.backgroundWorkerMiao.RunWorkerAsync(this.textBoxCode.Text);
            }
        }

        private void backgroundWorkerMiao_DoWork(object sender, DoWorkEventArgs e)
        {
            mTryCnt = mTryCnt + 1;
            String checkCode = e.Argument.ToString();
            Encoding encoding = Encoding.GetEncoding("gb2312");
            CookieCollection cookieCollection = mUser._cookies;
            String msg = "";
            try
            {
                //验证验证码
                // url:"/tuan/miao/miaoindex.html?tradeId=checkCodeForImmediately",
                String checkCodeURL = "http://sale.dooyo.cn/tuan/miao/miaoindex.html?tradeId=checkCode";
                checkCodeURL = "http://sale.dooyo.cn/tuan/miao/miaoindex.html?tradeId=checkCodeForImmediately";
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
                        parameters2.Add("maxOrdersCount", "2");
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
                                msg = "ER:提交表单失败啦";
                            }
                            else
                            {
                                //下单                                              
                                IDictionary<string, string> buyParams = new Dictionary<string, string>();
                                buyParams.Add("code", checkCode);
                                buyParams.Add("vCode", checkCode);
                                buyParams.Add("mobile", mUser._phone);
                                //buyParams.Add("payTypeRadio", "1");
                                buyParams.Add("product_id", this.mProductID);
                                buyParams.Add("product_props", "");
                                buyParams.Add("product_type", "0");
                                buyParams.Add("payTypeRadio", "ABC2");
                                buyParams.Add("quantity", "1");
                                buyParams.Add("userOrdersCount", "1");
                                buyParams.Add("maxOrdersCount", "10");
                               
                                String mBuyURL = "http://sale.dooyo.cn/tuan/miao/orderMiao.html?tradeId=miaoSha";
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
                                        msg = "ER:下单失败啦";
                                    }
                                    else
                                    {
                                        msg = "DONE:快付款吧";
                                    }
                                }
                                // }//while
                            }//if(!content3.Contains("抱歉"))
                        }//if (response3.StatusCode == HttpStatusCode.OK)
                    }
                }
            }
            catch (Exception ex)
            {
                String str = ex.ToString();
                str = "ER:" + str;
            }
            e.Result = msg;
        }

        private void backgroundWorkerMiao_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            String msg = e.Result.ToString();
            this.textBoxMsg.Text = msg;
            OnMsg("[" + mUser._phone + "]:" + msg);
            this.buttonMiao.Enabled = false;
            if (msg.StartsWith("DONE"))
            {
                this.mSuccCnt++;
                if (mUser._status == UserManager.STATUS_ONE)
                {
                    mUser._status = UserManager.STATUS_TWO;
                }
                else
                {
                    mUser._status = UserManager.STATUS_ONE;
                }
                this.groupBoxUser.BackColor = Color.Red;
            }
            this.label1.Text = "次数：" + this.mSuccCnt + "/" + mTryCnt;

            if (mSuccCnt >= 2)
            {
                this.label1.ForeColor = Color.Red;
                this.textBoxMsg.BackColor = Color.Red;
                this.textBoxMsg.ForeColor = Color.White;
            }
            else
            {
                if (!this.backgroundWorkerGetCode.IsBusy)
                {
                    this.backgroundWorkerGetCode.RunWorkerAsync();
                }
            }
        }
           

        private void pictureBoxCode_Click(object sender, EventArgs e)
        {
            if (!this.backgroundWorkerGetCode.IsBusy)
            {
                this.backgroundWorkerGetCode.RunWorkerAsync();
            }
        }

       
        private void textBoxCode_TextChanged(object sender, EventArgs e)
        {
            String code = this.textBoxCode.Text;
            if (code.Length == 4)
            {
                if (!backgroundWorkerMiao.IsBusy)
                {
                    
                    this.buttonMiao.Enabled = false;
                    this.backgroundWorkerMiao.RunWorkerAsync(this.textBoxCode.Text);
                    this.textBoxCode.Text = "";
                    this.textBoxCode.BackColor = Color.LightGray;
                    SendKeys.Send("{tab}");
                }
            }
        }

        private void textBoxCode_Enter(object sender, EventArgs e)
        {
            this.textBoxCode.BackColor = Color.White;
        }

      


      
    }
}
