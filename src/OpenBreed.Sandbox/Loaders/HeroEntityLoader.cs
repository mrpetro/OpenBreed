using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System;

namespace OpenBreed.Sandbox.Loaders
{
    internal class HeroEntityLoader : IMapWorldEntityLoader
    {
        #region Private Fields

        private readonly ActorHelper actorHelper;

        #endregion Private Fields

        #region Internal Constructors

        internal HeroEntityLoader(ActorHelper actorHelper)
        {
            this.actorHelper = actorHelper;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Load(MapLayoutModel layout, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            actorHelper.AddHero(world, ix, iy);
        }

        #endregion Public Methods
    }
}