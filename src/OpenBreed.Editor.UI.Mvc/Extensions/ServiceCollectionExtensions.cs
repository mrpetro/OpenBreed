using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Editor.UI.Mvc.Controllers;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Editor.UI.Mvc.Views;
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
            services.AddTransient<AnimationEditorModel>();
            services.AddTransient<TileStampEditorController>();
            services.AddTransient<AnimationCurvesEditorController>();
            services.AddTransient<AnimationPreviewController>();
            services.AddTransient<AnimationCurvesEditorView>();
            services.AddTransient<AnimationPreviewView>();
            services.AddTransient<TileStampEditorView>();

            services.AddSingleton<IAnimationSandboxFactory, AnimationSandboxFactory>();
            services.AddScoped<IAnimationSandbox, AnimationSandbox>();
        }
    }
}
