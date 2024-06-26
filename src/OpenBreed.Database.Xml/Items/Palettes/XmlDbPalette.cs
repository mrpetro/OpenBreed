using OpenBreed.Database.Interface.Items.Palettes;
using System;

namespace OpenBreed.Database.Xml.Items.Palettes
{
    [Serializable]
    public abstract class XmlDbPalette : XmlDbEntry, IDbPalette
    {
        #region Public Constructors

        protected XmlDbPalette()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbPalette(XmlDbPalette other) : base(other)
        {
        }

        #endregion Protected Constructors
    }
}