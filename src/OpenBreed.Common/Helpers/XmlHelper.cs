using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.Helpers
{
    /// <summary>
    /// Helper class with various xml related methods
    /// </summary>
    public static class XmlHelper
    {
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
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
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
        /// Deserializes object from xml file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
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
    }
}
