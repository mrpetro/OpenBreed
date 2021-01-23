using OpenBreed.Wecs.Components.Common;
using OpenTK;

namespace OpenBreed.Wecs.Components.Control
{
    /// <summary>
    /// Placeholder class for walking controls
    /// </summary>
    public class WalkingControl : IControlComponent
    {
        #region Public Fields

        public const string TYPE = "Walking";

        #endregion Public Fields

        #region Public Properties

        public Vector2 Direction { get; set; }

        public string Type => TYPE;

        #endregion Public Properties
    }
}