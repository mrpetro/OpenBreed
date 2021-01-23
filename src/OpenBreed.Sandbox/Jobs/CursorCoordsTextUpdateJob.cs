using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Sandbox.Jobs
{
    public class CursorCoordsTextUpdateJob : IJob
    {
        #region Private Fields

        private Entity entity;

        #endregion Private Fields

        #region Public Constructors

        public CursorCoordsTextUpdateJob(Entity entity)
        {
            this.entity = entity;
        }

        #endregion Public Constructors

        #region Public Properties

        public Action<IJob> Complete { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Execute()
        {
        }

        public void Update(float dt)
        {
            var cursorEntity = entity.Core.GetManager<IEntityMan>().GetByTag("MouseCursor").First();

            var cursorPos = cursorEntity.Get<PositionComponent>();

            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, $"Cursor: ({cursorPos.Value.X.ToString("0.00", CultureInfo.InvariantCulture)},{cursorPos.Value.Y.ToString("0.00", CultureInfo.InvariantCulture)})"));
        }

        public void Dispose()
        {
        }

        #endregion Public Methods

    }
}
