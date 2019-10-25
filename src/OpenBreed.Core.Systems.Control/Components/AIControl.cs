﻿using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenTK;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Components
{
    public class AiControl : IControlComponent
    {
        #region Public Constructors

        public AiControl()
        {
            Waypoints = new List<Vector2>();
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Direction { get; set; }
        public List<Vector2> Waypoints { get; }

        public string Type => throw new System.NotImplementedException();

        #endregion Public Properties
    }
}