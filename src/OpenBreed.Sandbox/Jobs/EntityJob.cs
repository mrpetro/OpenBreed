using OpenBreed.Core;

using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Physics.Commands;
using OpenBreed.Sandbox.Entities.WorldGate;
using OpenBreed.Sandbox.States;
using OpenBreed.Sandbox.Worlds;
using OpenTK;
using System;
using System.Linq;

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
            entity.Subscribe(CoreEventTypes.ENTITY_LEFT_WORLD, OnEntityRemovedFromWorld);

            //entity.RemovedFromWorld += Entity_RemovedFromWorld;
            entity.World.RemoveEntity(entity);
        }

        private void OnEntityRemovedFromWorld(object sender, EventArgs e)
        {
            Complete(this);
        }

        //private void Entity_RemovedFromWorld(object sender, Core.Common.World e)
        //{
        //    Complete(this);
        //}

        private void SetPosition(IEntity entity, int entryId)
        {
            var pair = new WorldGatePair() { Id = entryId };

            var entryEntity = entity.Core.Entities.GetByTag(pair).FirstOrDefault();

            if (entryEntity == null)
                throw new Exception($"No entry with id '{pair.Id}' found.");

            var entryPos = entryEntity.Components.OfType<Position>().First();
            var entryAabb = entryEntity.Components.OfType<IShapeComponent>().First().Aabb;
            //var entityAabb = entity.Components.OfType<IShapeComponent>().First().Aabb;
            var entityPos = entity.Components.OfType<Position>().First();

            //var offset = new Vector2((32 - entityAabb.Width) / 2.0f, (32 - entityAabb.Height) / 2.0f);

            entityPos.Value = entryPos.Value;// + offset;
        }

        private void EnterWorld(string worldName, int entryId)
        {
            var world = entity.Core.Worlds.GetByName(worldName);

            if (world == null)
            {
                using (var reader = new TxtFileWorldReader(entity.Core, $".\\Content\\Maps\\{worldName}.txt"))
                    world = reader.GetWorld();
            }

            entity.Subscribe(CoreEventTypes.ENTITY_ENTERED_WORLD, OnEntityAddedToWorld);
            world.AddEntity(entity);

            SetPosition(entity, entryId);
        }

        private void OnEntityAddedToWorld(object sender, EventArgs e)
        {
            Complete(this);
        }

        private void OnBodyOff(object sender, EventArgs e)
        {
            Complete(this);
        }

        private void OnBodyOn(object sender, EventArgs e)
        {
            Complete(this);
        }

        private void BodyOff()
        {
            entity.Subscribe(PhysicsEventTypes.BODY_OFF, OnBodyOff);
            entity.PostCommand(new BodyOffCommand(entity.Id));
        }

        private void BodyOn()
        {
            entity.Subscribe(PhysicsEventTypes.BODY_ON, OnBodyOn);
            entity.PostCommand(new BodyOnCommand(entity.Id));
        }

        #endregion Private Methods
    }
}