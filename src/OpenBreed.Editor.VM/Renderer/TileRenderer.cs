//using OpenBreed.Editor.VM.Tiles;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OpenBreed.Editor.VM.Renderer
//{
//    public class TileRenderer : RendererBase<TileVM>
//    {
//        #region Private Fields


//        #endregion Private Fields

//        #region Public Constructors

//        public TileRenderer(Graphics gfx) : base(gfx)
//        {
//        }

//        #endregion Public Constructors

//        #region Public Methods

//        public override void Render(TileVM renderable)
//        {
//            if (tileRef.TileSetId < Parent.Root.LevelEditor.CurrentLevel.TileSets.Count)
//                Parent.Root.LevelEditor.CurrentLevel.TileSets[tileRef.TileSetId].DrawTile(gfx, tileRef.TileId, x, y, tileSize);
//            else
//                DrawDefaultTile(gfx, tileRef, x, y, tileSize);
//        }

//        #endregion Public Methods
//    }
//}
