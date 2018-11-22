using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using OpenBreed.Common.Logging;

namespace OpenBreed.Common
{
    public static class Tools
    {

        /// <summary>
        /// Tries to do some action
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="arg1"></param>
        public static bool TryAction(Action a)
        {
            try
            {
                a();
                return true;
            }
            catch (Exception ex)
            {
                LogMan.Instance.LogError(ex);
                return false;
            }
        }

        //public static bool IsInDesignMode(IComponent component)
        //{
        //    bool ret = false;
        //    if (null != component)
        //    {
        //        ISite site = component.Site;
        //        if (null != site)
        //        {
        //            ret = site.DesignMode;
        //        }
        //        else if (component is System.Windows.Forms.Control)
        //        {
        //            IComponent parent = ((System.Windows.Forms.Control)component).Parent;
        //            ret = IsInDesignMode(parent);
        //        }
        //    }

        //    return ret;
        //}

        public static string GetNormalizedPath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToUpperInvariant();
        }



        /// <summary>
        /// Tries to do some action
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="arg1"></param>
        public static bool TryAction<T>(Action<T> a, T arg1)
        {
            try
            {
                a(arg1);
                return true;
            }
            catch (Exception ex)
            {
                LogMan.Instance.LogError(ex);
                return false;
            }
        }


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

        public static string ConvertLineBreaksCRLFToLF(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return input.Replace("\r\n", "\n");
        }

        public static string ConvertLineBreaksLFToCRLF(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return input.Replace("\n", "\r\n");
        }

        /// <summary>
        /// Rounds integer to next closest power of 2
        /// </summary>
        /// <param name="x">number to round</param>
        /// <returns>next closest power of 2 number</returns>
        public static int ToNextPowOf2(int x)
        {
            if (x < 0) { return 0; }
            --x;
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            return x + 1;
        }

        /// <summary>
        /// Gets current time
        /// </summary>
        /// <returns></returns>
        public static string TimeNowForFilename()
        {
            return DateTime.Now.ToString("yyMMdd-HHmmss");
        }

    }
}
