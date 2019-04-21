using OpenBreed.Game.Common;
using System;

namespace OpenBreed.Game.Entities.Components
{
    public interface IEntityComponent
    {
        #region Public Properties

        Type SystemType { get; }

        #endregion Public Properties

        #region Public Methods

        void Deinitialize(IComponentSystem system);

        void Initialize(IComponentSystem system);

        #endregion Public Methods
    }
}