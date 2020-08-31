namespace OpenBreed.Common.Model.Texts.Builders
{
    internal class TextBuilder
    {
        #region Internal Fields

        internal string Text;
        internal string Name;

        #endregion Internal Fields

        #region Public Methods

        public static TextBuilder NewTextModel()
        {
            return new TextBuilder();
        }

        public TextModel Build()
        {
            return new TextModel(this);
        }

        public TextBuilder SetText(string text)
        {
            Text = text; 
            return this;
        }

        public TextBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        #endregion Public Methods
    }
}