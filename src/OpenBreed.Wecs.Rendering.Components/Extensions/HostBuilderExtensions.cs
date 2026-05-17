using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Extensions;

namespace OpenBreed.Wecs.Rendering.Components.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupWecsRenderingComponents(this IHostBuilder hostBuilder)
        {
            XmlComponentsList.RegisterAllAssemblyComponentTypes();
        }

        #endregion Public Methods
    }
}