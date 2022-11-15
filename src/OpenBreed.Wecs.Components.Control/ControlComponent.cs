using OpenBreed.Wecs.Components.Common;
using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Components.Control
{
    /// <summary>
    /// Component for storing information about controller entity
    /// </summary>
    public class ControlComponent : IEntityComponent
    {
        #region Public Properties

        public int ControlledEntityId { get; set; }

        #endregion Public Properties
    }
}