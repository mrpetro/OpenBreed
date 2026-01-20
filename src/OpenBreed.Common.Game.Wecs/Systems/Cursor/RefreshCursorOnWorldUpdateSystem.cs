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
    internal class RefreshCursorOnWorldUpdateSystem : IEntityOnTriggerActionSystem
    {
        public string TriggerName => "UpdateWorld";

        public string ActionName => "RefreshCursor";

        public void OnTrigger(IEntity triggeringEntity, IEntity triggerEntity)
        {
            var pos = triggeringEntity.GetPosition();

            FormattableString text = $"({pos.X:0.0}, {pos.Y:0.0})";

            triggeringEntity.SetText(0, text.ToString(CultureInfo.InvariantCulture));
        }
    }
}
