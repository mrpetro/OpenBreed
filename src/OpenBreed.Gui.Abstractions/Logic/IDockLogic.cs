using OpenBreed.Gui.Abstractions.Elements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Logic
{
    public interface IDockLogic
    {
        #region Public Methods

        void Apply(IElement dockable, Box2 hostBox);

        #endregion Public Methods
    }
}