using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.States;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenBreed.Core.Entities
{
    /// <summary>
    /// Entity interface implementation
    /// </summary>
    public class Entity : IEntity
    {
        #region Private Fields

        private readonly List<IEntityComponent> components = new List<IEntityComponent>();

        #endregion Private Fields

        #region Public Constructors

        public Entity(ICore core)
        {
            Core = core ?? throw new ArgumentNullException(nameof(core));
            Components = new ReadOnlyCollection<IEntityComponent>(components);
        }

        #endregion Public Constructors

        #region Public Properties

        public ReadOnlyCollection<IEntityComponent> Components { get; }

        public ICore Core { get; }

        public World World { get; private set; }

        public int Id { get; internal set; }

        public object DebugData { get; set; }

        public EntityPerform PerformDelegate { get; set; }

        public SystemEventDelegate HandleSystemEvent { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void PostMessage(IEntityMsg message)
        {
            if (message.Type == StateChangeMsg.TYPE && PerformDelegate != null)
            {
                var stateChangeMsg = (StateChangeMsg)message;
                PerformDelegate.Invoke(stateChangeMsg.StateId, stateChangeMsg.Args);
            }
            else
            {
                if (World != null)
                    World.PostMsg(this, message);
            }
        }

        public void Add(IEntityComponent component)
        {
            components.Add(component);
        }

        public virtual void EnterWorld(World world)
        {
            world.AddEntity(this);
        }

        public virtual void LeaveWorld()
        {
            World.RemoveEntity(this);
        }

        public bool Remove(IEntityComponent component)
        {
            return components.Remove(component);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Deinitialize()
        {
            //Deinitialize all entity components
            //for (int i = 0; i < Components.Count; i++)
            //    Components[i].Deinitialize(this);

            //Forget the world in which entity was
            World = null;
        }

        internal void Initialize(World world)
        {
            //Remember in what world entity is
            World = world;

            //Initialize all entity components
            //for (int i = 0; i < Components.Count; i++)
            //    Components[i].Initialize(this);
        }

        #endregion Internal Methods
    }
}