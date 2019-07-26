using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Common.Systems
{
    public class GroupSystem : WorldSystem
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly List<GroupPart> groupPartComps = new List<GroupPart>();

        #endregion Private Fields

        #region Public Constructors

        public GroupSystem(ICore core) : base(core)
        {
            Require<GroupPart>();
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            entities.Add(entity);
            groupPartComps.Add(entity.Components.OfType<GroupPart>().First());
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            var index = entities.IndexOf(entity);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
            groupPartComps.RemoveAt(index);
        }

        public override bool HandleMsg(object sender, IMsg message)
        {
            foreach (var item in entities.Where(item => item.Id == ((IEntityMsg)message).Entity.Id))
                item.PostMsg((IEntityMsg)message);

            return true;
        }

        #endregion Protected Methods
    }
}