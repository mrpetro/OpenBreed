using OpenBreed.Core.Commands;
using OpenBreed.Core.Builders;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Builders;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Core;

namespace OpenBreed.Sandbox.Entities
{
    public static class TextHelper
    {
        public static void RemoveLastChar(World world)
        {
        }

        public static Entity CreateText(World world)
        {
            var e = world.Core.Entities.Create();
            e.Add(new TextCaretComponent());
            e.Add(new TextDataComponent("This is test"));

            var font = world.Core.Rendering.Fonts.Create("Consolas", 20);

            e.Add(new TextPresentationComponent(font.Id, Color4.White, 200.0f));
            e.Add(PositionComponent.Create(-200.0f,50.0f));
            return e;
        }
    }
}
