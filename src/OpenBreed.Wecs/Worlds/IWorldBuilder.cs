using OpenBreed.Wecs.Systems;
using System;

namespace OpenBreed.Wecs.Worlds
{
    public interface IWorldBuilder
    {
        #region Public Methods

        IWorld Build();

        IWorldBuilder AddSystem<TSystem>() where TSystem : ISystem;
        IWorldBuilder SetName(string name);

        #endregion Public Methods
    }
}