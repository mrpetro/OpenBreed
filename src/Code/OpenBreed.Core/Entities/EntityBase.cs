using OpenBreed.Core.Systems.Common.Components;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Entities
{
    public abstract class EntityBase : IEntity
    {
        #region Protected Constructors

        protected EntityBase(ICore core)
        {
            Core = core ?? throw new ArgumentNullException(nameof(core));

            Guid = Core.EntityMan.GetGuid();

            Core.EntityMan.AddEntity(this);
        }

        #endregion Protected Constructors

        #region Public Properties

        public List<IEntityComponent> Components { get; } = new List<IEntityComponent>();
        public Guid Guid { get; }
        public ICore Core { get; }

        #endregion Public Properties
    }
}