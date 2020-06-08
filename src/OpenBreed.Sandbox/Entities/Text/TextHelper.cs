using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Builders;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities
{
    public static class TextHelper
    {
        public static void RemoveLastChar(World world)
        {
        }

        public static IEntity CreateCaret(World world)
        {
            var e = world.Core.Entities.Create();
            e.Add(new TextCaretComponent());
            e.Add(new TextDataComponent());
            return e;
        }

        public static void AddChar(World world, char text)
        {
            var font = world.Core.Rendering.Fonts.Create("Consolas", 20);

            var e = world.Core.Entities.Create();

            e.Add(PositionComponentBuilder.New(world.Core).Build());

            var textBuilder = TextComponentBuilder.New(world.Core);
            textBuilder.SetProperty("FontId", font.Id);
            textBuilder.SetProperty("Offset", Vector2.Zero);
            textBuilder.SetProperty("Color", Color4.White);
            textBuilder.SetProperty("Text", text);
            textBuilder.SetProperty("Order", 100.0f);

            e.Add(textBuilder.Build());
            world.PostCommand(new AddEntityCommand(world.Id, e.Id));
        }

    }
}
