using OpenBreed.Database.Xml.Items.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Images;

namespace OpenBreed.Database.Xml.Items.Images
{
    [Serializable]
    [Description("Image"), Category("Appearance")]
    public class XmlDbImage : XmlDbEntry, IDbImage
    {
        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        public override IDbEntry Copy()
        {
            return new XmlDbImage()
            { 
                Id = this.Id, 
                Description = this.Description, 
                DataRef = this.DataRef 
            };
        }

        #endregion Public Properties
    }
}
