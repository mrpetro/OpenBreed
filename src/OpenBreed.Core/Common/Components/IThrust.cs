using OpenTK;

namespace OpenBreed.Core.Common.Systems.Components
{
    /// <summary>
    /// Thrust entity component class that can be used to store entity current thrust information
    /// Example: Actor is applied with specific thrust vector to move in specific direction
    /// </summary>
    public interface IThrust : IEntityComponent
    {
        #region Public Properties

        /// <summary>
        /// Thrust value
        /// </summary>
        Vector2 Value { get; set; }

        #endregion Public Properties
    }
}