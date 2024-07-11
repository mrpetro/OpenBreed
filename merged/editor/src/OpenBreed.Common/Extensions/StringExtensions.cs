using OpenBreed.Reader.Legacy.Maps.MAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Extensions
{
    public static class StringExtensions
    {
        #region Public Methods

        public static MAPFormat ToMapFormat(this string formatString)
        {
            if (!Enum.TryParse<MAPFormat>(formatString, out MAPFormat format))
            {
                throw new NotImplementedException($"Map format '{formatString}' not implemented.");
            }

            return format;
        }

        #endregion Public Methods
    }
}