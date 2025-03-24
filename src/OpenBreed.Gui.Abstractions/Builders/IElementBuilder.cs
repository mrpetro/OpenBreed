using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Rendering.Interface.Events;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenBreed.Gui.Abstractions.Builders
{
    public enum PositionSystem
    {
        World,
        Parent,
        ParentNormalized
    }

    public interface IElementBuilder<TElement> : IElementBuilder where TElement : IElement
    {
        #region Public Methods

        TElement Build();

        #endregion Public Methods
    }

    public interface IElementBuilder
    {
        #region Public Methods

        void SetClickCallback(Action<IElement, IInteractionCursor, CursorKey> callback);

        void SetEnterCallback(Action<IElement> callback);

        void SetLeaveCallback(Action<IElement> callback);

        void SetMoveCallback(Action<IElement, Vector2> callback);

        void SetDownCallback(Action<IElement> callback);

        void SetUpCallback(Action<IElement> callback);

        void SetPosition(float x, float y, PositionSystem positionSystem = PositionSystem.Parent);

        void SetPadding(float padding);

        void SetPadding(float left, float bottom, float right, float top);

        void SetMargin(float margin);

        void SetMargin(float left, float bottom, float right, float top);

        void SetSize(float width, float height);

        void SetMinimumSize(float width, float height);

        void SetMaximumSize(float width, float height);

        void SetTag(string tag);

        void SetOption(IElementOption parentOption);

        void SetHitTestable(bool flag);

        void SetMovable(bool flag);

        #endregion Public Methods
    }
}