using MURDoX.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Interface
{
    public interface IDataService
    {
        public ConfigJson GetApplicationConfig();
    }
}
