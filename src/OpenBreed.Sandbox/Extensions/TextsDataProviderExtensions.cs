using Microsoft.Extensions.Logging;
using OpenBreed.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Extensions
{
    public static class TextsDataProviderExtensions
    {
        #region Public Methods

        public static string GetTextString(this TextsDataProvider textsDataProvider, string entryId)
        {
            return textsDataProvider.GetTextById(entryId).Text;
        }

        #endregion Public Methods
    }
}