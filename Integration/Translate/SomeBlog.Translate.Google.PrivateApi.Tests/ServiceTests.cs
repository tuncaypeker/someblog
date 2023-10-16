using System;
using Xunit;

namespace SomeBlog.Translate.Google.PrivateApi.Tests
{
    public class ServiceTests
    {
        [Fact]
        public void Translate_En_Tr()
        {
            var service = new Service();
            var sampleText = "Potential assistance from the Chinese would be a significant development in Russia's invasion. " +
                "It could upend the hold Ukrainian forces still have in the country as well as provide a counterweight to the harsh sanctions imposed on Russia's economy.";
            var responseModel = service.Translate("en", "tr", sampleText);

            Assert.True(responseModel.sentences.Count == 2);
            Assert.Contains("Çin'den gelecek olası yardım", responseModel.sentences[0].trans);
        }

        [Fact]
        public void Translate_Tr_En()
        {
            var service = new Service();
            var sampleText = "Çin'den gelecek olasý yardým, Rusya'nýn iþgalinde önemli bir geliþme olacaktýr. " +
                "Bu, Ukrayna kuvvetlerinin ülkede hâlâ sahip olduðu hakimiyeti alt üst edebilir ve Rusya ekonomisine uygulanan sert yaptýrýmlara karþý bir denge saðlayabilir..";
            var responseModel = service.Translate("tr", "en", sampleText);

            Assert.True(responseModel.sentences.Count == 2);
            Assert.Contains("Possible assistance from China", responseModel.sentences[0].trans);
        }
    }
}
