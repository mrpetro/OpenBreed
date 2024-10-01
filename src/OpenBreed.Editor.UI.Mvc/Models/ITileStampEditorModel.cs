using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Mvc.Models
{
    public interface ITileStampEditorModel : IEditorModel
    {
        #region Public Properties

        public int CenterX { get; set; }

        public int CenterY { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public IReadOnlyList<IDbTileStampCell> Cells { get; }

        #endregion Public Properties

        #region Public Methods

        void EraseTile(Vector4i erasePoint);

        void PutTiles(Vector4i insertPoint, string tileAtlasId, IReadOnlyList<TileSelection> toInsert);

        #endregion Public Methods
    }
}