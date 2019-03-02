using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Maps.Blocks
{
    public class MapBodyBlock : IMapDataBlock
    {

        #region Public Fields

        public const string NAME = "BODY";

        #endregion Public Fields

        #region Public Constructors

        public MapBodyBlock(int length)
        {
            Cells = new MapCell[length];
        }

        #endregion Public Constructors

        #region Public Properties

        public MapCell[] Cells { get; }

        public string Name { get { return NAME; } }

        #endregion Public Properties

        #region Public Structs

        public struct MapCell
        {

            #region Public Constructors

            public MapCell(int gfxId, int actionId)
            {
                GfxId = gfxId;
                ActionId = actionId;
            }

            #endregion Public Constructors

            #region Public Properties

            public int ActionId { get; set; }
            public int GfxId { get; set; }

            #endregion Public Properties
        }

        #endregion Public Structs

    }
}
