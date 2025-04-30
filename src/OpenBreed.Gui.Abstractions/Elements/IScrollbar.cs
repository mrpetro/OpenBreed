using OpenBreed.Gui.Abstractions.Constants;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Elements
{
    public interface IScrollbar : IElement
    {
        #region Public Properties

        ScrollbarMode Mode { get; }
        bool IsHandleHovered { get; }
        bool IsHandleGrabbed { get; }

        Box2 HandleBox { get; }

        #endregion Public Properties

        #region Public Methods

        void EnterHandle();

        void LeaveHandle();

        void MoveHandle(Vector2 offset);

        void GrabHandle(Vector2 startPosition);

        void ReleaseHandle();

        #endregion Public Methods
    }
}