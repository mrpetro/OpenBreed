using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Interface.Elements;
using OpenTK.Mathematics;

namespace OpenBreed.Gui.Interface.Builders
{
    public enum PositionSystem
    {
        World,
        Parent,
        ParentNormalized
    }

    public interface IElementBuilder
    {
        #region Public Methods

        void SetClickCallback(Action<IElement> callback);

        void SetEnterCallback(Action<IElement> callback);

        void SetLeaveCallback(Action<IElement> callback);

        void SetMoveCallback(Action<IElement> callback);

        void SetDownCallback(Action<IElement> callback);

        void SetUpCallback(Action<IElement> callback);

        void SetPosition(float x, float y, PositionSystem positionSystem = PositionSystem.Parent);

        void SetPadding(float padding);

        void SetPadding(float left, float bottom, float right, float top);

        void SetMargin(float margin);

        void SetMargin(float left, float bottom, float right, float top);

        void SetSize(int width, int height);

        void SetTag(string tag);

        void SetOption(IElementOption parentOption);

        void SetHitTestable(bool flag);

        void SetMovable(bool flag);

        IElement Build();

        #endregion Public Methods
    }
}