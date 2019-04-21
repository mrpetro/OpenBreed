using OpenBreed.Game.Entities.Components;
using System;
using System.Collections.Generic;

namespace OpenBreed.Game.Entities
{
    public interface IEntity
    {
        #region Public Properties

        List<IEntityComponent> Components { get; }

        Guid Guid { get; }

        #endregion Public Properties

        #region Public Methods

        void Initialize();

        #endregion Public Methods
    }
}