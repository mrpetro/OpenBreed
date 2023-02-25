namespace OpenBreed.Wecs.Components.Control
{
    /// <summary>
    /// Component which stores information about controlled entity
    /// </summary>
    public class ControllerComponent : IEntityComponent
    {
        #region Public Constructors

        public ControllerComponent()
        {
            ControlledEntityId = -1;
        }

        #endregion Public Constructors

        #region Public Properties

        public int ControlledEntityId { get; set; }

        #endregion Public Properties
    }
}