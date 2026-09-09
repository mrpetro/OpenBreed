using OpenBreed.Common.Tools.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
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

            RegisterComponents(callingAssembly);
            RegisterXmlComponentTemplates(callingAssembly);
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
                if (XmlNodeMan.Instance.DeserializeXml<XmlComponentTemplate>(reader, out var outputDef))
                {
                    this.Add(outputDef);
                    continue;
                }

                var nonTemplateComponent = new XmlComponentTemplate(reader.Name);
                this.Add(nonTemplateComponent);

                reader.Skip();
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

        #region Private Methods

        private static void RegisterComponents(Assembly assembly)
        {
            var componentType = typeof(IEntityComponent);

            foreach (var type in assembly
                .DefinedTypes
                .Where(type => componentType.IsAssignableFrom(type)))
            {
                XmlComponentTemplate.RegisterComponentType(type);
            }
        }

        private static void RegisterXmlComponentTemplates(Assembly assembly)
        {
            foreach (var type in assembly
                .DefinedTypes
                .Where(type => type.BaseType == typeof(XmlComponentTemplate)))
            {
                XmlNodeMan.Instance.RegisterNodeType(type);
            }
        }

        #endregion Private Methods
    }
}