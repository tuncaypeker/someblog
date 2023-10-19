namespace SomeBlog.Bots.CustomBots
{
	public interface ICustomBot
	{
		/// <summary>
		/// Liste olarak bana bulabildigi icerikleri vermelidir, e peki resimlerden content olusturma
		/// daha once eklenmis mi kontrol etme, log atma bu isleri kim yapacak
		/// 
		/// 1- CustomBot olarak ben id'mi biliyorum
		/// 2- CustomBot olarak daha once ekledigim icerikleri biliyorum cunku CustomBotLogs tablosu var
		/// 3- Calısıyorum ve son icerikleri belli bir mantiga gore geiyorum
		/// 4- Hata alirsam bunu logluyorum
		/// 5- Bu icerigi eklediysem bir daha eklemiyorum
		/// 6- Icerik bana geldiyse burdan kurallar ile olusturup veritabanına gonderiyorum
		///		- Burada beni hangi site calistirdi ise onun veritabanına yazıyorum tabi ki
		///		- Burada bir sitede RunCustomJob Job'ı calısır ve db'de SiteCustomJobs tablosuna bakar, foreach ile donerek o custom job'ı factory'den alarak execute methodunu calistirir
		/// </summary>
		void Execute(Model.Blog blog);
	}
}
