using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common.Interface;
using OpenBreed.Editor.UI.WinForms.Controls.Actions;
using OpenBreed.Editor.UI.WinForms.Controls.DataSources;
using OpenBreed.Editor.UI.WinForms.Controls.EntityTemplates;
using OpenBreed.Editor.UI.WinForms.Controls.Images;
using OpenBreed.Editor.UI.WinForms.Controls.Maps;
using OpenBreed.Editor.UI.WinForms.Controls.Scripts;
using OpenBreed.Editor.UI.WinForms.Controls.Sounds;
using OpenBreed.Editor.UI.WinForms.Controls.Sprites;
using OpenBreed.Editor.UI.WinForms.Controls.Texts;
using OpenBreed.Editor.UI.WinForms.Controls.Tiles;
using OpenBreed.Editor.UI.WinForms.Views;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.VM.DataSources;
using OpenBreed.Editor.VM.EntityTemplates;
using OpenBreed.Editor.VM.Images;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Scripts;
using OpenBreed.Editor.VM.Sounds;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Editor.VM.Texts;
using OpenBreed.Editor.VM.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.WinForms.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void ConfigurePcmPlayer(this IHostBuilder hostBuilder)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                hostBuilder.ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IPcmPlayer, OpenBreed.Common.Windows.PcmPlayer>();
                });
            }
            else
            {
                throw new PlatformNotSupportedException(nameof(IPcmPlayer));
            }
        }

        #endregion Public Methods
    }
}