﻿using OpenBreed.Core.Common;
using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Events
{
    /// <summary>
    /// Event args for event that occurs when entity left world
    /// </summary>
    public class EntityLeftWorldEventArgs : EventArgs
    {
        #region Public Constructors

        public EntityLeftWorldEventArgs(IEntity entity, World world)
        {
            Entity = entity;
            World = world;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public World World { get; }

        #endregion Public Properties
    }
}
