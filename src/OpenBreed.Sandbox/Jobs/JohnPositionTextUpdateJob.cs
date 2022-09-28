using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using System;
using System.Globalization;
using System.Linq;

namespace OpenBreed.Sandbox.Jobs
{
    public class JohnPositionTextUpdateJob : IJob
    {
        #region Private Fields

        private readonly IEntityMan entityMan;

        private Entity entity;

        #endregion Private Fields

        #region Public Constructors

        public JohnPositionTextUpdateJob(IEntityMan entityMan, Entity entity)
        {
            this.entityMan = entityMan;
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
            //var johnEntity = entityMan.GetByTag("John").FirstOrDefault();

            //if (johnEntity == null)
            //    return;

            //var playerPos = johnEntity.Get<PositionComponent>();

            //var pos = playerPos.Value;
            //var indexPosX = (int)pos.X / 16;
            //var indexPosY = (int)pos.Y / 16;

            //entity.SetText(0, $"Player Pos: ({pos.X.ToString("0.00", CultureInfo.InvariantCulture)},{pos.Y.ToString("0.00", CultureInfo.InvariantCulture)}) ({indexPosX}, {indexPosY})");
        }

        public void Dispose()
        {
        }

        #endregion Public Methods
    }
}