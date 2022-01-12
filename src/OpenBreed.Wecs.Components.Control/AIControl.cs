using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components;
using OpenTK;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Components.Control
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

        public string Type => throw new System.NotImplementedException();

        #endregion Public Properties
    }
}