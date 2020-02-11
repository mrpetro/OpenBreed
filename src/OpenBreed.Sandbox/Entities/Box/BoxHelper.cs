using OpenBreed.Core;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Box
{
    public static class BoxHelper
    {
        public static IEntity CreateBox(ICore core, Vector2 size, Vector2 position, Vector2 velocity)
        {
            var ball = core.Entities.Create();

            ball.Add(core.Rendering.CreateWireframe(1.0f, Color4.Red));
            ball.Add(Position.Create(position));
            ball.Add(Thrust.Create(0, 0));
            ball.Add(Velocity.Create(velocity));
            ball.Add(Direction.Create(1, 0));
            ball.Add(AxisAlignedBoxShape.Create(-size.X / 2, -size.Y / 2, size.X, size.Y));
            ball.Add(new MotionComponent());
            ball.Add(BodyComponent.Create(1.0f, 1.0f, null));

            return ball;
        }
    }
}
