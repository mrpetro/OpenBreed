using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBreed.Common.Helpers
{
    public class MathHelper
    {
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
    }
}
