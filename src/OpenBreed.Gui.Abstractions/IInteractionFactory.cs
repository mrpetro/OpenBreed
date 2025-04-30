using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions
{
    public interface IInteractionFactoryProvider
    {
        #region Public Methods

        IInteractionFactory GetFactory(IRenderView view);

        #endregion Public Methods
    }

    public interface IInteractionFactory
    {
        #region Public Properties

        IServiceProvider ServiceProvider { get; }
        IRenderView View { get; }

        #endregion Public Properties

        #region Public Methods

        IDesktop CreateDesktop(Action<IDesktopBuilder> setter);

        #endregion Public Methods
    }
}