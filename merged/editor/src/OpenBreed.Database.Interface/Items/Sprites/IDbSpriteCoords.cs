using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Sprites
{
    public interface IDbSpriteCoords
    {
        #region Public Properties

        int X { get; set; }
        int Y { get; set; }
        int Width { get; set; }
        int Height { get; set; }

        #endregion Public Properties

        #region Public Methods

        IDbSpriteCoords Copy();

        #endregion Public Methods
    }
}