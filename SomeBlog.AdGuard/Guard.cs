using Microsoft.Extensions.Options;
using SomeBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SomeBlog.AdGuard
{
    public class Guard : IGuard
    {
        //00:00'da view logları temizlenmeli
        AdViewLogData adViewLogData;
        SomeBlog.AdGuard.CacheHelper cacheHelper;
        AdConfiguration configuration;

        public Guard(AdViewLogData adViewLogData, SomeBlog.AdGuard.CacheHelper cacheHelper, IOptions<AdConfiguration> configuration)
        {
            this.adViewLogData = adViewLogData;
            this.cacheHelper = cacheHelper;
            this.configuration = configuration.Value;
        }

        public Dto.ShouldShowAdDto ShouldShowAds(string ipAddress, int blogId)
        {
            if (!configuration.ShowAds)
                return new Dto.ShouldShowAdDto(false);

            //1-
            //Daha once bot oldugunu bildgimiz ya da baska bir sebep ile engellenmis ip'den biri 
            //ise reklam gosterme
            if (cacheHelper.AdBannedIps.Any(x => x.IpAddress == ipAddress))
                return new Dto.ShouldShowAdDto(false);

            //1.0-
            //local'de ise reklam gosterme ya da tasarımı bozmasın die dummy alan goster
            if (IsLocalIpAddress(ipAddress))
                return new Dto.ShouldShowAdDto(false);

            //1.1- 
            //Bu Ip adresi çin gibi ya da bangladesh gibi ayarlanabiloecek bir ulke ip'si ise, reklam gosterme
            //Bunun ayarının sitede yapılabilmesi lazım sonrası için, sadece şu ülkelere reklam göster gibi bir ayar
            //ornek olarak hazirbilgi sadece türkiye'ye reklam göstermeli

            //2-
            //Bu adam bugun ilk defa reklam talebinde bulundu ise gosterme, [AdViewLog sitename,ip,reqdate,allowed:false]
            //Bu adam bugun ikinci defa reklam talebinde bulundu ise goster [sayfa gezinmistir, die varsayıyoruz]
            //Bu adam bugun ucuncu defa reklam talebinde bulundu ise goster
            //Bu adam bugun dorduncu defa reklam talebinde bulundu ise goster
            //Bu adam bugun besinci defa reklam talebinde bulundu ise orda dur
            //........
            var dateToday = DateTime.Now.Date;
            var adViewsToday = adViewLogData.GetBy(x => x.BlogId == blogId && x.IpAddress == ipAddress && x.Date == dateToday);
            var adViewsTodayCount = adViewsToday.Count;

            if (adViewsTodayCount < configuration.FirstAdAfterHowManyRequest || adViewsTodayCount >= configuration.HowManyAdRequestShow + configuration.FirstAdAfterHowManyRequest)
                return new Dto.ShouldShowAdDto(false);

             var shouldShowAdDto = new Dto.ShouldShowAdDto(true);

            //Build AdCode Counts
            foreach (var adView in adViewsToday)
            {
                if (string.IsNullOrEmpty(adView.AdCodeIds))
                    continue;

                var adCodeIdsShown = adView.AdCodeIds.Split(','); //[1,2,4,6]

                for (int i = 0; i < adCodeIdsShown.Length; i++) {
                    var adCodeId = -1;
                    if (!int.TryParse(adCodeIdsShown[i], out adCodeId))
                        continue;

                    if (shouldShowAdDto.AdCodeViewCounts.ContainsKey(adCodeId))
                    {
                        var count = shouldShowAdDto.AdCodeViewCounts[adCodeId];

                        shouldShowAdDto.AdCodeViewCounts[adCodeId] = count + 1;
                    }
                    else {
                        shouldShowAdDto.AdCodeViewCounts.Add(adCodeId, 1);
                    }
                }
            }
            
            return shouldShowAdDto;
        }

        private bool IsLocalIpAddress(string host)
        {
            try
            {
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                // get local IP addresses
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (IPAddress hostIP in hostIPs)
                {
                    // is localhost
                    if (IPAddress.IsLoopback(hostIP)) return true;
                    // is local address
                    foreach (IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP)) return true;
                    }
                }
            }
            catch { }

            return false;
        }

        public bool ClearBlackList()
        {
            return cacheHelper.AdBannedIpsClear();
        }
    }
}
