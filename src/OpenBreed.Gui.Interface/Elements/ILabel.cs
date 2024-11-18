namespace OpenBreed.Gui.Interface.Elements
{
    public enum HorizontalAlignment
    {
        Center,
        Left,
        Right
    }

    public enum VerticalAlignment
    {
        Center,
        Bottom,
        Top
    }

    public interface ILabel : IElement
    {
        #region Public Properties

        HorizontalAlignment HorizontalAlignment { get; set; }
        VerticalAlignment VerticalAlignment { get; set; }

        string Text { get; set; }

        #endregion Public Properties
    }
}