using OpenBreed.Core.Blueprints;
using OpenBreed.Core.Collections;
using OpenBreed.Core.Common.Components.Builders;
using OpenBreed.Core.Common.Systems.Components;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Entities
{
    public class EntityMan
    {
        #region Private Fields

        private static Dictionary<string, Func<ICore, ComponentBuilder>> builders = new Dictionary<string, Func<ICore, ComponentBuilder>>();

        private static Dictionary<string, Action<object>> setters = new Dictionary<string, Action<object>>();

        private readonly IdMap<IEntity> entities = new IdMap<IEntity>();

        #endregion Private Fields

        #region Public Constructors

        public EntityMan(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public static void RegisterBuilder(string componentTypeName, Func<ICore, ComponentBuilder> creatorFunc)
        {
            builders.Add(componentTypeName, creatorFunc);
        }

        public static void AddSetter(string key, Action<object> action)
        {
            setters.Add(key, action);
        }

        public IEntity GetById(int id)
        {
            var entity = entities[id];

            if (entities.TryGetValue(id, out entity))
                return entity;
            else
                throw new InvalidOperationException($"Entity with Guid '{id}' not found.");
        }

        public List<IEntity> CreateFromBlueprint(IBlueprint blueprint, Dictionary<string, IComponentState> states = null)
        {
            var list = new List<IEntity>();

            foreach (var entityDef in blueprint.Entities)
            {
                var entity = Create();

                var builders = CreateBuilders(entityDef);

                //AppendComponents(entity, entityDef);
                SetComponentStates(entity, entityDef);
            }

            return list;
        }

        public IEntity Create()
        {
            var newEntity = new Entity(Core);
            newEntity.Id = entities.Add(newEntity);
            return newEntity;
        }

        public void Destroy(IEntity entity)
        {
            entity.RemovedFromWorld += Entity_RemovedFromWorld;
            entity.World.RemoveEntity(entity);
        }

        #endregion Public Methods

        #region Private Methods

        private void Entity_RemovedFromWorld(object sender, Common.World e)
        {
            var entity = (IEntity)sender;
            entity.RemovedFromWorld -= Entity_RemovedFromWorld;
            entities.RemoveById(entity.Id);
        }

        private ComponentBuilder CreateBuilder(string componentType)
        {
            if (builders.TryGetValue(componentType, out Func<ICore, ComponentBuilder> builderCreator))
                return builderCreator.Invoke(Core);
            else
                return null;
        }

        private void SetComponentStates(IEntity entity, IEntityDef def)
        {
            foreach (var componentState in def.ComponentStates)
            {
            }
        }

        private List<ComponentBuilder> CreateBuilders(IEntityDef def)
        {
            var builders = new List<ComponentBuilder>();

            foreach (var componentType in def.ComponentTypes)
            {
                var componentBuilder = CreateBuilder(componentType);

                if (componentBuilder != null)
                    builders.Add(componentBuilder);
            }

            return builders;
        }

        private void AppendComponents(IEntity entity, IEntityDef def)
        {
            foreach (var componentType in def.ComponentTypes)
            {
                var componentBuilder = CreateBuilder(componentType);

                var type = Type.GetType(componentType);

                if (type == null)
                    continue;

                var component = Activator.CreateInstance(type) as IEntityComponent;

                entity.Add(component);
            }
        }

        #endregion Private Methods
    }
}