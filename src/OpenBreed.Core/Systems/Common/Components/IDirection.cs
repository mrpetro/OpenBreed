using OpenTK;

namespace OpenBreed.Core.Systems.Common.Components
{
    /// <summary>
    /// Direction entity component class that can be used to store entity current direction information
    /// Example: Actor is facing particular direction when standing
    /// </summary>
    public interface IDirection : IEntityComponent
    {
        #region Public Properties

        /// <summary>
        /// Direction value
        /// </summary>
        Vector2 Value { get; set; }

        #endregion Public Properties
    }
}