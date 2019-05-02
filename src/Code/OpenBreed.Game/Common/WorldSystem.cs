using OpenBreed.Game.Common.Components;
using System.Collections.ObjectModel;

namespace OpenBreed.Game.Common
{
    public abstract class WorldSystem<T> : IWorldSystem where T : IEntityComponent
    {
        //private readonly List<T> components = new List<T>();

        #region Protected Constructors

        protected WorldSystem()
        {
            //Components = new ReadOnlyCollection<T>(components);
        }

        #endregion Protected Constructors

        #region Protected Properties

        protected ReadOnlyCollection<T> Components { get; }

        #endregion Protected Properties

        #region Public Methods

        public void AddComponent(IEntityComponent component)
        {
            AddComponent((T)component);
        }

        public virtual void Deinitialize(World world)
        {
        }

        public virtual void Initialize(World world)
        {
        }

        public void RemoveComponent(IEntityComponent component)
        {
            RemoveComponent((T)component);
        }

        public virtual void Update(float dt)
        {
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract void AddComponent(T component);

        protected abstract void RemoveComponent(T component);

        #endregion Protected Methods
    }
}