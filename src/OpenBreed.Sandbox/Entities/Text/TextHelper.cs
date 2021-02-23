using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Core;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Sandbox.Entities
{
    public static class TextHelper
    {
        public static void RemoveLastChar(World world)
        {
        }

        public static Entity CreateText(World world)
        {
            var e = world.Core.GetManager<IEntityMan>().Create();
            e.Add(new TextCaretComponent());
            e.Add(new TextDataComponent("This is test"));

            var font = world.Core.GetManager<IFontMan>().Create("Consolas", 20);

            e.Add(new TextPresentationComponent(font.Id, Color4.White, 200.0f));
            e.Add(PositionComponent.Create(-200.0f,50.0f));
            return e;
        }
    }
}
