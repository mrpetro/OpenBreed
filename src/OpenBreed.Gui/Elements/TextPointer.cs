using OpenBreed.Common.Tools;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Extensions;
using OpenTK.Mathematics;
using System.Reflection;

namespace OpenBreed.Gui.Elements
{
    internal class TextPointer : ITextPointer
    {
        #region Private Fields

        private const int BlickFrames = 15;

        private readonly ITextField owner;

        private int blinkTimer;

        #endregion Private Fields

        #region Public Constructors

        public TextPointer(ITextField owner)
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

        public float GetXPosition()
        {
            var xPositions = owner.GetCharacterPositions(LineIndex);

            if (xPositions.Count == 0)
            {
                return 0.0f;
            }

            if (ColumnIndex == -1)
            {
                return xPositions.Last();
            }

            return xPositions[ColumnIndex];
        }

        public float GetYPosition()
        {
            return LineIndex * owner.Font.Height;
        }

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

            AdjustScroll();
        }

        public Vector2 GetPosition()
        {
            return new Vector2(GetXPosition(), GetYPosition());
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

            AdjustScroll();
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

            AdjustScroll();
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

            AdjustScroll();
        }

        public void MoveToLineBegin()
        {
            ColumnIndex = 0;

            AdjustScroll();
        }

        public void MoveToLineEnd()
        {
            ColumnIndex = owner.GetLineLength(LineIndex);
            SkipPreviousNewLineCharacters();

            AdjustScroll();
        }

        public void SetIndexPosition(Vector2 cursorPos)
        {
            var pos = -owner.ScrollPosition + cursorPos;
            var indexPosition = owner.GetIndexPosition(pos);

            ColumnIndex = indexPosition.X;
            LineIndex = indexPosition.Y;

            SkipPreviousNewLineCharacters();

            AdjustScroll();
        }

        #endregion Public Methods

        #region Private Methods

        private void AdjustScroll()
        {
            var scrollBox = owner.GetScrollBox();

            var position = GetPosition();

            var xOffset = 0.0f;
            var yOffset = 0.0f;

            if (position.X < scrollBox.Min.X)
            {
                xOffset = scrollBox.Min.X - position.X + 3;
            }
            if (position.X > scrollBox.Max.X)
            {
                xOffset = scrollBox.Max.X - position.X - 3;
            }

            if (position.Y < scrollBox.Min.Y + owner.Font.Height)
            {
                yOffset = -(scrollBox.Min.Y +owner.Font.Height) + position.Y;
            }
            if (position.Y > scrollBox.Max.Y)
            {
                yOffset = -scrollBox.Max.Y + position.Y;
            }

            owner.ScrollPosition += new Vector2(xOffset, yOffset);
        }

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