using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Actions
{
    public interface IActionSetEntry : IEntry
    {

        #region Public Properties

        List<IActionEntry> Items { get; }

        #endregion Public Properties

        #region Public Methods

        IActionEntry NewItem();

        #endregion Public Methods

    }
}
