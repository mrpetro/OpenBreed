using System;

namespace OpenBreed.Core.Events
{
    public class TextCaretPositionChanged : EventArgs
    {
        #region Public Constructors

        public TextCaretPositionChanged(int position)
        {
            Position = position;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Position { get; }

        #endregion Public Properties
    }
}