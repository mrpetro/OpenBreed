using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenBreed.Wecs.Components.Common.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupCommonComponents(this IHostBuilder hostBuilder)
        {
            XmlComponentsList.RegisterAllAssemblyComponentTypes();
            hostBuilder.SetupAssemblyComponentFactories();
        }

        #endregion Public Methods
    }
}