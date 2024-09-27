using OpenBreed.Rendering.Interface.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Interface.Events
{
    public class RenderContextInitializedEvent : EventArgs
    {
        #region Public Constructors

        public RenderContextInitializedEvent(IRenderContext renderContext)
        {
            RenderContext = renderContext;
        }

        #endregion Public Constructors

        #region Public Properties

        public IRenderContext RenderContext { get; }

        #endregion Public Properties
    }
}