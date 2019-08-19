using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace OpenBreed.Core.Blueprints
{
    public class ComponentStateXml : IComponentState, IXmlSerializable
    {
        private static Dictionary<Type, Func<XmlReader, object>> parsers = new Dictionary<Type, Func<XmlReader, object>>();

        public static void RegisterTypeParser(Type type, Func<XmlReader, object> parser)
        {
            parsers.Add(type, parser);
        }

        #region Public Properties

        [XmlAttribute]
        public string Key { get; private set; }

        public object Value { get; private set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            string nodeName = reader.Name;

            Key = reader.GetAttribute("Key");

            reader.ReadStartElement(nodeName);

            try
            {
                if (reader.Name != "Value")
                    throw new FormatException();

                string valueTypeString = reader.GetAttribute("Type");

                var valueType = Type.GetType(valueTypeString);

                if (valueType.IsPrimitive)
                {
                    var tc = TypeDescriptor.GetConverter(valueType);

                    reader.ReadStartElement();
                    var value = reader.ReadContentAsString();
                    Value = tc.ConvertFromInvariantString(value);
                    reader.ReadEndElement();
                    return;
                }
                else
                {
                    Func<XmlReader, object> parser;
                    if (!parsers.TryGetValue(valueType, out parser))
                    {

                    }

                    Value = parser.Invoke(reader);
                }
            }
            finally
            {
                reader.ReadEndElement();
            }


        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{Key} = {Value}";
        }

        #endregion Public Properties
    }
}
