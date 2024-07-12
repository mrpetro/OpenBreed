using System;

namespace OpenBreed.Core.Events
{
    public class TextDataChanged : EventArgs
    {
        public enum ChangeType
        {
            Inserted,
            Removed
        }

        #region Public Constructors

        public TextDataChanged(string text, ChangeType type, int start, int length)
        {
            Text = text;
            Type = type;
            Start = start;
            Length = length;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Text { get; }

        public ChangeType Type { get; }

        public int Start { get; }
        public int Length { get; }

        public char GetAffectedText()
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}