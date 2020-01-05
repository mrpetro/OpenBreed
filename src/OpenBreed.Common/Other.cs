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
            catch (Exception ex)
            {
                LogMan.Instance.Error(ex);
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
                LogMan.Instance.Error(ex);
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
