using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MURDoX.Data;
using MURDoX.Interface;
using MURDoX.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Configuration
{
   public class Startup
    {
        static IHostBuilder builder = ConfigureServices();
        public static IHostBuilder ConfigureServices()
        {
           return  builder.ConfigureServices((context, services) => 
            {
                services.AddSingleton<AppDbContext>();
                services.AddSingleton<IMemberService, MemberService>();
                services.AddSingleton<IBankService, BankService>();
            });
 
        }
    }
}
