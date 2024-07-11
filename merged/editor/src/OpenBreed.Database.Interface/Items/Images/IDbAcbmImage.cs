using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Images
{
    public interface IDbAcbmImage : IDbImage
    {
        #region Public Properties

        string DataRef { get; set; }

        int Width { get; set; }
        int Height { get; set; }
        int BitPlanesNo { get; set; }
        string PaletteMode { get; set; }

        #endregion Public Properties
    }
}