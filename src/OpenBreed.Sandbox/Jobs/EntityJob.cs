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

        private void SetPosition(IEntity entity, int entryId)
        {
            var pair = new WorldGatePair() { Id = entryId };

            var entryEntity = entity.Core.Entities.GetByTag(pair).FirstOrDefault();

            if (entryEntity == null)
                throw new Exception($"No entry with id '{pair.Id}' found.");

            var entryPos = entryEntity.GetComponent<PositionComponent>();
            var entryAabb = entryEntity.GetComponent<BodyComponent>().Aabb;
            //var entityAabb = entity.GetComponent<IShapeComponent>().First().Aabb;
            var entityPos = entity.GetComponent<PositionComponent>();

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

            entity.Subscribe<EntityEnteredWorldEventArgs>(OnEntityEntered);
            world.PostCommand(new AddEntityCommand(world.Id, entity.Id));

            SetPosition(entity, entryId);
        }

        private void OnEntityEntered(object sender, EntityEnteredWorldEventArgs e)
        {
            Complete(this);
        }

        #endregion Private Methods
    }
}