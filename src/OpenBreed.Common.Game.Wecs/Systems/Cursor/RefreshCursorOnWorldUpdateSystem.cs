using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Gui.Components;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Systems.Cursor
{
    [RequireEntityWith(typeof(CursorInputComponent))]
    internal class RefreshCursorOnWorldUpdateSystem : IUpdatableSystem
    {
        #region Public Methods

        public void Update(IEnumerable<IEntity> entities, IUpdateContext context)
        {
            foreach (var entity in entities)
            {
                var pos = entity.GetPosition();

                FormattableString text = $"({pos.X:0.0}, {pos.Y:0.0})";

                entity.SetText(0, text.ToString(CultureInfo.InvariantCulture));
            }
        }

        #endregion Public Methods
    }
}