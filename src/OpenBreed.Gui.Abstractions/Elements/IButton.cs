namespace OpenBreed.Gui.Abstractions.Elements
{
    public interface IButton : IElement
    {
        #region Public Properties

        public bool IsPressed { get; }

        #endregion Public Properties

        #region Public Methods

        public void Press();

        public void Release();

        #endregion Public Methods
    }
}