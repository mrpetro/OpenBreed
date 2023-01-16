using OpenBreed.Wecs.Components.Common;
using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Components.Control
{
    /// <summary>
    /// Component which stores information about controlled entity
    /// </summary>
    public class ControllerComponent : IEntityComponent
    {
        #region Public Properties

        public int ControlledEntityId { get; set; }

        #endregion Public Properties
    }
}