using OpenBreed.Gui.Abstractions.Constants;
using OpenBreed.Gui.Abstractions.Presentations;
using OpenBreed.Rendering.Interface;
using OpenTK.Mathematics;

namespace OpenBreed.Gui.Abstractions.Elements
{
    public interface ITextBox : IElement
    {
        #region Public Properties

        ITextPointer Pointer { get; }

        IFont Font { get; }

        int LinesCount { get; }

        #endregion Public Properties

        #region Public Methods

        float GetPointerXPosition();

        int GetLineLength(int lineIndex);

        char GetCharacter(int columnIndex, int lineIndex);

        void Insert(string text);

        int GetCharacterIndex(int columnIndex, int lineIndex);

        Vector2i GetIndexPosition(Vector2 position);

        IEnumerable<char> GetLineCharacters(int lineIndex);

        #endregion Public Methods
    }
}