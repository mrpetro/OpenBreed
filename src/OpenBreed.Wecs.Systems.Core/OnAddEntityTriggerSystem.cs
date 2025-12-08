using Microsoft.Extensions.Logging;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Services;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Core
{
    [RequireEntityWith(typeof(OnTriggerComponent))]
    public class OnAddEntityTriggerSystem : IOnAddEntitySystem, IMatchingSystem
    {
        #region Private Fields

        private readonly IEntityTriggerMan entityTriggerMan;

        #endregion Private Fields

        #region Public Constructors

        public OnAddEntityTriggerSystem(
            IEntityTriggerMan entityTriggerMan)
        {
            this.entityTriggerMan = entityTriggerMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnAddEntity(IWorld world, IEntity entity)
        {
            entityTriggerMan.TryOnTrigger("EnterWorld", entity, entity);
        }

        #endregion Public Methods
    }
}