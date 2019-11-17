using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core
{
    public static class MathTools
    {
        public static object Lerp(object start, object end, float by)
        {
            if (start is float && end is float)
                return Lerp((float)start, (float)end, by);
            else
                throw new NotImplementedException();
        }

        public static float Lerp(float start, float end, float by)
        {
            return start + (end - start) * by;
        }
    }
}
