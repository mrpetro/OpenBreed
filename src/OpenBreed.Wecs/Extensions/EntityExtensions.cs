using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Extensions
{
    public static class EntityExtensions
    {
        #region Public Methods

        public static bool HasWorld(this IEntity entity)
        {
            return entity.WorldId != WecsConsts.NO_WORLD_ID;
        }

        #endregion Public Methods
    }
}