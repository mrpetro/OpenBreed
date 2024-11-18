namespace OpenBreed.Gui.Interface.Elements
{
    public interface ICheckbox : IElement
    {
        #region Public Properties

        public bool IsPressed { get; }

        public bool IsChecked { get; }

        #endregion Public Properties
    }
}