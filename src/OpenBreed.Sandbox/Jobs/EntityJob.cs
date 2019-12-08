using OpenBreed.Core;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Physics.Messages;
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
            entity.RemovedFromWorld += Entity_RemovedFromWorld;
            entity.World.RemoveEntity(entity);
        }

        private void Entity_RemovedFromWorld(object sender, Core.Common.World e)
        {
            Complete(this);
        }

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

            entity.AddedToWorld += Entity_AddedToWorld;
            world.AddEntity(entity);


            SetPosition(entity, entryId);
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
            entity.PostMsg(new BodyOffMsg(entity.Id));
        }

        private void BodyOn()
        {
            entity.Subscribe(BodyOnEvent.TYPE, OnBodyOn);
            entity.PostMsg(new BodyOnMsg(entity.Id));
        }

        #endregion Private Methods
    }
}