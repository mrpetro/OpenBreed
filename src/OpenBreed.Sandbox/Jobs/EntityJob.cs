using OpenBreed.Core;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Physics.Messages;
using OpenBreed.Sandbox.States;
using OpenBreed.Sandbox.Worlds;
using System;

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
                case "LeaveWorld":
                    LeaveWorld();
                    break;

                case "EnterWorld":
                    EnterWorld((string)args[0], (int)args[1]);
                    break;
                case "BodyOff":
                    BodyOff();
                    break;
                case "BodyOn":
                    BodyOn();
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

        private void LeaveWorld()
        {
            entity.RemovedFromWorld += Entity_RemovedFromWorld;
            entity.World.RemoveEntity(entity);
        }

        private void Entity_RemovedFromWorld(object sender, Core.Common.World e)
        {
            Complete(this);
        }

        private void EnterWorld(string worldName, int entryId)
        {
            var world = entity.Core.Worlds.GetByName(worldName);

            if (world == null)
            {
                using (var reader = new TxtFileWorldReader(entity.Core, $".\\Content\\Maps\\{worldName}.txt"))
                    world = reader.GetWorld();
                //world = GameWorldHelper.CreateGameWorld(entity.Core, worldName);

   
                //StateTechDemo5.SetupWorld(world);
            }

            entity.AddedToWorld += Entity_AddedToWorld;
            world.AddEntity(entity);
        }

        private void Entity_AddedToWorld(object sender, Core.Common.World e)
        {
            Complete(this);
        }

        private void OnBodyOff(object sender, IEvent entity)
        {
            Complete(this);
        }

        private void OnBodyOn(object sender, IEvent entity)
        {
            Complete(this);
        }

        private void BodyOff()
        {
            entity.Subscribe(BodyOffEvent.TYPE, OnBodyOff);
            entity.PostMsg(new BodyOffMsg(entity));
        }

        private void BodyOn()
        {
            entity.Subscribe(BodyOnEvent.TYPE, OnBodyOn);
            entity.PostMsg(new BodyOnMsg(entity));
        }

        #endregion Private Methods
    }
}