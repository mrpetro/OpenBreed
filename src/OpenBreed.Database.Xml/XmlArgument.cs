using OpenBreed.Database.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml
{
    public class XmlArgument : IArgument
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
