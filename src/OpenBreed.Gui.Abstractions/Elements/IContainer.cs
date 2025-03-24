using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Elements
{
    /// <summary>
    /// Element that can have child elements
    /// </summary>
    public interface IContainer : IElement
    {
        #region Public Properties

        IReadOnlyList<IElement> Childs { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Adds element to this container.
        /// </summary>
        /// <param name="element">Element to be added.</param>
        void AddChild(IElement element);

        /// <summary>
        /// Removes element from this container.
        /// </summary>
        /// <param name="element">Element t be removed.</param>
        void RemoveChild(IElement element);

        #endregion Public Methods
    }
}