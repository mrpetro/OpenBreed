namespace OpenBreed.Gui.Abstractions.Elements
{
    public interface ICheckField : IElement
    {
        #region Public Properties

        public bool IsPressed { get; }

        public bool Value { get; }

        #endregion Public Properties
    }
}