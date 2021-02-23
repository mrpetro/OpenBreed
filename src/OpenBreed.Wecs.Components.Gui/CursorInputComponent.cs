namespace OpenBreed.Wecs.Components.Gui
{
    public class CursorInputComponent : IEntityComponent
    {
        #region Public Constructors

        public CursorInputComponent(int cursorId)
        {
            CursorId = cursorId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int CursorId { get; set; }

        #endregion Public Properties
    }
}