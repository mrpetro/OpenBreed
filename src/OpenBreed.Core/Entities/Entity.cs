﻿using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Common.Components;
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

        private EntityMan manager;

        private List<IEntityComponent> components = new List<IEntityComponent>();

        #endregion Private Fields

        #region Public Constructors

        public Entity(EntityMan manager)
        {
            this.manager = manager ?? throw new ArgumentNullException(nameof(manager));

            Components = new ReadOnlyCollection<IEntityComponent>(components);

            Guid = Core.Entities.GetGuid();

            Core.Entities.AddEntity(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public ReadOnlyCollection<IEntityComponent> Components { get; }

        public ICore Core { get { return manager.Core; } }

        public World World { get; private set; }

        public Guid Guid { get; }

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