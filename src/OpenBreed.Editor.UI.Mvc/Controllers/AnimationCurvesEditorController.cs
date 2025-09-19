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

            view.CursorDown += View_CursorDown; ;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Reset()
        {
            view.Reset();
        }

        #endregion Public Methods

        #region Private Methods

        private void View_CursorDown(ViewCursorDownEvent e)
        {
            if (e.Key == CursorKey.Left)
            {
                //var cursorPos = GetCellIndexCoords(e.View, e.Position) + new Vector4i(model.CenterX, model.CenterY, 0, 1);

                //model.PutTiles(cursorPos, CurrentTileAtlasId, CurrentTileSelection);
            }
            else if (e.Key == CursorKey.Right)
            {
                view.AutoCenter();

                //var cursorPos = GetCellIndexCoords(e.View, e.Position) + new Vector4i(model.CenterX, model.CenterY, 0, 1);

                //model.EraseTile(cursorPos);
            }
        }

        #endregion Private Methods
    }
}