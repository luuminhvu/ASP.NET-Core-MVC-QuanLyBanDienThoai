﻿using System.Text;
using XSystem.Security.Cryptography;

namespace QuanLyBanDienThoai.Extension
{
    public static class HMD5
    {
        public static string ToHMD5(this string str)
        {
           MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in bHash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            return sbHash.ToString();
        }
    }
}
