
namespace EAFrame.WebControls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
    /// 数据安全验证类 
    /// </summary>
    public static class DataSecurity
    {
        private static Random rnd = new Random();

        /// <summary>
        /// 获取字符串数组中指定的值
        /// </summary>
        /// <param name="index">数据中的位置</param>
        /// <param name="field">字符串数组</param>
        /// <returns>返回字符串数组中指定的值</returns>
        public static string GetArrayValue(int index, string[] field)
        {
            if (field == null)
            {
                return string.Empty;
            }

            if (index >= 0 && index < field.Length)
            {
                return field[index];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取字符串数组中指定的值
        /// </summary>
        /// <param name="index">数据中的位置</param>
        /// <param name="field">字符串数组</param>
        /// <returns>返回字符串数组中指定的值</returns>
        public static string GetArrayValue(int index, Collection<string> field)
        {
            if (index >= 0 && index < field.Count)
            {
                return field[index];
            }
            else
            {
                return string.Empty;
            }
        }

        #region 生成随机数字字符串
        /// <summary>
        /// 获取指定长度的纯数字随机数字串
        /// </summary>
        /// <param name="intlong">数字串长度</param>
        /// <returns>字符串</returns>
        public static string RandomNum(int intlong)
        {
            //Random r = new Random();
            StringBuilder w = new StringBuilder(string.Empty);

            for (int i = 0; i < intlong; i++)
            {
                w.Append(rnd.Next(10));
            }

            return w.ToString();
        }

        /// <summary>
        /// 生成指定长度和字符的随机字符串
        /// 通过调用 Random 类的 Next() 方法，先获得一个大于或等于 0 而小于 pwdchars 长度的整数
        /// 以该数作为索引值，从可用字符串中随机取字符，以指定的密码长度为循环次数
        /// 依次连接取得的字符，最后即得到所需的随机密码串了。
        /// </summary>
        /// <param name="pwdchars">随机字符串里包含的字符</param>
        /// <param name="pwdlen">随机字符串的长度</param>
        /// <returns>随机产生的字符串</returns>
        public static string MakeRandomString(string pwdchars, int pwdlen)
        {
            StringBuilder tmpstr = new StringBuilder();
            int randNum;
            //Random rnd = new Random();
            for (int i = 0; i < pwdlen; i++)
            {
                randNum = rnd.Next(pwdchars.Length);
                tmpstr.Append(pwdchars[randNum]);
            }

            return tmpstr.ToString();
        }

        /// <summary>
        /// 生成指定长度和字符的随机字符串
        /// </summary>
        /// <param name="pwdlen">随机字符串的长度</param>
        /// <returns>随机产生的字符串</returns>
        public static string MakeRandomString(int pwdlen)
        {
            return MakeRandomString("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_*", pwdlen);
        }

        /// <summary>
        /// 按照月份生成文件夹名
        /// 生成的格式为 200608
        /// </summary>
        /// <returns>文件夹名</returns>
        public static string MakeFolderName()
        {
            return DateTime.Now.ToString("yyyyMM", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// 按照年月时分秒随机数生成文件名
        /// </summary>
        /// <returns>随机文件名</returns>
        public static string MakeFileRndName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture) + MakeRandomString("0123456789", 4);
        }

        #endregion

        #region 对字符串进行编码或者过滤

        /// <summary>
        /// 对字符串进行 HTML 编码操作
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>替换后的字符串</returns>
        public static string HtmlEncode(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                str = str.Replace(" ", "&nbsp;");
                str = str.Replace("'", "&#39;");
                str = str.Replace("\"", "&quot;");
                str = str.Replace("\r\n", "<br />");
                str = str.Replace("\n", "<br />");
            }

            return str;
        }

        /// <summary>
        /// 转换为JavaScript输出
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string ConvertToJavaScript(string str)
        {
            str = str.Replace("\\", "\\\\");
            str = str.Replace("\n", "\\n");
            str = str.Replace("\r", "\\r");
            str = str.Replace("\"", "\\\"");
            str = str.Replace("'", "\\\'");
            //str = str.Replace("\"", "\\\"");
            return str;
        }

        /// <summary>
        /// 对字符串进行 XML 编码操作
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>替换后的字符串</returns>
        public static string XmlEncode(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("&", "&amp;");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                str = str.Replace("'", "&apos;");
                str = str.Replace("\"", "&quot;");
            }

            return str;
        }

        /// <summary>
        /// 对对象进行HtmlEncode编码
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>字符串</returns>
        public static string HtmlEncode(object value)
        {
            if (value == null)
            {
                return null;
            }

            return HtmlEncode(value.ToString());
        }

        /// <summary>
        /// 对字符串进行 HTML 解码操作
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>替换后的字符串</returns>
        public static string HtmlDecode(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("<br>", "\n");
                value = value.Replace("<br/>", "\n");
                value = value.Replace("<br />", "\n");
                value = value.Replace("&gt;", ">");
                value = value.Replace("&lt;", "<");
                value = value.Replace("&nbsp;", " ");
                value = value.Replace("&#39;", "'");
                value = value.Replace("&quot;", "\"");
            }

            return value;
        }

        /// <summary>
        /// 对对象进行HtmlDecode解码操作
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>字符串</returns>
        public static string HtmlDecode(object value)
        {
            if (value == null)
            {
                return null;
            }

            return HtmlDecode(value.ToString());
        }

        /// <summary>
        /// 对地址进行编码，拒绝跨站脚本攻击
        /// </summary>
        /// <param name="weburl">URL </param>
        /// <returns>返回UrlEncode后的值</returns>
        public static string UrlEncode(string weburl)
        {
            if (string.IsNullOrEmpty(weburl))
            {
                return null;
            }

            return Regex.Replace(weburl, "[^a-zA-Z0-9,-_\\.]+", new MatchEvaluator(UrlEncodeMatch));
        }

        /// <summary>
        /// 对地址进行编码，拒绝跨站脚本攻击
        /// </summary>
        /// <param name="weburl">URL </param>
        /// <param name="systemEncode">True用系统自带方法Encode,否则用程序Encode</param>
        /// <returns>返回UrlEncode后的值</returns>
        public static string UrlEncode(string weburl, bool systemEncode)
        {
            if (string.IsNullOrEmpty(weburl))
            {
                return null;
            }

            if (systemEncode)
            {
                return HttpUtility.UrlEncode(weburl);
            }
            else
            {
                return UrlEncode(weburl);
            }
        }

        /// <summary>
        /// 编码字符串中的标签调用
        /// </summary>
        /// <param name="value">源字符串</param>
        /// <returns>返回编码后的字符串</returns>
        public static string PELabelEncode(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("{", "ζ#123;");
                value = value.Replace("}", "ζ#125;");
                value = Regex.Replace(value, @"<!--([^>]*?#include[\s\S]*?)-->", "&lt;!--$1--&gt;", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                value = value.Replace("ζ#123;PE.SiteConfig.uploaddir/ζ#125;", "{PE.SiteConfig.uploaddir/}");
                value = value.Replace("ζ#123;PE.SiteConfig.ApplicationPath/ζ#125;", "{PE.SiteConfig.ApplicationPath/}");
            }

            return value;
        }

        /// <summary>
        /// 解码进行标签调用编码的字符串
        /// </summary>
        /// <param name="value">源字符串</param>
        /// <returns>返回解码后的字符串</returns>
        public static string PELabelDecode(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("ζ#123;", "{");
                value = value.Replace("ζ#125;", "}");
            }

            return value;
        }

        /// <summary>
        /// 过滤掉字符串中会引起注入攻击的字符
        /// </summary>
        /// <param name="strchar">要过滤的字符串</param>
        /// <returns>已过滤的字符串</returns>
        public static string FilterBadChar(string strchar)
        {
            string tempstrChar;
            string newstrChar = string.Empty;
            if (string.IsNullOrEmpty(strchar))
            {
                newstrChar = string.Empty;
            }
            else
            {
                tempstrChar = strchar;
                string[] strBadChar = { "+", "'", "%", "^", "&", "?", "(", ")", "<", ">", "[", "]", "{", "}", "/", "\"", ";", ":", "Chr(34)", "Chr(0)", "--" };
                StringBuilder strBuilder = new StringBuilder(tempstrChar);
                for (int i = 0; i < strBadChar.Length; i++)
                {
                    newstrChar = strBuilder.Replace(strBadChar[i], string.Empty).ToString();
                }

                newstrChar = Regex.Replace(newstrChar, "@+", "@");
            }

            return newstrChar;
        }

        /// <summary>
        /// 过滤SQL关键字
        /// </summary>
        /// <param name="strchar">待过滤的字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string FilterSqlKeyword(string strchar)
        {
            bool contains = false;
            if (string.IsNullOrEmpty(strchar))
            {
                return string.Empty;
            }

            //strchar = strchar.ToUpperInvariant();
            string[] keywords = { "SELECT", "UPDATE", "INSERT", "DELETE", "DECLARE", "@", "EXEC", "DBCC", "ALTER", "DROP", "CREATE", "BACKUP", "IF", "ELSE", "END", "AND", "OR", "ADD", "SET", "OPEN", "CLOSE", "USE", "BEGIN", "RETUN", "AS", "GO", "EXISTS", "KILL", "&" };

            for (int i = 0; i < keywords.Length; i++)
            {
                //if (strchar.Contains(keywords[i]))
                //{
                //    strchar = strchar.Replace(keywords[i], string.Empty);
                //    contains = true;
                //}
                Regex regex = new Regex(keywords[i], RegexOptions.IgnoreCase);
                if (regex.IsMatch(strchar))
                {
                    strchar = regex.Replace(strchar, string.Empty);
                    contains = true;
                }
            }

            if (contains)
            {
                return FilterSqlKeyword(strchar);
            }

            return strchar;
        }

        /// <summary>
        /// 对对象进行UrlEncode
        /// </summary>
        /// <param name="value">URL地址对象</param>
        /// <returns>编码后的URL地址</returns>
        public static string UrlEncode(object value)
        {
            if (value == null)
            {
                return null;
            }

            return UrlEncode(value.ToString());
        }

        /// <summary>
        /// UrlEncode匹配
        /// </summary>
        /// <param name="match">匹配后得到的字符串</param>
        /// <returns>字符的URL实体</returns>
        private static string UrlEncodeMatch(Match match)
        {
            string matchString = match.ToString();
            if (matchString.Length < 1)
            {
                return matchString;
            }

            StringBuilder sb = new StringBuilder();

            foreach (char ii in matchString)
            {
                if (ii > '\x007f')
                {
                    sb.AppendFormat("%u{0:X4}", (int)ii);
                }
                else
                {
                    sb.AppendFormat("%{0:X2}", (int)ii);
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
