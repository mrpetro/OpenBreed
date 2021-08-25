﻿using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.WorldGate;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System;

namespace OpenBreed.Sandbox.Loaders
{
    internal class LevelEntryCellLoader : IMapWorldEntityLoader
    {
        #region Private Fields

        private readonly ActorHelper actorHelper;
        private readonly WorldGateHelper worldGateHelper;

        #endregion Private Fields

        #region Internal Constructors

        internal LevelEntryCellLoader(ActorHelper actorHelper, WorldGateHelper worldGateHelper)
        {
            this.actorHelper = actorHelper;
            this.worldGateHelper = worldGateHelper;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Load(MapLayoutModel layout, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            var entryId = -1;

            if (actionValue == 56)
                entryId = 0;

            worldGateHelper.AddWorldEntry(world, ix, iy, entryId);

            //actorHelper.AddHero(world, ix, iy);
        }

        #endregion Public Methods
    }
}