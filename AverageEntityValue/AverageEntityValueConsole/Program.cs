using Microsoft.Extensions.Configuration;
using Nomnio.AverageEntityValue;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AverageEntityValueConsole
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{SourceContext}] [{Level}] {Message}{NewLine}{Exception}")
                .WriteTo.RollingFile("log-{Date}.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{SourceContext}] [{Level}] {Message}{NewLine}{Exception}")
                .CreateLogger();

            MainAsync(args).GetAwaiter().GetResult();

            Console.WriteLine("press any key");
            Console.ReadKey();
        }

        public static async Task MainAsync(string[] args)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            string ConnectionString;
            string TableName;
            if (args.Length == 2)
            {
                ConnectionString = args[0];
                TableName = args[1];
            }
            else
            {
                ConnectionString = Configuration["ConnectionString"];
                TableName = Configuration["TableName"];
            }

            await CallServices(ConnectionString, TableName);
        }

        static async Task CallServices(string connectionString, string tableName)
        {
            var entityValue = new CalculateEntityValueServices(connectionString, tableName,100);
            var task = entityValue.GetAverage();
            await Task.WhenAll(task);
        }

    }
}