using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Core
{
    public static class Other
    {
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
