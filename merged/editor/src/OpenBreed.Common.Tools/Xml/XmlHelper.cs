using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Common.Tools.Xml
{
    /// <summary>
    /// Helper class with various xml related methods
    /// </summary>
    public static class XmlHelper
    {
        #region Private Fields

        private static readonly Regex variableRegexPattern = new Regex(@"\$\((\w+)\)");

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Serializes object to xml file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        public static void StoreAsXml<T>(string path, object obj, bool createDir = false)
        {
            // Verify input
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (path == null)
                throw new ArgumentNullException("path");
            if (path == string.Empty)
                throw new InvalidOperationException("path cannot be empty");

            // Path
            path = Path.GetFullPath(path);

            // Create directory
            if (createDir)
            {
                var dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }

            // Serialize
            XmlSerializer ser = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            using (var s = new FileStream(path, FileMode.Create))
            {
                ser.Serialize(s, obj, ns);
            }
        }

        /// <summary>
        /// Deserializes object from stream
        /// </summary>
        /// <typeparam name="T">Type of object to restore</typeparam>
        /// <param name="stream">Stream instance</param>
        /// <returns>Object instance</returns>
        public static T RestoreFromXml<T>(Stream stream)
        {
            // Verify input
            if (stream == null)
                throw new ArgumentNullException("stream");

            // Deserialize
            var serializer = new XmlSerializer(typeof(T));

            return (T)serializer.Deserialize(stream);
        }

        /// <summary>
        /// Deserializes object from XML Reader
        /// </summary>
        /// <typeparam name="T">Type of object to restore</typeparam>
        /// <param name="xmlReader">XmlReader instance</param>
        /// <returns>Object instance</returns>
        public static T RestoreFromXml<T>(XmlReader xmlReader)
        {
            // Verify input
            if (xmlReader == null)
                throw new ArgumentNullException("xmlReader");

            // Deserialize
            var serializer = new XmlSerializer(typeof(T));

            return (T)serializer.Deserialize(xmlReader);
        }

        /// <summary>
        /// Deserializes object from Text Reader
        /// </summary>
        /// <typeparam name="T">Type of object to restore</typeparam>
        /// <param name="textReader">TextReader instance</param>
        /// <returns>Object instance</returns>
        public static T RestoreFromXml<T>(TextReader textReader)
        {
            // Verify input
            if (textReader == null)
                throw new ArgumentNullException("textReader");

            // Deserialize
            var serializer = new XmlSerializer(typeof(T));

            return (T)serializer.Deserialize(textReader);
        }

        /// <summary>
        /// Deserializes object from file with given path
        /// </summary>
        /// <typeparam name="T">Type of object to restore</typeparam>
        /// <param name="path">Path to XML file</param>
        /// <returns>Object instance</returns>
        public static T RestoreFromXml<T>(string path)
        {
            // Verify input
            if (path == null)
                throw new ArgumentNullException("path");
            if (path == string.Empty)
                throw new InvalidOperationException("path cannot be empty");

            // Deserialize
            using (var s = File.Open(path, FileMode.Open))
            {
                return RestoreFromXml<T>(s);
            }
        }

        /// <summary>
        /// Deserializes object from file with given path
        /// </summary>
        /// <typeparam name="T">Type of object to restore</typeparam>
        /// <param name="path">Path to XML file</param>
        /// <param name="variables">Dictionary of variables</param>
        /// <returns>Object instance</returns>
        public static T RestoreFromXml<T>(string path, Dictionary<string, string> variables)
        {
            // Verify input
            if (path == null)
                throw new ArgumentNullException("path");

            if (path == string.Empty)
                throw new InvalidOperationException("path cannot be empty");

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            ReplaceVariablesWithValues(xmlDoc, variables);

            return RestoreFromXml<T>(xmlDoc);
        }

        /// <summary>
        /// Deserializes object from XmlDocument
        /// </summary>
        /// <typeparam name="T">Type of object to restore</typeparam>
        /// <param name="xmlDocument">XmlDocument instance</param>
        /// <returns>Object instance</returns>
        public static T RestoreFromXml<T>(XmlDocument xmlDocument)
        {
            // Verify input
            if (xmlDocument is null)
                throw new ArgumentNullException("xmlDocument");

            using (var reader = new XmlNodeReader(xmlDocument))
                return RestoreFromXml<T>(reader);
        }

        #endregion Public Methods

        #region Private Methods

        private static string ReplaceVariablesWithValues(string input, Dictionary<string, string> variables)
        {
            return variableRegexPattern.Replace(input, (match) =>
            {
                var varName = match.Groups[1].Value;
                if (variables.TryGetValue(varName, out string varValue))
                    return varValue;
                else
                {
                    Console.WriteLine($"Error: Variable '{varName}' not set.");

                    return varName;
                }
            });
        }

        private static void ReplaceVariablesWithValues(XmlNode xmlNode, Dictionary<string, string> variables)
        {
            if (xmlNode.NodeType == XmlNodeType.Text)
                xmlNode.Value = ReplaceVariablesWithValues(xmlNode.Value, variables);

            if (xmlNode.Attributes != null)
            {
                foreach (XmlAttribute xmlAttribute in xmlNode.Attributes)
                    xmlAttribute.Value = ReplaceVariablesWithValues(xmlAttribute.Value, variables);
            }

            foreach (XmlNode childNode in xmlNode.ChildNodes)
                ReplaceVariablesWithValues(childNode, variables);
        }

        #endregion Private Methods
    }
}