using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game
{
    public static class EntityNames
    {
        #region Public Fields

        public const string GameViewport = "Viewport.Game";
        public const string DebugHudViewport = $"Viewport.{WorldNames.DebugHud}";
        public const string GameHudViewport = $"Viewport.{WorldNames.GameHud}";
        public const string TextViewport = "Viewport.Text";

        #endregion Public Fields
    }
}