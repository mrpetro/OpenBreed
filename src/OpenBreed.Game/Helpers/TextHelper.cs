using OpenBreed.Core;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Helpers
{
    public static class TextHelper
    {
        public static IEntityComponent Create(ICore core, Vector2 offset, string text)
        {
            var fontAtlas = core.Rendering.Fonts.Create("Arial", 12);
            return Text.Create(fontAtlas.Id, offset, text);
        }
    }
}
