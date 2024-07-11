using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Physics.Extensions;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace OpenBreed.Wecs.Systems.Physics
{
    [RequireEntityWith(
        typeof(CollisionComponent))]
    public class UpdateDynamicBodySystem : MatchingSystemBase<UpdateDynamicBodySystem>, IUpdatableSystem
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Internal Constructors

        internal UpdateDynamicBodySystem(
            IEventsMan eventsMan,
            IEntityMan entityMan,
            IWorldMan worldMan)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
        }

        public int PhaseId => throw new System.NotImplementedException();

        #endregion Internal Constructors

        #region Public Methods

        public void Update(IUpdateContext context)
        {
            foreach (var entity in entities)
            {
                if (entity.WorldId != context.WorldId)
                    continue;

                entity.UpdateDynamics(entityMan);
            }
        }

        #endregion Public Methods
    }
}