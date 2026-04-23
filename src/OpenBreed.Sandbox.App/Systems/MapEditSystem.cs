using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Input.Abstractions;
using OpenBreed.Input.Abstractions.Events;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.OpenGL;
using OpenBreed.Sandbox.App.Components;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace OpenBreed.Sandbox.App.Systems
{
    [RequireEntityWith(typeof(MapComponent))]
    public class MapEditSystem : IEventSystem
    {
        #region Private Fields

        private const int CellSize = 16;
        private readonly IInputsMan inputsMan;

        #endregion Private Fields

        #region Public Constructors

        public MapEditSystem(IInputsMan inputsMan)
        {
            this.inputsMan = inputsMan ?? throw new ArgumentNullException(nameof(inputsMan));
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IEnumerable<IEntity> entities, IWorldRenderContext context)
        {
            //var cursorDownIds = 0;// context.View.GetCursorDownIds();

            //var addTile = inputsMan;

            ////if (cursorDownIds.Any())
            ////{
            ////    addObstacle = true;
            ////}

            //var cursorPos4 = context.View.FromHostToWorldPoint((Vector2i)inputsMan.CursorPos);
            //var indexPos = new Vector2i((int)(cursorPos4.X / CellSize), (int)(cursorPos4.Y / CellSize));

            //foreach (var entity in entities)
            //{
            //    var map = entity.Get<MapComponent>();

            //    if (addTile)
            //    {
            //        var data = map.Grid.Get(indexPos);

            //        if (data is null)
            //        {
            //            data = new CellData();
            //            map.Grid.Set(indexPos, data);
            //        }

            //        data.GfxId = 0;
            //    }
            //}
        }

        public void Update(IEnumerable<IEntity> entities, Wecs.Abstractions.Primitives.IUpdateContext context)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}