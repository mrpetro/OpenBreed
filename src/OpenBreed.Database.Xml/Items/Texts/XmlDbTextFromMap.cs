using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Texts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Texts
{
    [Serializable]
    [Description("Text from MAP"), Category("Appearance")]
    public class XmlDbTextFromMap : XmlDbText, IDbTextFromFile
    {
        [XmlElement("BlockName")]
        public string BlockName { get; set; }

        public override IDbEntry Copy()
        {
            return new XmlDbTextFromMap()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef,
                BlockName = this.BlockName
            };
        }
    }
}
