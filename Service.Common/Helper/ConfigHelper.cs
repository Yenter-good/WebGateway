using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Service.Common
{
    public class ConfigHelper
    {
        private static Lazy<IConfigurationRoot> _instance = new Lazy<IConfigurationRoot>(() => BuildConfig());

        private static IConfigurationRoot BuildConfig()
        {
            List<string> appsettingPaths = new List<string>();

            var rootPath = Assembly.GetExecutingAssembly().Location;
            var files = new DirectoryInfo(Path.GetDirectoryName(rootPath)).GetFiles("*.json");
            foreach (var file in files)
            {
                if (file.Name.StartsWith("appsettings"))
                    appsettingPaths.Add(file.Name);
            }

            var builder = new ConfigurationBuilder();
            foreach (var appsettingPath in appsettingPaths)
            {
                builder.Add(new JsonConfigurationSource()
                {
                    Path = appsettingPath,
                    ReloadOnChange = true
                });
            }

            builder.Add(new JsonConfigurationSource()
            {
                Path = @"Config\config.json",
                ReloadOnChange = true
            }); 
     
             return builder.Build();
        }

        public static IConfigurationRoot Instance
        {
            get
            {
                return _instance.Value;
            }
        }
    }
}
