using OpenBreed.Game.Common;
using System;

namespace OpenBreed.Game.Common.Components
{
    public interface IEntityComponent
    {
        #region Public Properties

        Type SystemType { get; }

        #endregion Public Properties

        #region Public Methods

        void Deinitialize(IWorldSystem system);

        void Initialize(IWorldSystem system);

        #endregion Public Methods
    }
}