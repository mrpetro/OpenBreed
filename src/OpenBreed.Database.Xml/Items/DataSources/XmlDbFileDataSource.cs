using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.DataSources;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.DataSources
{
    [Serializable]
    [Description("File"), Category("Appearance")]
    public class XmlDbFileDataSource : XmlDbDataSource, IDbFileDataSource
    {
        #region Public Constructors

        public XmlDbFileDataSource()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbFileDataSource(XmlDbFileDataSource other) : base(other)
        {
            FilePath = other.FilePath;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlAttribute]
        public string FilePath { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbFileDataSource();

        #endregion Public Methods
    }
}