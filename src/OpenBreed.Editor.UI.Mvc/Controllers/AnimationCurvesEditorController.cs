using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Rendering.Abstractions.Data;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Editor.UI.Mvc.Views;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Rendering.Abstractions.Extensions;
using System.Drawing;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.UI.Mvc.Extensions;
using OpenBreed.Common;
using OpenBreed.Core.Interface.Extensions;
using static System.Net.Mime.MediaTypeNames;
using System.Collections;
using static System.Formats.Asn1.AsnWriter;
using OpenBreed.Rendering.Abstractions.Renderers;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Pipes;
using OpenBreed.Animation.Interface;

namespace OpenBreed.Editor.UI.Mvc.Controllers
{
    public class AnimationCurvesEditorController : IController
    {
        #region Private Fields

        private readonly AnimationCurvesEditorView view;
        private readonly IAnimationEditorModel model;

        #endregion Private Fields

        #region Public Constructors

        public AnimationCurvesEditorController(
            IEventsMan eventsMan,
            AnimationCurvesEditorView view,
            IAnimationEditorModel model)
        {
            this.view = view;
            this.model = model;

            view.CursorDown += View_CursorDown;
            view.CursorUp += View_CursorUp;
            view.CursorMove += View_CursorMove;
            view.KeyDown += View_KeyDown;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Focus()
        {
            view.Reset();
            view.AutoCenter();
        }

        public void SetMode(AnimationCurvesEditorMode mode)
        {
            model.Mode = mode;
        }

        #endregion Public Methods

        #region Private Methods

        private void InsertKeyFrame()
        {
            var position = view.CursorInteractionSnapPosition;
            model.InsertKeyFrame(position.X, position.Y);
        }

        private void View_CursorDown(ViewCursorDownEvent e)
        {
            if (e.Key == CursorKey.Left)
            {
                switch (model.Mode)
                {
                    case AnimationCurvesEditorMode.InsertKeyFrames:
                        InsertKeyFrame();
                        break;

                    case AnimationCurvesEditorMode.SelectKeyFrames:

                        if (model.HoveredKeyFrame is not null)
                        {
                            model.StartSelecting(view.CursorInteractionPosition);
                        }

                        break;

                    case AnimationCurvesEditorMode.MoveKeyFrames:
                        break;

                    default:
                        break;
                }
            }
        }

        private void View_CursorUp(ViewCursorUpEvent e)
        {
            if (e.Key == CursorKey.Left)
            {
                switch (model.Mode)
                {
                    case AnimationCurvesEditorMode.SelectKeyFrames:

                        if (model.AnchorPoint != null)
                        {
                            model.FinishMoving();

                            if (model.HoveredKeyFrame is not null)
                            {
                                if (model.AnchorPoint.Value == view.CursorInteractionPosition)
                                {
                                    TrySelectKeyFrame(e.Modifiers.HasFlag(KeyModifiers.Control));
                                }
                            }
                            else
                            {
                                model.ClearSelectedKeyFrames();
                            }

                            model.FinishSelecting();
                        }
                        else
                        {
                            model.ClearSelectedKeyFrames();
                        }

                        break;

                    case AnimationCurvesEditorMode.MoveKeyFrames:
                        break;
                    default:
                        break;
                }
            }
        }

        private void View_CursorMove(ViewCursorMoveEvent e)
        {
            switch (model.Mode)
            {
                case AnimationCurvesEditorMode.InsertKeyFrames:
                    break;

                case AnimationCurvesEditorMode.SelectKeyFrames:

                    if (model.AnchorPoint != null)
                    {
                        if (model.SelectedKeyFrames.Contains(model.HoveredKeyFrame))
                        {
                            model.MoveKeyFramesTo(view.CursorInteractionSnapPosition);
                        }
                        else
                        {
                            model.CancelSelecting();
                        }
                    }
                    else
                    {
                        TryHoverOverKeyFrame();
                    }

                    break;

                case AnimationCurvesEditorMode.MoveKeyFrames:

                    break;

                default:
                    break;
            }
        }

        private void View_KeyDown(ViewKeyDownEvent e)
        {
            switch (e.Key)
            {
                case Keys.Escape:
                    model.ClearSelectedKeyFrames();
                    break;

                case Keys.Delete:
                    model.DeleteSelectedKeyFrames();
                    break;
            }
        }

        private void TrySelectKeyFrame(bool multiSelection)
        {
            if (!multiSelection)
            {
                model.ClearSelectedKeyFrames();
            }

            if (model.HoveredKeyFrame is not null)
            {
                model.SelectKeyFrame(model.HoveredKeyFrame);
                return;
            }
        }

        private void TryHoverOverKeyFrame()
        {
            var cursorPos = view.CursorInteractionSnapPosition;

            var keyFramePointSize = 15.0f;

            var tolerance = new Vector2(keyFramePointSize, keyFramePointSize) / view.RenderView.GetScale();

            if (model.TryGetClosestKeyFrame(cursorPos, tolerance, out ITrackKeyFrame? keyFrame))
            {
                model.HoveredKeyFrame = keyFrame;

                return;
            }

            model.HoveredKeyFrame = null;
        }

        #endregion Private Methods
    }
}