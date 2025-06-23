using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Abstractions
{
    public interface IRenderContext
    {
        #region Public Properties

        ISpriteMan Sprites { get; }
        ISpriteRenderer SpriteRenderer { get; }
        IPrimitiveRenderer Primitives { get; }
        IFontMan Fonts { get; }
        ITextureMan Textures { get; }
        IPictureMan Pictures { get; }
        IPictureRenderer PictureRenderer { get; }
        ITileMan Tiles { get; }
        IStampMan TileStamps { get; }
        IPaletteMan Palettes { get; }

        IEnumerable<IRenderView> Views { get; }

        #endregion Public Properties

        #region Public Methods

        IRenderView CreateView(float minX = 0, float minY = 0, float maxX = 1, float maxY = 1);
        void RemoveView(IRenderView renderView);

        void Render(float dt);
        void Resize(int width, int height);
        void CursorLeave(int cursorId, Vector2i point);
        void CursorEnter(int cursorId, Vector2i point);
        void CursorUp(int cursorId, Vector2i point, CursorKey cursorKey);
        void CursorDown(int cursorId, Vector2i point, CursorKey cursorKey);
        void CursorMove(int cursorId, Vector2i point);
        void CursorWheel(int cursorId, Vector2i point, int wheelDelta);
        void TextInput(string text);
        void KeyDown(Keys key, KeyModifiers modifiers);
        void KeyUp(Keys key, KeyModifiers modifiers);

        void Initialize();
        void Deinitialize();

        #endregion Public Methods
    }
}