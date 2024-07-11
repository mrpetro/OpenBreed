using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Tools.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void ConfigureCommonTools(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<AbtaPasswordEncoder>();
            });
        }

        #endregion Public Methods
    }
}