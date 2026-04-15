using OpenBreed.Common.Interface.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Abstractions.Builders
{
    public interface ITextureBuilder
    {
        #region Public Methods

        ITextureBuilder SetName(string name);

        ITextureBuilder SetBitmap(IBitmap bitmap);

        //ITextureBuilder SetBitmap(IBitmap bitmap);

        #endregion Public Methods
    }
}