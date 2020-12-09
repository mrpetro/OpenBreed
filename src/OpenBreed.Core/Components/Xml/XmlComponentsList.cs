using AVEVA.NET.Gateways.Components.Common;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Core.Components.Xml
{
    public class XmlComponentsList : List<XmlComponentTemplate>, IXmlSerializable
    {
        #region Public Constructors

        public XmlComponentsList() : base()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public static void RegisterComponentType<T>()
        {
            XmlNodeMan.Instance.RegisterNodeType(typeof(T));
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