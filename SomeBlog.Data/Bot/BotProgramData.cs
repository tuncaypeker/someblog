namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Data.Infrastructure.Entities;
    using SomeBlog.Infrastructure.Interfaces;

    public class BotProgramData : EntityBaseData<Model.BotProgram>
    {
        public BotProgramData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }

        public DataResult UpdatePulse(string name)
        {
            var botProgram = FirstOrDefault(x => x.Name == name);
            if (botProgram == null)
                return new DataResult(false, "Bot bulunamadı");

            botProgram.LastPulse = System.DateTime.Now;

            return Update(botProgram);
        }
    }
}
