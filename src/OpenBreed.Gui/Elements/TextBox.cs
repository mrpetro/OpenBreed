using OpenBreed.Common.Tools;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Extensions;
using OpenBreed.Gui.Builders;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Events;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace OpenBreed.Gui.Elements
{

    internal class TextBox : Element, ITextBox
    {
        #region Private Fields

        private readonly List<int> lines = new List<int>([0]);
        private readonly List<char> characters = new List<char>();

        #endregion Private Fields

        #region Internal Constructors

        internal TextBox(TextBoxBuilder builder) : base(builder)
        {
            Font = builder.GetFont();
            Pointer = new TextPointer(this);

            InsertAt(builder.Text, Pointer.ColumnIndex, Pointer.LineIndex);
        }

        #endregion Internal Constructors

        #region Public Properties

        public Vector2 ScrollPosition { get; private set; }

        public ITextPointer Pointer { get; }

        public IFont Font { get; }

        public int LinesCount => lines.Count;

        #endregion Public Properties

        #region Public Methods

        public char GetCharacter(int columnIndex, int lineIndex)
        {
            var characterIndex = GetCharacterIndex(columnIndex, lineIndex);
            return characters[characterIndex];
        }

        public IEnumerable<char> GetCharacters(int lineIndex)
        {
            var startIndex = lines[lineIndex];
            var endIndex = lineIndex == lines.Count - 1 ? characters.Count : lines[lineIndex + 1];

            for (int i = startIndex; i < endIndex; i++)
            {
                yield return characters[i];
            }
        }

        public IReadOnlyList<float> GetCharacterPositions(int lineIndex)
        {
            var xPositions = new List<float>();

            var lineCharacters = GetCharacters(lineIndex).ToArray();
            var textPLength = 0.0f;

            for (int i = 0; i < lineCharacters.Length; i++)
            {
                var ch = lineCharacters[i];
                var chWidth = Font.GetWidth(ch);

                xPositions.Insert(i, textPLength);

                textPLength += chWidth;
            }

            xPositions.Insert(lineCharacters.Length, textPLength);

            return xPositions;
        }

        public int GetCharacterIndex(int columnIndex, int lineIndex)
        {
            var startIndex = lines[lineIndex];
            return startIndex + columnIndex;
        }

        public int GetLineLength(int lineIndex)
        {
            var startIndex = lines[lineIndex];
            var endIndex = lineIndex == lines.Count - 1 ? characters.Count : lines[lineIndex + 1];

            return endIndex - startIndex;
        }

        public void ScanLines(Action<int, IEnumerable<char>> action)
        {
            for (int lineIndex = 0; lineIndex < lines.Count; lineIndex++)
            {
                var lineCharacters = GetCharacters(lineIndex);

                action.Invoke(lineIndex, lineCharacters);
            }
        }

        public Vector2i GetIndexPosition(Vector2 position)
        {
            var startPos = this.StartPos();

            position -= startPos;

            var lineIndex = (int)((Font.Height - position.Y) / Font.Height);

            if (lineIndex >= this.lines.Count)
            {
                lineIndex = this.lines.Count - 1;
            }

            var lineCharacters = GetCharacters(lineIndex).ToArray();
            var lineXPositions = GetCharacterPositions(lineIndex);

            for (int i = 0; i < lineCharacters.Length; i++)
            {
                var chWidth = Font.GetWidth(lineCharacters[i]);

                if (position.X < lineXPositions[i] + chWidth / 2.0f)
                {
                    return new Vector2i(i, lineIndex);
                }
            }

            return new Vector2i(lineXPositions.Count - 1, lineIndex);
        }

        public void Input(string text)
        {
            InsertAt(text, Pointer.ColumnIndex, Pointer.LineIndex);
            Pointer.MoveForward();
        }

        public void Remove()
        {
            var characterIndex = GetCharacterIndex(Pointer.ColumnIndex, Pointer.LineIndex) - 1;

            if (characterIndex < 0)
            {
                return;
            }

            var ch = characters[characterIndex];

            var charactersCount = 1;
            var columnIndex = Pointer.ColumnIndex;
            var lineIndex = Pointer.LineIndex;

            if (ch == '\n')
            {
                if (characterIndex > 0)
                {
                    ch = characters[characterIndex - 1];

                    if (ch == '\r')
                    {
                        characterIndex--;
                        charactersCount++;
                    }
                }
            }

            Pointer.MoveBack();
            RemoveAt(charactersCount, columnIndex, lineIndex);
        }

        #endregion Public Methods

        #region Private Methods

        private void InsertAt(string text, int columnIndex, int lineIndex)
        {
            var lineCharacters = new List<char>();

            var characterIndex = GetCharacterIndex(columnIndex, lineIndex);

            var newLines = Enumerable.Range(0, text.Length).Where(i => text[i] == '\n').Select(i => i + characterIndex + 1).ToList();
            characters.InsertRange(characterIndex, text);
            lines.InsertRange(lineIndex + 1, newLines);

            var endLineIndex = lineIndex + 1 + newLines.Count;

            for (int i = endLineIndex; i < lines.Count; i++)
            {
                lines[i] += text.Length;
            }
        }

        private void RemoveAt(int charactersCount, int columnIndex, int lineIndex)
        {
            var characterIndex = GetCharacterIndex(columnIndex, lineIndex) - charactersCount;

            if (characterIndex < 0)
            {
                return;
            }

            var linesCount = characters.Skip(characterIndex).Take(charactersCount).Count(ch => ch == '\n');

            characters.RemoveRange(characterIndex, charactersCount);

            lines.RemoveRange(lineIndex, linesCount);

            if (linesCount == 0)
            {
                lineIndex++;
            }

            for (int i = lineIndex; i < lines.Count; i++)
            {
                lines[i] -= charactersCount;
            }
        }

        #endregion Private Methods
    }
}