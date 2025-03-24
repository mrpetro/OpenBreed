using OpenBreed.Gui.Abstractions.Elements;
using OpenTK.Mathematics;

namespace OpenBreed.Gui.Elements
{
    internal class TextPointer : ITextPointer
    {
        #region Private Fields

        private const int BlickFrames = 15;

        private readonly ITextBox owner;

        private int blinkTimer;

        #endregion Private Fields

        #region Public Constructors

        public TextPointer(ITextBox owner)
        {
            this.owner = owner;
        }

        #endregion Public Constructors

        #region Public Properties

        public int LineIndex { get; private set; }

        public int ColumnIndex { get; private set; }

        public int ValidColumnIndex { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public bool Blink()
        {
            blinkTimer++;

            if (blinkTimer > BlickFrames)
            {
                blinkTimer = -BlickFrames;
            }

            return blinkTimer > 0;
        }

        public void MoveForward()
        {
            var lineLength = owner.GetLineLength(LineIndex);

            if (ColumnIndex < lineLength)
            {
                ColumnIndex++;
            }

            while (ColumnIndex < lineLength)
            {
                var ch = owner.GetCharacter(ColumnIndex - 1, LineIndex);

                if (IsSkipped(ch))
                {
                    ColumnIndex++;
                    continue;
                }

                break;
            }

            if (ColumnIndex >= lineLength)
            {
                if (LineIndex < owner.LinesCount - 1)
                {
                    ColumnIndex = 0;
                    LineIndex++;
                }
            }
        }

        public void MoveBack()
        {
            if (ColumnIndex == 0)
            {
                if (LineIndex > 0)
                {
                    LineIndex--;
                    ColumnIndex = owner.GetLineLength(LineIndex);
                }
            }
            else
            {
                ColumnIndex--;
            }

            SkipPreviousNewLineCharacters();
        }

        public void MoveLineBack()
        {
            if (LineIndex > 0)
            {
                LineIndex--;

                var lineLength = owner.GetLineLength(LineIndex);

                if (ColumnIndex > lineLength)
                {
                    ColumnIndex = lineLength;
                }
            }
        }

        public void MoveLineForward()
        {
            if (LineIndex < owner.LinesCount - 1)
            {
                LineIndex++;

                var lineLength = owner.GetLineLength(LineIndex);

                if (ColumnIndex > lineLength)
                {
                    ColumnIndex = lineLength;
                }
            }
        }

        public void MoveToLineBegin()
        {
            ColumnIndex = 0;
        }

        public void MoveToLineEnd()
        {
            ColumnIndex = owner.GetLineLength(LineIndex);
            SkipPreviousNewLineCharacters();
        }

        public void NewLine()
        {
            owner.Insert(Environment.NewLine);
            MoveForward();
        }

        public void SetIndexPosition(Vector2 cursorPos)
        {
            var indexPosition = owner.GetIndexPosition(cursorPos);

            ColumnIndex = indexPosition.X;
            LineIndex = indexPosition.Y;

            SkipPreviousNewLineCharacters();
        }

        #endregion Public Methods

        #region Private Methods

        private void SkipPreviousNewLineCharacters()
        {
            while (ColumnIndex > 0)
            {
                var ch = owner.GetCharacter(ColumnIndex - 1, LineIndex);

                if (IsSkipped(ch))
                {
                    ColumnIndex--;
                    continue;
                }

                return;
            }
        }

        private bool IsSkipped(char ch)
        {
            if (ch == '\r')
            {
                return true;
            }

            if (ch == '\n')
            {
                return true;
            }

            return false;
        }

        #endregion Private Methods
    }
}