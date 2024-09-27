using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Editor.UI.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Mvc.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void ConfigureEditorUIMvc(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddEditorUIMvc();
            });
        }

        #endregion Public Methods
    }
}