using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace SomeBlog.CssOptimizer
{
    /// <summary>
    /// https://github.com/thoqbk/css-optimizer
    /// </summary>
    public class CSSOptimizer
    {
        private string openBracket = "\\{";
        private string closeBracket = "\\}";
        private string notOpenAndCloseBracketPattern;
        private string singleRulePattern;
        private string mediaRulePattern;
        private string targetFilePath;
        private string resultFilePath;

        private HashSet<string> htmlFiles = new HashSet<string>();
        private HashSet<string> usedClassNames = new HashSet<string>();
        private HashSet<string> usedIdNames = new HashSet<string>();
        private HashSet<string> usedTagNames = new HashSet<string>();

        private Regex rulePattern;
        private Regex commentPattern;
        private Regex mediaRuleBodyPattern;
        private Regex classNamePattern;
        private Regex idNamePattern;
        private Regex tagNamePattern;
        private Regex selectorPattern;
        private Regex isCSSMediaRulePattern;
        private Regex isNonCSSStyleRulePattern;

        private bool ShouldMinify;

        public CSSOptimizer(string targetFilePath, string resultFilePath, bool shouldMinify = false)
        {
            this.targetFilePath = targetFilePath;
            this.resultFilePath = resultFilePath;
            this.ShouldMinify = shouldMinify;

            notOpenAndCloseBracketPattern = "[^" + openBracket + closeBracket + "]";
            singleRulePattern = notOpenAndCloseBracketPattern + "+" + openBracket + notOpenAndCloseBracketPattern + "*?" + closeBracket;
            mediaRulePattern = "@media" + notOpenAndCloseBracketPattern + "+" + openBracket + "(" + singleRulePattern + ")*.*?" + closeBracket;

            rulePattern = new Regex("(" + mediaRulePattern + "|" + singleRulePattern + ")", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            commentPattern = new Regex("\\/\\*.*?\\*\\/", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            mediaRuleBodyPattern = new Regex(openBracket + "((?:" + singleRulePattern + ")*.*?)" + closeBracket, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            classNamePattern = new Regex("\\.([_a-zA-Z]+[_a-zA-Z0-9-]*)");
            idNamePattern = new Regex("\\#([_a-zA-Z]+[_a-zA-Z0-9-]*)");
            tagNamePattern = new Regex("(?:^|\\s)([a-zA-Z][a-zA-Z0-9]*)");
            selectorPattern = new Regex("(" + notOpenAndCloseBracketPattern + "+)" + openBracket, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            isCSSMediaRulePattern = new Regex("\\s*@media");
            isNonCSSStyleRulePattern = new Regex("\\s*@|(::)|\\*|(:root)");
        }

        public void AddHtmlFile(string path)
        {
            this.htmlFiles.Add(path);
        }

        public void KeepTagName(string tagName)
        {
            usedTagNames.Add(tagName.ToLower());
        }

        /// <summary>
        /// className basinda "." olmadan verilmelidir
        /// </summary>
        /// <param name="className"></param>
        public void KeepClassName(string className)
        {
            usedClassNames.Add(className);
        }

        public void Optimize()
        {
            //Parse input html files and extract used class and tag
            ExtractUsedClassNamesNTagNames();
            string targetFileContent = ReadFileContent(targetFilePath);
            List<CSSRule> cssRules = ExtractCSSRules(targetFileContent);

            //debug
            var result = BuildResult(cssRules);
            System.IO.File.WriteAllText(resultFilePath, result);
        }

        private void ExtractUsedClassNamesNTagNames()
        {
            foreach (var inputFilePath in htmlFiles)
            {
                var inputFileContent = ReadFileContent(inputFilePath);
                ExtractUsedClassNamesNTagNames(inputFileContent);
            }
        }

        private void ExtractUsedClassNamesNTagNames(string htmlContent)
        {
            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(htmlContent);

            var elements = document.DocumentNode.ChildNodes;

            ExtractUsedClassNamesNTagNamesFromElements(elements);
        }

        private void ExtractUsedClassNamesNTagNamesFromElements(HtmlNodeCollection elements)
        {
            foreach (HtmlNode element in elements)
            {
                if (element.Name.ToLower() == "#text")
                    continue;

                var innerElements = element.ChildNodes;
                if (innerElements.Count > 0)
                    ExtractUsedClassNamesNTagNamesFromElements(innerElements);

                var attributeClass = element.Attributes["class"];
                if (attributeClass != null)
                {
                    var classNames = attributeClass.Value.Split(" ");
                    foreach (string className in classNames)
                    {
                        if (!string.IsNullOrEmpty(className) && !string.IsNullOrWhiteSpace(className))
                            usedClassNames.Add(className);
                    }
                }

                var attributeId = element.Attributes["id"];
                if (attributeId != null)
                {
                    var idNames = attributeId.Value.Split(" ");
                    foreach (string idName in idNames)
                    {
                        if (!string.IsNullOrEmpty(idName) && !string.IsNullOrWhiteSpace(idName))
                            usedIdNames.Add(idName);
                    }
                }

                usedTagNames.Add(element.Name.ToLower());
            }
        }

        private string ReadFileContent(string filePath)
        {
            StringBuilder retVal = new StringBuilder();
            bool remoteFile = filePath.ToLower().StartsWith("http") || filePath.ToLower().StartsWith("https");
            if (remoteFile)
            {
                using (var client = new WebClient())
                {
                    var html = client.DownloadString(filePath);
                    return html;
                }
            }
            else
            {
                if (!System.IO.File.Exists(filePath))
                    throw new Exception("File Not Found");

                return System.IO.File.ReadAllText(filePath);
            }
        }

        private string RemoveContentByPattern(string content, Regex regex)
        {
            string retVal = content;
            bool found = true;
            while (found)
            {
                if (regex.IsMatch(retVal))
                {
                    var matcher = regex.Match(retVal);
                    StringBuilder retValBuilder = new StringBuilder();
                    int start = matcher.Index;
                    int end = matcher.Index + matcher.Length;
                    if (start > 0)
                        retValBuilder.Append(retVal.Substring(0, start));

                    retValBuilder.Append(retVal.Substring(end));
                    retVal = retValBuilder.ToString();
                }
                else
                {
                    found = false;
                }
            }
            return retVal;
        }

        protected List<string> MatchAll(string inputstring, Regex pattern, int group)
        {
            List<string> retVal = new List<string>();

            if (pattern.IsMatch(inputstring))
            {
                var collection = pattern.Matches(inputstring);
                for (int i = 0; i < collection.Count; i++)
                {
                    var match = collection[i];

                    retVal.Add(match.Groups[group].Value);
                }
            }

            //return
            return retVal;
        }

        private string ExtractMediaRuleBody(string ruleContent)
        {
            List<string> matchedstrings = MatchAll(ruleContent, mediaRuleBodyPattern, 1);
            return matchedstrings[0];
        }
       
        private List<CSSRule> ExtractCSSRules(string rulesInstring)
        {
            //debug
            List<CSSRule> retVal = new List<CSSRule>();
            string stdContent = RemoveContentByPattern(rulesInstring, commentPattern);
            List<string> cssRulestrings = MatchAll(stdContent, rulePattern, 0);
            foreach (string cssRulestring in cssRulestrings)
            {
                List<string> matchedstrings = MatchAll(cssRulestring, selectorPattern, 1);
                if (matchedstrings.Count == 0)
                    throw new Exception("Invalid css rule: " + stdContent);

                string selector = matchedstrings[0];
                if (isCSSMediaRulePattern.IsMatch(cssRulestring))
                {
                    string ruleBody = ExtractMediaRuleBody(cssRulestring);
                    List<CSSRule> children = ExtractCSSRules(ruleBody);
                    CSSMediaRule mediaRule = new CSSMediaRule(cssRulestring);
                    mediaRule.getChildren().AddRange(children);
                    mediaRule.Selector = selector;

                    //save
                    retVal.Add(mediaRule);
                }
                else if (isNonCSSStyleRulePattern.IsMatch(cssRulestring))
                {
                    NonCSSStyleRule nonCSSStyleRule = new NonCSSStyleRule(cssRulestring);
                    nonCSSStyleRule.Selector = selector;
                    retVal.Add(nonCSSStyleRule);
                }
                else
                {
                    CSSStyleRule cssStyleRule = new CSSStyleRule(cssRulestring);
                    
                    cssStyleRule.Selector = selector;

                    List<string> classNames = MatchAll(selector, classNamePattern, 1);
                    cssStyleRule.ClassNames.UnionWith(classNames);

                    List<string> tagNames = MatchAll(selector, tagNamePattern, 1);
                    cssStyleRule.TagNames.UnionWith(tagNames.Select(x=>x.ToLower()));

                    List<string> idNames = MatchAll(selector, idNamePattern, 1);
                    cssStyleRule.IdNames.UnionWith(idNames.Select(x => x.ToLower()));

                    //save
                    retVal.Add(cssStyleRule);
                }
            }
            //return
            return retVal;
        }

        private string BuildResult(List<CSSRule> cssRules)
        {
            StringBuilder retVal = new StringBuilder();
            foreach (CSSRule cssRule in cssRules)
            {
                if (cssRule.GetType() == typeof(CSSMediaRule))
                {
                    string mediaRuleResult = BuildResult(((CSSMediaRule)cssRule).getChildren());
                    if (!string.IsNullOrWhiteSpace(mediaRuleResult))
                    {
                        retVal.Append(ShouldMinify
                            ? cssRule.Selector.Replace("\r\n", "").Replace("  ", " ")
                            : cssRule.Selector);

                        retVal.Append("{");
                        if (!ShouldMinify) retVal.Append("\n");
                        retVal.Append(mediaRuleResult);
                        retVal.Append("}");
                        if (!ShouldMinify) retVal.Append("\n");
                    }
                }
                else if (cssRule.GetType() == typeof(NonCSSStyleRule))//if rule is grouping rule but is not a media rule
                {
                    retVal.Append(ShouldMinify
                            ? cssRule.Content.Replace("\r\n", "").Replace("  ", " ")
                            : cssRule.Content);
                    if (!ShouldMinify) retVal.Append("\n");
                }
                else
                {
                    CSSStyleRule styleRule = (CSSStyleRule)cssRule;
                    if (styleRule.ClassNames.Count > 0 && styleRule.TagNames.Count > 0)// example rule: * {}
                    {
                        retVal.Append(ShouldMinify 
                            ? styleRule.Content.Replace("\r\n", "").Replace("  ", " ")
                            : styleRule.Content);

                        if (!ShouldMinify) retVal.Append("\n");
                    }
                    else
                    {
                        bool isUsedRule = false;
                        foreach (string className in styleRule.ClassNames)
                        {
                            if (usedClassNames.Contains(className))
                            {
                                isUsedRule = true;
                                break;
                            }
                        }

                        foreach (string idName in styleRule.IdNames)
                        {
                            if (usedIdNames.Contains(idName))
                            {
                                isUsedRule = true;
                                break;
                            }
                        }

                        if (!isUsedRule)
                        {
                            foreach (string tagName in styleRule.TagNames)
                            {
                                if (usedTagNames.Contains(tagName))
                                {
                                    isUsedRule = true;
                                    break;
                                }
                            }
                        }
                        if (isUsedRule)
                        {
                            retVal.Append(ShouldMinify
                            ? styleRule.Content.Replace("\r\n", "").Replace("  "," ")
                            : styleRule.Content);

                            if (!ShouldMinify) retVal.Append("\n");
                        }
                    }
                }
            }
            return retVal.ToString();
        }
    }
}