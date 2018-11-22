using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using OpenBreed.Common.Logging;

namespace OpenBreed.Common.Props.Readers.XML
{
    [Serializable]
    public class PropertySetDef
    {
        private const string DEFAULT_PATH = @"Resources\DefaultPropertySetDef.xml";

        [XmlAttribute]
        public string Name { get; set; }



        public List<PropertyDef> PropertyDefs { get; set; }

        public static PropertySetDef LoadDefault()
        {
            try
            {
                string defaultPath = Path.Combine(ProgramTools.AppDir, DEFAULT_PATH);
                PropertySetDef propertySetDef = Tools.RestoreFromXml<PropertySetDef>(defaultPath);
                LogMan.Instance.LogSuccess("Default Database loaded successful from " + defaultPath);
                return propertySetDef;
            }
            catch (Exception ex)
            {
                LogMan.Instance.LogError("Unable to load default PropertySet. Reason: " + ex.Message);
            }

            return null;
        }
    }
}
