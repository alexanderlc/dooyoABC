using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace dooyoABC
{
    class UserManager
    {
        public static int STATUS_NULL = -1;
        public static int STATUS_COOKIE_READY =0;
        public static int STATUS_ONE = 1;
        public static int STATUS_TWO = 2;
        public class User
        {
            public String _phone;
            public String _pwd;
            public int _status;
            public CookieCollection _cookies;
        }
        public Dictionary<String, User> mMapUser;
        public UserManager()
        {
            mMapUser = new Dictionary<string, User>();
           
        }
       
        public int getUserCount()
        {
            return mMapUser.Count;
        }
        public Boolean loadUsers(String fileName)
        {
            try
            {
                StreamReader sr = new StreamReader(fileName, Encoding.Default);
                String lineStr;
                while ((lineStr = sr.ReadLine()) != null)
                {
                    String[] sub = lineStr.Trim().Split(new char[] { ',' });
                    if (sub.Length == 2)
                    {
                        String phone = sub[0];
                        String pwd = sub[1];
                        User u = new User();
                        u._phone = phone;
                        u._pwd = pwd;
                        u._status = STATUS_NULL;
                        u._cookies = new CookieCollection();
                        mMapUser.Add(phone, u);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Boolean AddCookie(String phone, String key, String value)
        {
            if (!mMapUser.ContainsKey(phone))
            {
                return false;
            }
            else
            {
                User u = mMapUser[phone];
                Cookie ck = u._cookies[key];
                if (ck == null || !ck.Name.Equals(key))
                {
                    ck = new Cookie(key, value);
                    u._cookies.Add(ck);
                }
                else
                {
                    ck.Value = value;
                } 
                 return true;
            }
        }
        public User getCookies(String phone)
        {
            if (!mMapUser.ContainsKey(phone))
            {
                return null;
            }
            else
            {
                User u = mMapUser[phone];
                return u;
            }
        }
        public Boolean setUserStatus(String phone, int status)
        {
            if (!mMapUser.ContainsKey(phone))
            {
                return false;
            }
            else
            {
                User u = mMapUser[phone];
                u._status = status;
                return true;
            }
        }
    }
}
