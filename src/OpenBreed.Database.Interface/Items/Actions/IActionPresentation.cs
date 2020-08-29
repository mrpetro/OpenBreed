using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Actions
{
    public interface IActionPresentation
    {
        #region Public Properties

        string Color { get; set; }
        string Image { get; set; }
        bool Visibility { get; set; }

        #endregion Public Properties
    }
}
