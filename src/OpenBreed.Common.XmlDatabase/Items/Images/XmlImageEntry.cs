using OpenBreed.Common.XmlDatabase.Items.Assets;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;

namespace OpenBreed.Common.XmlDatabase.Items.Images
{
    [Serializable]
    [Description("Image"), Category("Appearance")]
    public class XmlImageEntry : XmlDbEntry, IImageEntry
    {
        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        public override IEntry Copy()
        {
            return new XmlImageEntry() { Id = this.Id, Description = this.Description, DataRef = this.DataRef };
        }

        #endregion Public Properties
    }
}
