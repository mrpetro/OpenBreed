using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Actions
{
    public interface IActionPresentation
    {
        #region Public Properties

        string Color { get; }
        string Image { get; }
        bool Visibility { get; }

        #endregion Public Properties
    }
}
