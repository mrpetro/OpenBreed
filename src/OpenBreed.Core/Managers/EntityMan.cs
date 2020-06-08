using NLua;
using OpenBreed.Core.Collections;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Managers
{
    public class EntityMan
    {
        #region Private Fields

        private const string TEMPLATES_NAMESPACE = "Templates.Entities";
        private readonly IdMap<IEntity> entities = new IdMap<IEntity>();

        private readonly Dictionary<string, Func<ICore, IComponentBuilder>> builders = new Dictionary<string, Func<ICore, IComponentBuilder>>();

        #endregion Private Fields

        #region Public Constructors

        public EntityMan(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public int Count
        {
            get
            {
                return entities.Count;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public IEnumerable<IEntity> GetByTag(object tag)
        {
            return entities.Items.Where(item => item.Tag != null && item.Tag.Equals(tag));
        }

        public IEntity GetById(int id)
        {
            if (entities.TryGetValue(id, out IEntity entity))
                return entity;
            else
                return null;
        }

        public void RegisterComponentBuilder(string componentName, Func<ICore, IComponentBuilder> newAction)
        {
            builders.Add(componentName, newAction);
        }

        public IEntity CreateFromTemplate(string templateName)
        {
            Core.Logging.Verbose($"Creating entity from '{templateName}' template.");

            var entityTable = Core.Scripts.GetObject($"{TEMPLATES_NAMESPACE}.{templateName}") as LuaTable;

            if (entityTable == null)
                throw new Exception($"Entity template '{templateName}' not found.");

            var components = new List<IEntityComponent>();
            BuildEntityComponents(entityTable, ref components);

            return Create(components);
        }

        public IEntity Create(List<IEntityComponent> initialComponents = null)
        {
            var newEntity = new Entity(Core, initialComponents);
            newEntity.Id = entities.Add(newEntity);
            return newEntity;
        }

        public void Destroy(IEntity entity)
        {
            entity.World.Subscribe<EntityRemovedEventArgs>(OnEntityRemovedEventArgs);
            entity.World.PostCommand(new RemoveEntityCommand(entity.World.Id, entity.Id));
        }

        #endregion Public Methods

        #region Private Methods

        private void BuildEntityComponents(LuaTable entityTable, ref List<IEntityComponent> components)
        {
            foreach (KeyValuePair<object, object> pair in entityTable)
            {
                var cmpName = pair.Key as string;
                if (cmpName == null)
                    throw new Exception("Expected component name of 'string' type in entity table key.");

                var cmpTable = pair.Value as LuaTable;
                if (cmpTable == null)
                    throw new Exception("Expected component value of 'LuaTable' type in entity table value.");

                AppendComponent(cmpName, cmpTable, ref components);
            }
        }

        private void AppendComponent(string componentName, LuaTable componentTable, ref List<IEntityComponent> components)
        {
            if (!builders.TryGetValue(componentName, out Func<ICore, IComponentBuilder> newAction))
                throw new NotImplementedException($"Builder for entity component '{componentName}' not implemented.");

            var builder = newAction.Invoke(this.Core);

            foreach (KeyValuePair<object, object> pair in componentTable)
            {
                try
                {
                    builder.SetProperty(pair.Key, pair.Value);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Invalid component property with key '{pair.Key}'.", ex);
                }
            }

            components.Add(builder.Build());
        }

        private void OnEntityRemovedEventArgs(object sender, EntityRemovedEventArgs e)
        {
            var entity = (IEntity)sender;
            entity.Unsubscribe<EntityRemovedEventArgs>(OnEntityRemovedEventArgs);
            entities.RemoveById(entity.Id);
        }

        #endregion Private Methods
    }
}