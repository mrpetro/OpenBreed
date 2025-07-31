using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Abstractions
{
    public interface IPicture
    {
        #region Public Properties

        /// <summary>
        /// Id of this image
        /// </summary>
        int Id { get; }

        #endregion Public Properties
    }
}