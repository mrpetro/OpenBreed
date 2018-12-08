using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.Database.Items.Sources
{
    [Serializable]
    public class SourceDef : DatabaseItemDef
    {
        #region Public Fields

        [XmlArrayItem(ElementName = "Parameter")]
        public readonly List<SourceParameterDef> Parameters = new List<SourceParameterDef>();

        #endregion Public Fields

        #region Public Properties

        [XmlAttribute]
        public string Type { get; set; }

        #endregion Public Properties

    }
}
