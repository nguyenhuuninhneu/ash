﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Web.Core
{
    public class HelperString
    {
        /// <summary>
        /// Chuyển đổi chữ tiếng việt có dấu sang không dấu
        /// </summary>
        /// <param name="strUnicode">Chuỗi tiếng việt có dấu</param>
        /// <returns>Trả lại một chuỗi không dấu</returns>
        //public static string UnsignCharacter(string text)
        //{
        //    text = text.ToLower();
        //    var pattern = new string[7];
        //    pattern[0] = "a|(á|ả|à|ạ|ã|ă|ắ|ẳ|ằ|ặ|ẵ|â|ấ|ẩ|ầ|ậ|ẫ)";
        //    pattern[1] = "o|(ó|ỏ|ò|ọ|õ|ô|ố|ổ|ồ|ộ|ỗ|ơ|ớ|ở|ờ|ợ|ỡ)";
        //    pattern[2] = "e|(é|è|ẻ|ẹ|ẽ|ê|ế|ề|ể|ệ|ễ)";
        //    pattern[3] = "u|(ú|ù|ủ|ụ|ũ|ư|ứ|ừ|ử|ự|ữ)";
        //    pattern[4] = "i|(í|ì|ỉ|ị|ĩ)";
        //    pattern[5] = "y|(ý|ỳ|ỷ|ỵ|ỹ)";
        //    pattern[6] = "d|đ";

        //    for (int i = 0; i < pattern.Length; i++)
        //    {

        //        // kí tự sẽ thay thế

        //        char replaceChar = pattern[i][0];

        //        MatchCollection matchs = Regex.Matches(text, pattern[i]);

        //        foreach (Match m in matchs)
        //        {

        //            text = text.Replace(m.Value[0], replaceChar);

        //        }

        //    }

        //    return text;

        //}

        public static bool CheckVietLienKhongDau(string code)
        {
            string varCheck = "á|ả|à|ạ|ã|ă|ắ|ẳ|ằ|ặ|ẵ|â|ấ|ẩ|ầ|ậ|ẫ|ó|ỏ|ò|ọ|õ|ô|ố|ổ|ồ|ộ|ỗ|ơ|ớ|ở|ờ|ợ|ỡ|é|è|ẻ|ẹ|ẽ|ê|ế|ề|ể|ệ|ễ|ú|ù|ủ|ụ|ũ|ư|ứ|ừ|ử|ự|ữ|í|ì|ỉ|ị|ĩ|ý|ỳ|ỷ|ỵ|ỹ|đ| ";
            var arrCheck = varCheck.Split('|');
            for (int i = 0; i < arrCheck.Count(); i++)
            {
                if (code.Contains(arrCheck[i]))
                {
                    return false;//trùng
                }
            }
            return true;
        }
        public static string ConvertToMoney(int? UnitCost)
        {
            string money = "";
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            money = double.Parse(Convert.ToDouble(UnitCost).ToString()).ToString("#,###", cul.NumberFormat);
            return money;
        }
        public static string UnsignCharacter(string s)
        {
            try
            {
                if (!string.IsNullOrEmpty(s))
                {
                    string stFormD = s.Normalize(NormalizationForm.FormD);
                    StringBuilder sb = new StringBuilder();
                    for (int ich = 0; ich < stFormD.Length; ich++)
                    {
                        System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                        if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                        {
                            sb.Append(stFormD[ich]);
                        }
                    }
                    sb = sb.Replace('Đ', 'D');
                    sb = sb.Replace('đ', 'd');

                    return (sb.ToString().Normalize(NormalizationForm.FormD));
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return s;
            }
        }

        /// <summary>
        /// Giới hạn số lượng từ trong một chuỗi
        /// </summary>
        /// <param name="fullText">Chuỗi truyền vào</param>
        /// <param name="maxLength">số lượng từ</param>
        /// <returns>Chuỗi đã giới hạn từ</returns>
        public static string TruncateByWord(string fullText, int maxLength)
        {
            if (fullText == null || fullText.Length < maxLength)
                return fullText;
            var iNextSpace = fullText.LastIndexOf(" ", maxLength, StringComparison.Ordinal);
            return string.Format("{0}...", fullText.Substring(0, (iNextSpace > 0) ? iNextSpace : maxLength).Trim());
        }

        public static string RemoveMark(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Trim();
                if (str.Length > 200)
                    str = str.Substring(0, 200);
                // var charsToRemove = new string[] { "@", ",","+","-", ";", "'", "/", "\\", "\"", "[", "]", "?", "%" };
                var charsToRemove = new string[] { "@", ",", "#", "!", "$", "^", "&", "(", ")", "{", "}", "+", "=", "*", "!", "-", ";", "'", "/", "\\", "\"", "[", "]", "?", "%", ":", "”", "“" };
                foreach (var c in charsToRemove)
                {
                    str = str.Replace(c, string.Empty);
                }
                str = str.Replace(' ', '-');
                //const string FindText = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ :&";
                //const string ReplText = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY---";
                //int index = -1;
                //char[] arrChar = FindText.ToCharArray();
                //while ((index = str.IndexOfAny(arrChar)) != -1)
                //{
                //    int index2 = FindText.IndexOf(str[index]);
                //    str = str.Replace(str[index], ReplText[index2]);
                //}
                str = UnsignCharacter(str);
            }
            return str;
        }


        // <summary>
        /// Giới hạn số lượng ký tự trong một chuỗi
        /// </summary>
        /// <param name="fullText">Chuỗi truyền vào</param>
        /// <param name="maxLength">số lượng ký tự</param>
        /// <returns>Chuỗi đã giới hạn ký tự</returns>
        public static string TruncateByChar(string fullText, int maxLength)
        {
            return fullText.Length > maxLength ? fullText.Substring(0, maxLength) + "..." : fullText;
        }
    }
}