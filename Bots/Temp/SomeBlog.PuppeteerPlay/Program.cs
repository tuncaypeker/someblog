using PuppeteerSharp;
using System;
using System.Collections.Generic;

namespace SomeBlog.PuppeteerPlay
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Test();

            Console.ReadLine();
        }

        static async void Test()
        {
            int waitInSeconds = 1000;
            var path = $"https://www.google.com/search?q=fnerbahçe";

            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            using (Browser browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = false }))
            using (Page page = await browser.NewPageAsync())
            {
                await page.SetViewportAsync(new ViewPortOptions
                {
                    Width = 1920,
                    Height = 1080
                });

                var listTemp = new List<string>();

                await page.SetRequestInterceptionAsync(true);
                page.Request += (sender, e) =>
                {
                    if (e.Request.ResourceType == ResourceType.Image || e.Request.ResourceType == ResourceType.Xhr)
                    {
                        e.Request.AbortAsync();
                        return;
                    }
                    e.Request.ContinueAsync();
                    listTemp.Add($"[{e.Request.ResourceType}] => {e.Request.Url}");
                };

                //https://stackoverflow.com/questions/65971972/puppeteer-sharp-get-html-after-js-finished-running
                //Just replacing navigation with WaitUntilNavigation.Networkidle2 worked to wait until Javascript is finished to excute.
                await page.GoToAsync(path, WaitUntilNavigation.Networkidle0);

                System.Threading.Thread.Sleep(waitInSeconds);

                await page.ClickAsync(".related-question-pair");
            }
        }
    }
}


//https://www.kiltandcode.com/puppeteer-sharp-crawl-the-web-using-csharp-and-headless-chrome/
//await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
/*
using (Browser browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
{
    var country = "TR";
    var path = $"https://trends.google.com/trends/trendingsearches/realtime?geo={country}&category=all";

    using (Page page = await browser.NewPageAsync())
    {
        await page.GoToAsync(path);

        string content = await page.GetContentAsync();

        await page.ScreenshotAsync($"screenshot.png");
        await browser.CloseAsync();
    }
}

/*
await page.GoToAsync("https://www.bing.com/maps");
await page.WaitForSelectorAsync(".searchbox input");
await page.FocusAsync(".searchbox input");
await page.Keyboard.TypeAsync("CN Tower, Toronto, Ontario, Canada");
await page.ClickAsync(".searchIcon");
await page.WaitForNavigationAsync();

await page.WaitForSelectorAsync("#ytd-player");

var texts = await page.EvaluateExpressionAsync("let elements = document.getElementsByClassName('ytp-play-button ytp-button'); alert(elements.length)");

string content = await page.GetContentAsync();

// Change the size of the view port to simulate the iPhone X
await page.SetViewportAsync(new ViewPortOptions
{
    Width = 1125,
    Height = 2436
});
*/
