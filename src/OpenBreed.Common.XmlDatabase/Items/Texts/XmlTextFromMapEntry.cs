using OpenBreed.Common.Palettes;
using OpenBreed.Common.Texts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Texts
{
    [Serializable]
    [Description("Text from MAP"), Category("Appearance")]
    public class XmlTextFromMapEntry : XmlTextEntry, ITextFromMapEntry
    {
        [XmlElement("BlockName")]
        public string BlockName { get; set; }

        public override IEntry Copy()
        {
            return new XmlTextFromMapEntry()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef,
                BlockName = this.BlockName
            };
        }
    }
}
