using OpenBreed.Common.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Texts
{
    public interface ITextEntry : IEntry
    {
        #region Public Properties

        string DataRef { get; set; }

        #endregion Public Properties
    }
}
