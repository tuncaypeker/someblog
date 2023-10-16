using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace SomeBlog.Infrastructure.Extensions
{
    public static class TextExtensions
    {
        public static string ToStripHtml(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            return Regex.Replace(value, "<.*?>", String.Empty);
        }

        public static string ToSlug(this string phrase)
        {
            if (phrase == null)
                phrase = "";

            //clear punctuation
            string str = phrase.ClearPunctuation()
                .Replace("İ", "i")
                .Replace("|", "")
                .Replace("-", " ") //icinde hali hazirda - varsa pespepe --- olma ihtimali var "Gerek Yok - Uzmanlığa" => gerek-yok---uzmanliga
                .ToLowerInvariant();

            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();

            // cut and trim 
            str = Regex.Replace(str, @"\s", "-"); //hyphens  

            str = str.Replace("ı", "i")
                    .Replace("ğ", "g")
                    .Replace("ö", "o")
                    .Replace("ü", "u")
                    .Replace("ş", "s")
                    .Replace("ş", "s")
                    .Replace("ç", "c");

            return str;
        }

        private static string ClearPunctuation(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var exludeChars = new List<char>() { '-' };
            var list = new List<Char>();
            foreach (char c in value)
            {
                if (Char.IsPunctuation(c) && !exludeChars.Contains(c))
                    continue;

                list.Add(c);
            }

            value = string.Concat(list.ToArray());

            return value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="removeRules"></param>
        /// <param name="isXPath"> isXpath alanı true,false olarak removeRules'daki degerin xpath olup olmadigini verir</param>
        /// <returns></returns>
        public static string ToExecuteRemoveRules(this string value, List<string> removeRules, List<bool> isXPath)
        {
            for (int i = 0; i < removeRules.Count; i++)
            {
                if (isXPath[i])
                {
                    HtmlDocument htmldoc = new HtmlDocument();
                    htmldoc.LoadHtml(value);

                    var removeNodes = htmldoc.DocumentNode.SelectNodes(removeRules[i]);
                    if (removeNodes == null || removeNodes.Count == 0)
                        continue;

                    foreach (HtmlNode removeNode in removeNodes.ToList())
                    {
                        removeNode.ParentNode.ReplaceChild(HtmlNode.CreateNode(""), removeNode);
                    }

                    value = htmldoc.DocumentNode.InnerHtml;
                }
                else
                {
                    value = value.Replace(removeRules[i], "");
                }
            }

            return value;
        }

        public static string ToExecuteReplaceRules(this string value, List<string> whatReplaceRules, List<string> withReplaceRules)
        {
            for (int i = 0; i < whatReplaceRules.Count; i++)
            {
                value = value.Replace(whatReplaceRules[i], withReplaceRules[i]);
            }

            return value;
        }

        public static string ToRemoveAnchors(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            return Regex.Replace(value, @"<a\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1");
        }

        public static string RemoveUnicodeCharacter(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            value = value.Replace(@"\ç", "ç")
                        .Replace(@"\u00e7", "ç")
                        .Replace(@"\u00c7", "Ç")
                        .Replace(@"\Ç", "Ç")
                        .Replace(@"\u015f", "ş")
                        .Replace(@"\ş", "ş")
                        .Replace(@"\u0131", "ı")
                        .Replace(@"\ı", "ı")
                        .Replace(@"\u022b", "ö")
                        .Replace(@"\ö", "ö")
                        .Replace(@"\u022a", "Ö")
                        .Replace(@"\Ö", "Ö")
                        .Replace(@"\u011e", "Ğ")
                        .Replace(@"\Ğ", "Ğ")
                        .Replace(@"\u011f", "ğ")
                        .Replace(@"\ğ", "ğ");

            value = value.Replace(@"\ç", "ç");

            return value;
        }

        public static string ToHtmlDecode(this string text)
        {
            return WebUtility.HtmlDecode(text);
        }

        public static string ToTrim(this string value, int length)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            if (value.Length <= length)
                return value;

            return value.Substring(0, length);
        }

        public static int ToWordCount(this string value)
        {
            var valueHtmlRemoved = value.ToStripHtml();

            while (valueHtmlRemoved.Contains("\t\t")) { valueHtmlRemoved = valueHtmlRemoved.Replace("\t\t", "\t"); }
            while (valueHtmlRemoved.Contains("\n\t")) { valueHtmlRemoved = valueHtmlRemoved.Replace("\n\t", "\n"); }
            while (valueHtmlRemoved.Contains("\n\n")) { valueHtmlRemoved = valueHtmlRemoved.Replace("\n\n", "\n"); }

            valueHtmlRemoved = valueHtmlRemoved.Replace("\n", " ");
            while (valueHtmlRemoved.Contains("  ")) { valueHtmlRemoved = valueHtmlRemoved.Replace("  ", " "); }

            var charArr = new char[] { '.', '?', '!', ' ', ';', ':', ',' };
            string[] source = valueHtmlRemoved.Split(charArr, StringSplitOptions.RemoveEmptyEntries).Where(x=>!string.IsNullOrWhiteSpace(x)).ToArray();

            int wordCount = source.Count();

            return wordCount;
        }
    }
}
