namespace OpenBreed.Wecs.Components.Control
{
    public class ActionControlComponent : IEntityComponent
    {
        #region Public Constructors

        public ActionControlComponent()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public int Primiary { get; set; }
        public int Secondary { get; set; }

        #endregion Public Properties
    }
}