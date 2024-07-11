using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Common.Tools.Xml
{
    /// <summary>
    /// This is singleton class of Xml Node Manager which helps in (de)serialization of Xml nodes which object types not known at compilation time 
    /// </summary>
    public class XmlNodeMan
    {
        #region Private Fields

        private static readonly Lazy<XmlNodeMan> _lazy = new Lazy<XmlNodeMan>(() => new XmlNodeMan());
        private Dictionary<Type, string> _nodeDictionary;
        private Dictionary<string, Type> _typeDictionary;

        #endregion

        #region Private Constructors

        /// <summary>
        /// Private constructor for XmlNodeMan
        /// </summary>
        private XmlNodeMan()
        {
            _nodeDictionary = new Dictionary<Type, string>();
            _typeDictionary = new Dictionary<string, Type>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Lazy Singleton instance for XmlNodeMan
        /// </summary>
        public static XmlNodeMan Instance { get { return _lazy.Value; } }

        /// <summary>
        /// Enabling this property will case XmlNodeMan not to throw exception when reading unrecognised node types, but skip them
        /// </summary>
        public bool IgnoreUnknownNodes { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// This will try deserialize currently read node in the reader into object of type T
        /// If corrently deserialized node name is unregistered then deserialization will skip it
        /// or thow InvalidOperationException
        /// </summary>
        /// <typeparam name="T">Type of object to be deserialized</typeparam>
        /// <param name="reader">XmlReader that currently reads the XML</param>
        /// <returns>Deserialized object of type T</returns>
        public T DeserializeXml<T>(XmlReader reader)
        {
            try
            {
                var nodeName = reader.Name;
                var nodeType = Instance.GetNodeType(nodeName);
                if (nodeType == null)
                {
                    if (IgnoreUnknownNodes || reader.NodeType == XmlNodeType.Comment)
                    {
                        reader.Skip();
                        return default(T);
                    }
                    else
                        throw new InvalidOperationException($"Unknown node type '{nodeName}'");
                }

                var xmlSerializer = new XmlSerializer(nodeType);
                var deserialized = (T)xmlSerializer.Deserialize(reader);
                return deserialized;
            }
            catch (Exception ex)
            {
                throw new Exception($"'{typeof(T)}' deserialization", ex);
            }
        }

        /// <summary>
        /// This will unregister all node types that are currently registered in this instance
        /// </summary>
        public void UnregisterAllNodeTypes()
        {
            _typeDictionary.Clear();
            _nodeDictionary.Clear();
        }

        /// <summary>
        /// This will bind object type with xml node name which supose to be defined in XmlRootAttribute of typed class/structue
        /// </summary>
        /// <param name="nodeType"></param>
        public void RegisterNodeType(Type nodeType)
        {
            var xmlRootAttribute = nodeType.GetCustomAttributes(typeof(XmlRootAttribute), true).FirstOrDefault() as XmlRootAttribute;

            if (xmlRootAttribute == null)
                throw new Exception($"XmlRootAttribute is missing on type '{nodeType}'");

            var nodeName = xmlRootAttribute.ElementName;

            _typeDictionary.Add(nodeName, nodeType);
            _nodeDictionary.Add(nodeType, nodeName);
        }

        /// <summary>
        /// This will serialize object of type T using given XmlWriter
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="writer">Xml writer for serialization</param>
        /// <param name="element">Element of type T to serialize</param>
        public void SerializeXml<T>(XmlWriter writer, T element)
        {
            if (element == null)
                return;

            var type = element.GetType();

            var xmlSerializer = new XmlSerializer(type);
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            xmlSerializer.Serialize(writer, element, ns);
        }

        #endregion

        #region Private Methods

        private Type GetNodeType(string nodeName)
        {
            Type foundType = null;
            _typeDictionary.TryGetValue(nodeName, out foundType);
            return foundType;
        }

        #endregion

    }
}