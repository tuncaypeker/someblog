using SomeBlog.Data.Content;

namespace SomeBlog.Bots.CustomBots
{
	public class CustomBotFactory
	{
		ContentData contentData;

		public CustomBotFactory(ContentData contentData)
		{
			this.contentData = contentData;
		}

		public ICustomBot GetBotById(int id) {
			switch (id) 
			{ 
				case 1: return new AkakceAktuelCustomBot(contentData);
				default: return null;
			}	
		}
	}
}
