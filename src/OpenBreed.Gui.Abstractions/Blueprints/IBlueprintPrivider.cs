using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Blueprints
{
    public interface IBlueprintPrivider
    {
        #region Public Methods

        IBlueprint GetBlueprint(string name);

        #endregion Public Methods
    }
}