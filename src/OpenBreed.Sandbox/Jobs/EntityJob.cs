using OpenBreed.Core;

using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Physics.Commands;
using OpenBreed.Sandbox.Entities.WorldGate;
using OpenBreed.Sandbox.Worlds;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;

namespace OpenBreed.Sandbox.Jobs
{
    public class EntityJob : IJob
    {
        #region Private Fields

        private IEntity entity;
        private string actionName;
        private object[] args;

        #endregion Private Fields

        #region Public Constructors

        public EntityJob(IEntity entity, string actionName, params object[] args)
        {
            this.entity = entity;
            this.actionName = actionName;
            this.args = args;
        }

        #endregion Public Constructors

        #region Public Properties

        public Action<IJob> Complete { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Execute()
        {
            switch (actionName)
            {
                case "EnterWorld":
                    EnterWorld((string)args[0], (int)args[1]);
                    break;
                default:
                    break;
            }
        }

        public void Update(float dt)
        {
        }

        public void Dispose()
        {
        }

        #endregion Public Methods

        #region Private Methods

        private void EnterWorld(string worldName, int entryId)
        {
            var world = entity.Core.Worlds.GetByName(worldName);

            world.Subscribe<EntityAddedEventArgs>(OnEntityAdded);
            //entity.Subscribe<EntityEnteredWorldEventArgs>(OnEntityEntered);
            world.PostCommand(new AddEntityCommand(world.Id, entity.Id));
        }

        private void OnEntityAdded(object sender, EntityAddedEventArgs e)
        {
            if (e.EntityId != entity.Id)
                return;

            ((World)sender).Unsubscribe<EntityAddedEventArgs>(OnEntityAdded);

            Complete(this);
        }

        #endregion Private Methods
    }
}