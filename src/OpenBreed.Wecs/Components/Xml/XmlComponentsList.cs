using OpenBreed.Common.Tools.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Xml
{
    public class XmlComponentsList : List<XmlComponentTemplate>, IXmlSerializable
    {
        #region Public Constructors

        public XmlComponentsList() : base()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public static void RegisterAllAssemblyComponentTypes()
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            foreach (var type in callingAssembly
                .DefinedTypes
                .Where(type => type.BaseType == typeof(XmlComponentTemplate)))
            {
                XmlNodeMan.Instance.RegisterNodeType(type);
            }
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement)
                return;
            var listNodeName = reader.Name;

            reader.ReadStartElement(listNodeName);
            while (reader.Name != listNodeName)
            {
                var outputDef = XmlNodeMan.Instance.DeserializeXml<XmlComponentTemplate>(reader);
                if (outputDef != null)
                    this.Add(outputDef);
            }
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var outputDef in this)
            {
                XmlNodeMan.Instance.SerializeXml<XmlComponentTemplate>(writer, outputDef);
            }
        }

        #endregion Public Methods
    }
}