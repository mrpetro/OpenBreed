using OpenBreed.Game.Entities.Components;
using OpenBreed.Game.States;
using System;
using System.Collections.Generic;

namespace OpenBreed.Game.Entities
{
    public abstract class EntityBase : IEntity
    {
        #region Protected Constructors

        protected EntityBase(GameState core)
        {
            Core = core ?? throw new ArgumentNullException(nameof(core));

            Guid = Core.EntityMan.GetGuid();

            Core.EntityMan.AddEntity(this);
        }

        #endregion Protected Constructors

        #region Public Properties

        public List<IEntityComponent> Components { get; } = new List<IEntityComponent>();
        public Guid Guid { get; }
        public GameState Core { get; }

        #endregion Public Properties
    }
}