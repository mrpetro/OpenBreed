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
    [Description("Text from file"), Category("Appearance")]
    public class XmlTextFromFileEntry : XmlTextEntry, ITextFromFileEntry
    {
        public override IEntry Copy()
        {
            return new XmlTextFromFileEntry()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef,
            };
        }
    }
}
