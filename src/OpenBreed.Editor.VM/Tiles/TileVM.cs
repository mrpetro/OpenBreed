using System;
using System.Drawing;
using OpenBreed.Common.Tiles;

namespace OpenBreed.Editor.VM.Tiles
{
    public class TileVM
    {

        #region Public Constructors

        public TileVM(TileSetVM owner, int index, Rectangle rectangle)
        {
            Owner = owner;
            Index = index;
            Rectangle = rectangle;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Index { get; private set; }
        public TileSetVM Owner { get; private set; }
        public Rectangle Rectangle { get; private set; }

        internal static TileVM Create(TileModel tile)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties

    }
}