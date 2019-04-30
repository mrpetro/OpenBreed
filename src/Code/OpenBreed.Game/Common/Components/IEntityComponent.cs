using OpenBreed.Game.Common;
using OpenBreed.Game.Entities;
using System;

namespace OpenBreed.Game.Common.Components
{
    public interface IEntityComponent
    {
        #region Public Properties

        Type SystemType { get; }

        #endregion Public Properties

        #region Public Methods

        void Deinitialize(IEntity entity);

        void Initialize(IEntity entity);

        #endregion Public Methods
    }
}