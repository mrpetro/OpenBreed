using OpenTK;

namespace OpenBreed.Core.Systems.Common.Components
{
    /// <summary>
    /// Velocity entity component class that can be used to store entity current velocity information
    /// Example: Actor is going somewhere with specific velocity vector
    /// </summary>
    public interface IVelocity : IEntityComponent
    {
        #region Public Properties

        /// <summary>
        /// Velocity value
        /// </summary>
        Vector2 Value { get; set; }

        #endregion Public Properties
    }
}