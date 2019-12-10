using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Common
{
    public class AppSettingsHelper
    {
        private IConfiguration configuration;
        public AppSettingsHelper(string path)
        {
            var builder = new ConfigurationBuilder();

            string extension = Path.GetExtension(path);

            switch (extension)
            {
                case ".json":
                    configuration = builder.AddJsonFile(path, optional: false, reloadOnChange: true).Build();
                    break;
                case ".xml":
                    configuration = builder.AddXmlFile(path, optional: false, reloadOnChange: true).Build();
                    break;
                case ".ini":
                    configuration = builder.AddIniFile(path, optional: false, reloadOnChange: true).Build();
                    break;
                default:
                    throw new ApplicationException("不支持的配置文件，仅支持.json、.xml、.ini类型的文件。");
            }
        }

        public AppSettingsHelper() : this(path: "appsettings.json")
        {

        }

        public string GetValueBy(string key)
        {
            string value = configuration[key];
            return value;
        }
    }
}
