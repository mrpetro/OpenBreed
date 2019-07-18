using OpenTK;

namespace OpenBreed.Core.Common.Systems.Components
{
    /// <summary>
    /// Position entity component class that can be used to store entity current position information
    /// Example: Actor is standing somewhere in the world at current position
    /// </summary>
    public interface IPosition : IEntityComponent
    {
        #region Public Properties

        /// <summary>
        /// Position value
        /// </summary>
        Vector2 Value { get; set; }

        #endregion Public Properties
    }
}