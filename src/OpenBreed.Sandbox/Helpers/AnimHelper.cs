using OpenBreed.Wecs.Systems.Control;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Helpers
{
    public static class AnimHelper
    {
        public static string ToDirectionName(Vector2 dir)
        {
            dir = MovementTools.SnapToCompass8Way(dir);

            if (dir.X == 1 && dir.Y == 0)
                return "Right";
            else if (dir.X == 1 && dir.Y == -1)
                return "RightDown";
            else if (dir.X == 0 && dir.Y == -1)
                return "Down";
            else if (dir.X == -1 && dir.Y == -1)
                return "DownLeft";
            else if (dir.X == -1 && dir.Y == 0)
                return "Left";
            else if (dir.X == -1 && dir.Y == 1)
                return "LeftUp";
            else if (dir.X == 0 && dir.Y == 1)
                return "Up";
            else if (dir.X == 1 && dir.Y == 1)
                return "UpRight";
            else
                throw new InvalidOperationException($"Unable to translate direction '{dir}' to name.");
        }
    }
}
