using SomeBlog.Infrastructure.Interfaces;

namespace SomeBlog.Infrastructure.Logging.SerilogElasticLog
{
    public class Logger<T> : ILogger<T>
    {
        public void Debug(string message)
        {
            Serilog.Log.Debug(message);
        }
        public void Debug(string message, T t)
        {
            Serilog.Log.Debug(message);
        }
        public void Debug(string message, params object[] propertyValues)
        {
            Serilog.Log.Debug(message, propertyValues);
        }

        public void Error(string message)
        {
            Serilog.Log.Error(message);
        }
        public void Error(string message, T t)
        {
            Serilog.Log.Error(message);
        }
        public void Error(string message, params object[] propertyValues)
        {
            Serilog.Log.Error(message, propertyValues);
        }

        public void Fatal(string message)
        {
            Serilog.Log.Fatal(message);
        }
        public void Fatal(string message, T t)
        {
            Serilog.Log.Fatal(message);
        }
        public void Fatal(string message, params object[] propertyValues)
        {
            Serilog.Log.Fatal(message, propertyValues);
        }

        public void Information(string message)
        {
            Serilog.Log.Information(message);
        }
        public void Information(string message, T t)
        {
            Serilog.Log.Information(message);
        }
        public void Information(string message, params object[] propertyValues)
        {
            Serilog.Log.Information(message, propertyValues);
        }

        public void Verbose(string message)
        {
            Serilog.Log.Verbose(message);
        }
        public void Verbose(string message, T t)
        {
            Serilog.Log.Verbose(message);
        }
        public void Verbose(string message, params object[] propertyValues)
        {
            Serilog.Log.Verbose(message, propertyValues);
        }

        public void Warning(string message)
        {
            Serilog.Log.Warning(message);
        }
        public void Warning(string message, T t)
        {
            Serilog.Log.Warning(message);
        }
        public void Warning(string message, params object[] propertyValues)
        {
            Serilog.Log.Warning(message, propertyValues);
        }
    }
}
