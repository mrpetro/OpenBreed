using OpenBreed.Core.Entities;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Control;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Core.Systems.Control.Systems;
using OpenBreed.Core.Systems.Movement;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Core.Systems.Control.Components
{
    public class AiControl : IEntityComponent
    {
        #region Private Fields

        private IPosition position;

        #endregion Private Fields

        #region Public Constructors

        public AiControl()
        {
            Waypoints = new List<Vector2>();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<Vector2> Waypoints { get; }

        #endregion Public Properties
    }
}