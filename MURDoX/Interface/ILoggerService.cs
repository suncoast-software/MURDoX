using MURDoX.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Interface
{
    interface ILoggerService
    {
        public void Save(Log log);
        public void Delete(string Id);
        public IEnumerable<Log> GetAll();
        public Log Get(string Id);
    }
}
