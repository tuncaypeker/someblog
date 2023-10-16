Console uygulamasi icerisinde  

Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .Enrich.WithProperty("Application", "SomeBlog.RequestLogProcessor")
              .MinimumLevel.Information()
              .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
              .MinimumLevel.Override("System", LogEventLevel.Information)
              .WriteTo.Debug()
              .WriteTo.Elasticsearch(
                  new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(
                      new Uri("http://localhost:9200/"))
                  {
                      CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                      AutoRegisterTemplate = true,
                      TemplateName = "serilog-events-template",
                      IndexFormat = "someblog-log-{0:yyyy.MM.dd}"
                  })
              .CreateLogger();

var logger = (ILogger<Program>)serviceProvider.GetService(typeof(ILogger<Program>));
logger.Warning("{method} için post gelmedi", "test");