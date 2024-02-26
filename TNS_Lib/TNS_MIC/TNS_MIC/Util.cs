using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TNS_MIC
{
    public static class Util
    {

        //to number
        public static int ToInt(this object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            try
            {
                return int.Parse(obj.ToString());
            }
            catch
            {
                return 0;
            }
        }
        public static float ToFloat(this object obj)
        {
            try
            {
                if (obj.ToString().Contains(","))
                {
                    obj = obj.ToString().Replace(",", "");
                }

                return float.Parse(obj.ToString());
            }
            catch
            {
                return 0;
            }
        }
        public static decimal ToDecimal(this object obj)
        {
            try
            {
                if (obj.ToString().Contains(","))
                {
                    obj = obj.ToString().Replace(",", "");
                }

                return decimal.Parse(obj.ToString());
            }
            catch
            {
                return 0;
            }
        }
        public static double ToRound(this object obj)
        {
            try
            {
                if (obj.ToString().Contains(","))
                {
                    obj = obj.ToString().Replace(",", "");
                }

                return Math.Round(double.Parse(obj.ToString()), 1);
            }
            catch
            {
                return 0;
            }
        }
        public static double ToDouble(this object obj)
        {
            try
            {
                if (obj.ToString().Contains(","))
                {
                    obj = obj.ToString().Replace(",", "");
                }

                return double.Parse(obj.ToString());
            }
            catch
            {
                return 0;
            }
        }
        //to string
        public static string Ton0String(this object obj)
        {
            try
            {
                return double.Parse(obj.ToString()).ToString("n0");
            }
            catch
            {
                return "0";
            }
        }
        public static string Ton1String(this object obj)
        {
            try
            {
                return double.Parse(obj.ToString()).ToString("n1");
            }
            catch
            {
                return "0";
            }
        }

        public static string Ton2String(this object obj)
        {
            try
            {
                return double.Parse(obj.ToString()).ToString("n2");
            }
            catch
            {
                return "0";
            }
        }
        public static string Toe0String(this object obj)
        {
            try
            {
                if (double.Parse(obj.ToString()) != 0)
                    return double.Parse(obj.ToString()).ToString("n0");
                else
                    return string.Empty;
            }
            catch
            {
                return "";
            }
        }
        public static string Toe1String(this object obj)
        {
            try
            {
                if (double.Parse(obj.ToString()) != 0)
                {
                    return double.Parse(obj.ToString()).ToString("n1");
                }
                else
                    return string.Empty;
            }
            catch
            {
                return "";
            }
        }
        public static string Toe2String(this object obj)
        {
            try
            {
                if (double.Parse(obj.ToString()) != 0)
                    return double.Parse(obj.ToString()).ToString("n2");
                else
                    return string.Empty;
            }
            catch
            {
                return "";
            }
        }
        public static string ToViString(this object obj)
        {
            return double.Parse(obj.ToString()).ToString("##,##0", new CultureInfo("vi-VN"));
        }
        /// <summary>
        /// parse datetime to dd/MM/yyyy
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToDateString(this object obj)
        {
            try
            {
                string s = "";
                DateTime zDate = DateTime.Parse(obj.ToString());
                if (zDate != DateTime.MinValue)
                {
                    s = zDate.ToString("dd/MM/yyyy");
                }

                return s;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static String ToAscii(this String unicode)
        {

            unicode = unicode.ToLower().Trim();
            unicode = Regex.Replace(unicode, "[áàảãạăắằẳẵặâấầẩẫậ]", "a");
            unicode = Regex.Replace(unicode, "[๖ۣۜ]", "");
            unicode = Regex.Replace(unicode, "[óòỏõọôồốổỗộơớờởỡợ]", "o");
            unicode = Regex.Replace(unicode, "[éèẻẽẹêếềểễệ]", "e");
            unicode = Regex.Replace(unicode, "[íìỉĩị]", "i");
            unicode = Regex.Replace(unicode, "[úùủũụưứừửữự]", "u");
            unicode = Regex.Replace(unicode, "[ýỳỷỹỵ]", "y");
            unicode = Regex.Replace(unicode, "[đ]", "d");
            unicode = unicode.Replace(" ", "-").Replace("[()]", "");
            unicode = Regex.Replace(unicode, "[-\\s+/]+", "-");
            unicode = Regex.Replace(unicode, "\\W+", "-"); //Nếu bạn muốn thay dấu khoảng trắng thành dấu "_" hoặc dấu cách " " thì thay kí tự bạn muốn vào đấu "-"
            return unicode;
        }
        /// <summary>
    }
}
