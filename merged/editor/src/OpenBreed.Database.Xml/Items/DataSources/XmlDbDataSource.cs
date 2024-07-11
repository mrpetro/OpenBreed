using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Xml.Items.Palettes;
using System;

namespace OpenBreed.Database.Xml.Items.DataSources
{
    [Serializable]
    public abstract class XmlDbDataSource : XmlDbEntry, IDbDataSource
    {
        #region Protected Constructors

        protected XmlDbDataSource()
        {
        }

        protected XmlDbDataSource(XmlDbDataSource other) : base(other)
        {
        }

        #endregion Protected Constructors
    }
}