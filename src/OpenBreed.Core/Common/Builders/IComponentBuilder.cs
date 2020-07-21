using OpenBreed.Core.Common.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Common.Builders
{
    /// <summary>
    /// Entity component builder interface
    /// </summary>
    public interface IComponentBuilder : IPropertySetter
    {
        #region Public Methods

        /// <summary>
        /// Build component
        /// </summary>
        /// <returns>Entity component</returns>
        IEntityComponent Build();

        #endregion Public Methods
    }
}
