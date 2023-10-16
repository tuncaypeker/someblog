using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace SomeBlog.Translate.Bing.UI
{
    public class Translator
    {
        private bool headless;

        public Translator(bool headless = false)
        {
            this.headless = headless;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceLang">tr</param>
        /// <param name="targetLang">en</param>
        /// <param name="text">Merhaba</param>
        public string Translate(string sourceLang, string targetLang, string text)
        {
            if (System.IO.File.Exists("chromedriver.exe"))
                System.IO.File.Delete("chromedriver.exe");

            var wait_time = 20;
            var chromeOptions = new ChromeOptions();
            if (headless)
            {
                chromeOptions.AddArguments(new List<string>() {
                    "--silent-launch",
                    "--no-startup-window",
                    "no-sandbox",
                    "headless",
                    "--lang=" + targetLang
                });
            }



            new DriverManager().SetUpDriver(new ChromeConfig());
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            ChromeDriver driver = new ChromeDriver(chromeDriverService, chromeOptions);

            driver.Navigate().GoToUrl("https://www.bing.com/translator?text=" + text);

            System.Threading.Thread.Sleep(10 * 1000);

            var search = driver.FindElement(By.Id("tta_output_ta"), wait_time);
            var asd = search.GetAttribute("value");

            return "";
        }
    }

    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }

            return driver.FindElement(by);
        }
    }
}
