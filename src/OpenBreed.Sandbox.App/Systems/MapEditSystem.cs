using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Input.Abstractions;
using OpenBreed.Input.Abstractions.Events;
using OpenBreed.Pathfinding.Abstractions.Services;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Factories;
using OpenBreed.Rendering.OpenGL;
using OpenBreed.Sandbox.App.Components;
using OpenBreed.Sandbox.App.Constants;
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
    public class MapEditSystem : IEventSystem<ViewCursorDownEvent>, IEventSystem<ViewCursorMoveEvent>
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

        public void OnEvent(ViewCursorDownEvent e, IWorld world)
        {
            var view = e.View;
            var cursorPos4 = e.Position;

            var indexPos = new Vector2i((int)(cursorPos4.X / CellSize), (int)(cursorPos4.Y / CellSize));

            foreach (var entity in world.Entities)
            {
                var map = entity.TryGet<MapComponent>();

                if (map is null)
                {
                    continue;
                }

                if (!map.Grid.TryGet(indexPos, out CellData data))
                {
                    continue;
                }

                if (data is null)
                {
                    data = new CellData();
                    map.Grid.Set(indexPos, data);
                }
                if (e.Key == CursorKey.Middle)
                {
                    if (e.Modifiers.HasFlag(KeyModifiers.Control))
                    {
                        data.GfxId = Tiles.Water;
                    }
                    else
                    {
                        data.GfxId = Tiles.Wall;
                    }
                }
                else if (e.Key == CursorKey.Right)
                {
                    data.GfxId = Tiles.Empty;
                }
            }
        }

        public void OnEvent(ViewCursorMoveEvent e, IWorld world)
        {
            var view = e.View;
            var cursorPos4 = e.Position;
            var indexPos = new Vector2i((int)(cursorPos4.X / CellSize), (int)(cursorPos4.Y / CellSize));
            var mapEntity = world.Entities.First(item => item.Tag == "Map");

            var map = mapEntity.TryGet<MapComponent>();

            if (map is null)
            {
                return;
            }

            if (!map.Grid.TryGet(indexPos, out CellData data))
            {
                return;
            }

            if (data is null)
            {
                data = new CellData();
                map.Grid.Set(indexPos, data);
            }

            if (e.IsCursorKeyPressed(CursorKey.Middle))
            {
                if (e.Modifiers.HasFlag(KeyModifiers.Control))
                {
                    data.GfxId = Tiles.Water;
                }
                else
                {
                    data.GfxId = Tiles.Wall;
                }
            }
            else if (e.IsCursorKeyPressed(CursorKey.Right))
            {
                data.GfxId = Tiles.Empty;
            }
        }

        #endregion Public Methods
    }
}