using Microsoft.Extensions.Configuration;
using System.IO;

namespace ExamAce.Data.Utils
{
    public class Configuration
    {
        public static IConfigurationRoot Config { get; set; }

        static Configuration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Config = builder.Build();
        }
    }
}
