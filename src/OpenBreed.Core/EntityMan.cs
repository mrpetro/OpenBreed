using OpenBreed.Core.Blueprints;
using OpenBreed.Core.Collections;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core
{
    public class EntityMan
    {
        #region Private Fields

        private static Dictionary<string, Action<object>> setters = new Dictionary<string, Action<object>>();

        public static void AddSetter(string key, Action<object> action)
        {
            setters.Add(key, action);
        }

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

        public IEntity GetById(int id)
        {
            var entity = entities[id];

            if (entities.TryGetValue(id, out entity))
                return entity;
            else
                throw new InvalidOperationException($"Entity with Guid '{id}' not found.");
        }

        public List<IEntity> CreateFromBlueprint(IBlueprint blueprint)
        {
            var list = new List<IEntity>();

            foreach (var entityDef in blueprint.Entities)
            {
                var entity = Create();
                AppendComponents(entity, entityDef);
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

        #endregion Public Methods

        #region Private Methods

        private void SetComponentStates(IEntity entity, IEntityDef def)
        {
            foreach (var componentState in def.ComponentStates)
            {

            }
        }

        private void AppendComponents(IEntity entity, IEntityDef def)
        {
            foreach (var componentType in def.ComponentTypes)
            {
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