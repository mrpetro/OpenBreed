using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Services;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Entities.Builders
{
    public class EntityBuilder : IEntityBuilder
    {
        #region Private Fields

        private readonly EntityMan entityMan;
        private readonly List<IEntityComponent> components = new List<IEntityComponent>();
        private string tag;
        private int classId = 0;

        #endregion Private Fields

        #region Internal Constructors

        internal EntityBuilder(EntityMan entityMan)
        {
            this.entityMan = entityMan;
        }

        #endregion Internal Constructors

        #region Public Methods

        public IEntityBuilder AddComponent<TEntityComponent>(TEntityComponent component) where TEntityComponent : IEntityComponent
        {
            components.Add(component);
            return this;
        }

        public IEntity Build()
        {
            var newEntity = new Entity(entityMan, tag, classId, components);
            entityMan.Register(newEntity);
            return newEntity;
        }

        public IEntityBuilder SetTag(string tag)
        {
            this.tag = tag;
            return this;
        }

        public IEntityBuilder SetClass(int classId)
        {
            this.classId = classId;
            return this;
        }

        #endregion Public Methods
    }
}