using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Texts;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Texts
{
    [Serializable]
    [Description("Text from file"), Category("Appearance")]
    public class XmlDbTextFromFile : XmlDbText, IDbTextFromFile
    {
        #region Public Constructors

        public XmlDbTextFromFile()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbTextFromFile(XmlDbTextFromFile other) : base(other)
        {
            DataRef = other.DataRef;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbTextFromFile(this);

        #endregion Public Methods
    }
}