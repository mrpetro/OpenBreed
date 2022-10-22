using OpenBreed.Wecs.Components.Common;
using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Components.Control
{
    /// <summary>
    /// Placeholder class for walking controls
    /// </summary>
    public class WalkingControlComponent : IEntityComponent
    {
        #region Public Properties

        public Vector2 Direction { get; set; }

        public int ControlledEntityId { get; set; }

        #endregion Public Properties
    }
}