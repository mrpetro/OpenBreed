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
using OpenBreed.Core.Managers;

namespace OpenBreed.Sandbox.Jobs
{
    public class CursorCoordsTextUpdateJob : IJob
    {
        private readonly ICore core;
        #region Private Fields

        private Entity entity;

        #endregion Private Fields

        #region Public Constructors

        public CursorCoordsTextUpdateJob(ICore core, Entity entity)
        {
            this.core = core;
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
            var cursorEntity = core.GetManager<IEntityMan>().GetByTag("MouseCursor").First();

            var cursorPos = cursorEntity.Get<PositionComponent>();

            core.GetManager<ICommandsMan>().Post(new TextSetCommand(entity.Id, 0, $"Cursor: ({cursorPos.Value.X.ToString("0.00", CultureInfo.InvariantCulture)},{cursorPos.Value.Y.ToString("0.00", CultureInfo.InvariantCulture)})"));
        }

        public void Dispose()
        {
        }

        #endregion Public Methods

    }
}
