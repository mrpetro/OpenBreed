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

        #region Public Methods

        /// <summary>
        /// Load this picture into render context
        /// </summary>
        /// <param name="renderContext">Render context</param>
        void Load(IRenderContext renderContext);

        #endregion Public Methods
    }
}