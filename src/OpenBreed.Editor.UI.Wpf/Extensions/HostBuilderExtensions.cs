﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Editor.UI.Wpf.Palettes;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Palettes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Wpf.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void ConfigureControlFactory(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IControlFactory>((sp) =>
                {
                    var controlFactory = new ControlFactory();
                    controlFactory.RegisterWpfControls();
                    return controlFactory;
                });
            });
        }

        #endregion Public Methods
    }
}