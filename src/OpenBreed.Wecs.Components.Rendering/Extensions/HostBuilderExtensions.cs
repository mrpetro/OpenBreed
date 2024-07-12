using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Extensions;

namespace OpenBreed.Wecs.Components.Rendering.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupRenderingComponents(this IHostBuilder hostBuilder)
        {
            XmlComponentsList.RegisterAllAssemblyComponentTypes();
            hostBuilder.SetupAssemblyComponentFactories();
        }

        #endregion Public Methods
    }
}