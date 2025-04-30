using OpenTK.Mathematics;
using System.Reflection;

namespace OpenBreed.Gui.Abstractions.Elements
{
    public interface ITextPointer
    {
        int LineIndex { get; }
        int ColumnIndex { get; }
        int ValidColumnIndex { get; }

        float GetXPosition();

        bool Blink();

        void MoveForward();
        void MoveBack();
        void MoveLineBack();
        void MoveLineForward();
        void MoveToLineBegin();
        void MoveToLineEnd();
        void SetIndexPosition(Vector2 cursorPos);
        void Reset();
    }
}