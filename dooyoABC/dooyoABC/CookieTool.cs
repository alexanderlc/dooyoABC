using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace Utils
{
    public class CookieTool
    {
        const char replacedChar = '_';
        List<string> cookieFieldList = new List<string>();
        public struct pairItem
        {
            public string key;
            public string value;
        };
        private string _processExpireField(Match foundExpire)
        {
            string replacedComma = "";
            replacedComma = foundExpire.Value.ToString().Replace(',', replacedChar);
            return replacedComma;
        }

        // replace the replacedChar back to original ','
        private string _recoverExpireField(Match foundPprocessedExpire)
        {
            string recovedStr = "";
            recovedStr = foundPprocessedExpire.Value.Replace(replacedChar, ',');
            return recovedStr;
        }
        //check whether the cookie name is valid or not
        public bool isValidCookieName(string ckName)
        {
            bool isValid = true;
            if (ckName == null)
            {
                isValid = false;
            }
            else
            {
                string invalidP = @"\W+";
                Regex rx = new Regex(invalidP);
                Match foundInvalid = rx.Match(ckName);
                if (foundInvalid.Success)
                {
                    isValid = false;
                }
            }

            return isValid;
        }
        // parse the cookie name and value

        public bool parseCookieNameValue(string ckNameValueExpr, out pairItem pair)
        {
            bool parsedOK = false;
            if (ckNameValueExpr == "")
            {
                pair.key = "";
                pair.value = "";
                parsedOK = false;
            }
            else
            {
                ckNameValueExpr = ckNameValueExpr.Trim();

                int equalPos = ckNameValueExpr.IndexOf('=');
                if (equalPos > 0) // is valid expression
                {
                    pair.key = ckNameValueExpr.Substring(0, equalPos);
                    pair.key = pair.key.Trim();
                    if (isValidCookieName(pair.key))
                    {
                        // only process while is valid cookie field
                        pair.value = ckNameValueExpr.Substring(equalPos + 1);
                        pair.value = pair.value.Trim();
                        parsedOK = true;
                    }
                    else
                    {
                        pair.key = "";
                        pair.value = "";
                        parsedOK = false;
                    }
                }
                else
                {
                    pair.key = "";
                    pair.value = "";
                    parsedOK = false;
                }
            }
            return parsedOK;
        }
        //given a string array 'origStrArr', get a sub string array from 'startIdx', length is 'len'
        public string[] getSubStrArr(string[] origStrArr, int startIdx, int len)
        {
            string[] subStrArr = new string[] { };
            if ((origStrArr != null) && (origStrArr.Length > 0) && (len > 0))
            {
                List<string> strList = new List<string>();
                int endPos = startIdx + len;
                if (endPos > origStrArr.Length)
                {
                    endPos = origStrArr.Length;
                }

                for (int i = startIdx; i < endPos; i++)
                {
                    //refer: http://zhidao.baidu.com/question/296384408.html
                    strList.Add(origStrArr[i]);
                }

                subStrArr = new string[len];
                strList.CopyTo(subStrArr);
            }

            return subStrArr;
        }

        //parse single cookie string to a cookie
        //example:
        //MSPShared=1; expires=Wed, 30-Dec-2037 16:00:00 GMT;domain=login.live.com;path=/;HTTPOnly= ;version=1
        //PPAuth=CkLXJYvPpNs3w!fIwMOFcraoSIAVYX3K!CdvZwQNwg3Y7gv74iqm9MqReX8XkJqtCFeMA6GYCWMb9m7CoIw!ID5gx3pOt8sOx1U5qQPv6ceuyiJYwmS86IW*l3BEaiyVCqFvju9BMll7!FHQeQholDsi0xqzCHuW!Qm2mrEtQPCv!qF3Sh9tZDjKcDZDI9iMByXc6R*J!JG4eCEUHIvEaxTQtftb4oc5uGpM!YyWT!r5jXIRyxqzsCULtWz4lsWHKzwrNlBRbF!A7ZXqXygCT8ek6luk7rarwLLJ!qaq2BvS; domain=login.live.com;secure= ;path=/;HTTPOnly= ;version=1
        public bool parseSingleCookie(string cookieStr, ref Cookie ck)
        {
            bool parsedOk = true;
            //Cookie ck = new Cookie();
            //string[] expressions = cookieStr.Split(";".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
            //refer: http://msdn.microsoft.com/en-us/library/b873y76a.aspx
            string[] expressions = cookieStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            //get cookie name and value
            pairItem pair = new pairItem();
            if (parseCookieNameValue(expressions[0], out pair))
            {
                ck.Name = pair.key;
                ck.Value = pair.value;

                string[] fieldExpressions = getSubStrArr(expressions, 1, expressions.Length - 1);
                foreach (string eachExpression in fieldExpressions)
                {
                    //parse key and value
                    if (parseCookieField(eachExpression, out pair))
                    {
                        // add to cookie field if possible
                        addFieldToCookie(ref ck, pair);
                    }
                    else
                    {
                        // if any field fail, consider it is a abnormal cookie string, so quit with false
                        parsedOk = false;
                        break;
                    }
                }
            }
            else
            {
                parsedOk = false;
            }

            return parsedOk;
        }//parseSingleCookie
        //add recognized cookie field: expires/domain/path/secure/httponly/version, into cookie
        public bool addFieldToCookie(ref Cookie ck, pairItem pairInfo)
        {
            bool added = false;
            if (pairInfo.key != "")
            {
                string lowerKey = pairInfo.key.ToLower();
                switch (lowerKey)
                {
                    case "expires":
                        DateTime expireDatetime;
                        if (DateTime.TryParse(pairInfo.value, out expireDatetime))
                        {
                            // note: here coverted to local time: GMT +8
                            ck.Expires = expireDatetime;

                            //update expired filed
                            if (DateTime.Now.Ticks > ck.Expires.Ticks)
                            {
                                ck.Expired = true;
                            }

                            added = true;
                        }
                        break;
                    case "domain":
                        ck.Domain = pairInfo.value;
                        added = true;
                        break;
                    case "secure":
                        ck.Secure = true;
                        added = true;
                        break;
                    case "path":
                        ck.Path = pairInfo.value;
                        added = true;
                        break;
                    case "httponly":
                        ck.HttpOnly = true;
                        added = true;
                        break;
                    case "version":
                        int versionValue;
                        if (int.TryParse(pairInfo.value, out versionValue))
                        {
                            ck.Version = versionValue;
                            added = true;
                        }
                        break;
                    default:
                        break;
                }
            }

            return added;
        }//addFieldToCookie
        // parse cookie field expression
        public bool parseCookieField(string ckFieldExpr, out pairItem pair)
        {
            bool parsedOK = false;

            if (ckFieldExpr == "")
            {
                pair.key = "";
                pair.value = "";
                parsedOK = false;
            }
            else
            {
                ckFieldExpr = ckFieldExpr.Trim();

                //some specials: secure/httponly
                if (ckFieldExpr.ToLower() == "httponly")
                {
                    pair.key = "httponly";
                    //pair.value = "";
                    pair.value = "true";
                    parsedOK = true;
                }
                else if (ckFieldExpr.ToLower() == "secure")
                {
                    pair.key = "secure";
                    //pair.value = "";
                    pair.value = "true";
                    parsedOK = true;
                }
                else // normal cookie field
                {
                    int equalPos = ckFieldExpr.IndexOf('=');
                    if (equalPos > 0) // is valid expression
                    {
                        pair.key = ckFieldExpr.Substring(0, equalPos);
                        pair.key = pair.key.Trim();
                        if (isValidCookieField(pair.key))
                        {
                            // only process while is valid cookie field
                            pair.value = ckFieldExpr.Substring(equalPos + 1);
                            pair.value = pair.value.Trim();
                            parsedOK = true;
                        }
                        else
                        {
                            pair.key = "";
                            pair.value = "";
                            parsedOK = false;
                        }
                    }
                    else
                    {
                        pair.key = "";
                        pair.value = "";
                        parsedOK = false;
                    }
                }
            }

            return parsedOK;
        }//parseCookieField
        public bool isValidCookieField(string cookieKey)
        {
            return cookieFieldList.Contains(cookieKey.ToLower());
        }
        public CookieCollection parseSetCookie(string setCookieStr, string curDomain)
        {
            CookieCollection parsedCookies = new CookieCollection();

            // process for expires and Expires field, for it contains ','
            //refer: http://www.yaosansi.com/post/682.html
            // may contains expires or Expires, so following use xpires
            string commaReplaced = Regex.Replace(setCookieStr, @"xpires=\w{3},\s\d{2}-\w{3}-\d{4}", new MatchEvaluator(_processExpireField));
            string[] cookieStrArr = commaReplaced.Split(',');
            foreach (string cookieStr in cookieStrArr)
            {
                Cookie ck = new Cookie();
                // recover it back
                string recoveredCookieStr = Regex.Replace(cookieStr, @"xpires=\w{3}" + replacedChar + @"\s\d{2}-\w{3}-\d{4}", new MatchEvaluator(_recoverExpireField));
                if (parseSingleCookie(recoveredCookieStr, ref ck))
                {
                    if (needAddThisCookie(ck, curDomain))
                    {
                        parsedCookies.Add(ck);
                    }
                }
            }

            return parsedCookies;
        }//parseSetCookie
        //check whether need add/retain this cookie
        // not add for:
        // ck is null or ck name is null
        // domain is null and curDomain is not set
        // expired and retainExpiredCookie==false
        private bool needAddThisCookie(Cookie ck, string curDomain)
        {
            bool needAdd = false;

            if ((ck == null) || (ck.Name == ""))
            {
                needAdd = false;
            }
            else
            {
                if (ck.Domain != "")
                {
                    needAdd = true;
                }
                else// ck.Domain == ""
                {
                    if (curDomain != "")
                    {
                        ck.Domain = curDomain;
                        needAdd = true;
                    }
                    else // curDomain == ""
                    {
                        // not set current domain, omit this
                        // should not add empty domain cookie, for this will lead execute CookieContainer.Add() fail !!!
                        needAdd = false;
                    }
                }
            }

            return needAdd;
        }
    }
}
