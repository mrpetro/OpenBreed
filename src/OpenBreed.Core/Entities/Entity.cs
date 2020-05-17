using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace OpenBreed.Core.Entities
{
    /// <summary>
    /// Entity interface implementation
    /// </summary>
    internal class Entity : IEntity
    {
        #region Private Fields

        private readonly List<IEntityComponent> components = new List<IEntityComponent>();

        #endregion Private Fields

        #region Internal Constructors

        internal Entity(ICore core, List<IEntityComponent> initialComponents)
        {
            Core = core ?? throw new ArgumentNullException(nameof(core));

            components = initialComponents ?? new List<IEntityComponent>();
            Components = new ReadOnlyCollection<IEntityComponent>(components);
        }

        #endregion Internal Constructors

        #region Public Properties

        public ReadOnlyCollection<IEntityComponent> Components { get; }

        public ICore Core { get; }

        /// <summary>
        /// Property for user purpose data
        /// </summary>
        public object Tag { get; set; }

        public int Id { get; internal set; }

        public object DebugData { get; set; }

        #endregion Public Properties

        #region Public Methods

        public T TryGetComponent<T>()
        {
            return components.OfType<T>().FirstOrDefault();
        }

        public bool Contains<T>()
        {
            return components.OfType<T>().Any();
        }

        public T GetComponent<T>()
        {
            return components.OfType<T>().First();
        }

        public void PostCommand(ICommand command)
        {
            Core.Commands.Post(this, command);
        }

        public void RaiseEvent<T>(T eventArgs) where T : EventArgs
        {
            Core.Events.Raise(this, eventArgs);
        }

        public void Subscribe<T>(Action<object, T> callback) where T : EventArgs
        {
            Core.Events.Subscribe(this, callback);
        }

        public void Unsubscribe<T>(Action<object, T> callback) where T : EventArgs
        {
            Core.Events.Unsubscribe(this, callback);
        }

        public void Add(IEntityComponent component)
        {
            Debug.Assert(component != null, "Adding null component to entity is forbidden.");
            Debug.Assert(!components.Any(item => item.GetType() == component.GetType()), "Adding two components of same type to one entity is forbidden.");
            components.Add(component);
        }

        public bool Remove(IEntityComponent component)
        {
            return components.Remove(component);
        }

        public override string ToString()
        {
            return $"Entity({Id})";
        }

        #endregion Public Methods

        #region Internal Methods


        #endregion Internal Methods
    }
}