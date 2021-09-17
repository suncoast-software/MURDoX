using MURDoX.Config;
using MURDoX.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services
{
    public class DataService : IDataService
    {
        public ConfigJson GetApplicationConfig()
        {
            var configFile = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "/Config/config.json");
            using var fs = File.OpenRead(configFile);
            using var sr = new StreamReader(fs, new UTF8Encoding(false));
            string json =  sr.ReadToEnd();

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            return configJson;
        }
    }
}
