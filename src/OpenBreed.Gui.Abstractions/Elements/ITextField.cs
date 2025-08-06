using OpenBreed.Gui.Abstractions.Constants;
using OpenBreed.Gui.Abstractions.Presentations;
using OpenBreed.Rendering.Abstractions;
using OpenTK.Mathematics;

namespace OpenBreed.Gui.Abstractions.Elements
{
    public interface ITextField : IElement
    {
        #region Public Properties

        ITextPointer Pointer { get; }

        IFontAtlas Font { get; }

        int LinesCount { get; }

        Vector2 ScrollPosition { get; set; }

        #endregion Public Properties

        #region Public Methods

        int GetLineLength(int lineIndex);

        char GetCharacter(int columnIndex, int lineIndex);

        /// <summary>
        /// Remove character from current text field pointer position and advance pointer back.
        /// </summary>
        void Remove();

        /// <summary>
        /// Input text at current text field pointer position.
        /// </summary>
        /// <param name="text"></param>
        void Input(string text);

        int GetCharacterIndex(int columnIndex, int lineIndex);

        IReadOnlyList<float> GetCharacterPositions(int lineIndex);

        Vector2i GetIndexPosition(Vector2 position);

        IEnumerable<char> GetCharacters(int lineIndex);

        #endregion Public Methods
    }
}