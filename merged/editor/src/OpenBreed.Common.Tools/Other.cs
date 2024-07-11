using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;

namespace OpenBreed.Common.Tools
{
    public static class Other
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
            catch
            {
                return false;
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
    }
}
