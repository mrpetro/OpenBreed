using OpenTK.Mathematics;

namespace OpenBreed.Gui.Abstractions.Elements
{
    public interface ITextPointer
    {
        int LineIndex { get; }
        int ColumnIndex { get; }
        int ValidColumnIndex { get; }

        bool Blink();

        void MoveForward();
        void MoveBack();
        void MoveLineBack();
        void MoveLineForward();
        void MoveToLineBegin();
        void MoveToLineEnd();
        void SetIndexPosition(Vector2 cursorPos);
        void NewLine();
    }
}