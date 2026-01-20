using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Systems.Cursor
{
    internal class RefreshCursorOnWorldUpdateSystem : IOnWorldUpdateActionSystem
    {
        #region Public Properties

        public string TriggerName => "UpdateWorld";

        public string ActionName => "RefreshCursor";

        #endregion Public Properties

        #region Public Methods

        public void OnUpdate(IEntity entity, IWorld world)
        {
            var pos = entity.GetPosition();

            FormattableString text = $"({pos.X:0.0}, {pos.Y:0.0})";

            entity.SetText(0, text.ToString(CultureInfo.InvariantCulture));
        }

        #endregion Public Methods
    }
}