using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenBreed.Wecs.Core.Components.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupWecsCommonComponents(this IHostBuilder hostBuilder)
        {
            XmlComponentsList.RegisterAllAssemblyComponentTypes();
        }

        #endregion Public Methods
    }
}