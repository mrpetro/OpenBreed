using OpenBreed.Core.Common.Systems.Components;
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
    public interface IComponentBuilder
    {
        #region Public Methods

        /// <summary>
        /// Build component
        /// </summary>
        /// <returns>Entity component</returns>
        IEntityComponent Build();

        /// <summary>
        /// Set component property of based on given key and value 
        /// </summary>
        /// <param name="key">Property key</param>
        /// <param name="value">Property value</param>
        void SetProperty(object key, object value);

        #endregion Public Methods
    }
}
