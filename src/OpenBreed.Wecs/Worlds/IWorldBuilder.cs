using OpenBreed.Wecs.Systems;
using System;

namespace OpenBreed.Wecs.Worlds
{
    public interface IWorldBuilder
    {
        #region Public Methods

        IWorld Build();

        IWorldBuilder AddSystem<TSystem>() where TSystem : ISystem;
        IWorldBuilder AddModule<TModule>(TModule module);
        IWorldBuilder SetName(string name);
        IWorldBuilder SetSize(int width, int height);

        #endregion Public Methods
    }
}