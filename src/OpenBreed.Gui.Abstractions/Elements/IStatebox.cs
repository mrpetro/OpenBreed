namespace OpenBreed.Gui.Abstractions.Elements
{
    public interface IStatebox : IElement
    {
        #region Public Properties

        public bool IsPressed { get; }

        public bool IsChecked { get; }

        #endregion Public Properties
    }
}