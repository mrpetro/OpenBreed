using OpenBreed.Core.Systems.Common.Components;
using OpenTK;
using System.Collections.Generic;

namespace OpenBreed.Core.Systems.Control.Components
{
    public class AiControl : IEntityComponent
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

        #endregion Public Properties
    }
}