using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Editor.UI.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Mvc.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEditorUIMvc(this IServiceCollection services)
        {
            services.AddTransient<TileStampEditorController>();
        }
    }
}
