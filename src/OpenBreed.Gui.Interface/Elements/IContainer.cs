using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Elements
{
    /// <summary>
    /// Element that can have child elements
    /// </summary>
    public interface IContainer : IElement
    {
        #region Public Properties

        IReadOnlyList<IElement> Childs { get; }

        #endregion Public Properties
    }
}