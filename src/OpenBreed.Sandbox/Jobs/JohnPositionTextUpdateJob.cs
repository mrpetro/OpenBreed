using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenBreed.Core.Managers;
using OpenTK;
using OpenBreed.Wecs.Systems.Rendering.Extensions;

namespace OpenBreed.Sandbox.Jobs
{
    public class JohnPositionTextUpdateJob : IJob
    {
        private readonly ICore core;
        #region Private Fields

        private Entity entity;

        #endregion Private Fields

        #region Public Constructors

        public JohnPositionTextUpdateJob(ICore core, Entity entity)
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
            var johnEntity = core.GetManager<IEntityMan>().GetByTag("John").FirstOrDefault();

            if (johnEntity == null)
                return;

            var playerPos = johnEntity.Get<PositionComponent>();

            var pos = playerPos.Value;
            var indexPosX = (int)pos.X / 16;
            var indexPosY = (int)pos.Y / 16;

            entity.SetText(0, $"Player Pos: ({pos.X.ToString("0.00", CultureInfo.InvariantCulture)},{pos.Y.ToString("0.00", CultureInfo.InvariantCulture)}) ({indexPosX}, {indexPosY})");
        }

        public void Dispose()
        {
        }

        #endregion Public Methods

    }
}
