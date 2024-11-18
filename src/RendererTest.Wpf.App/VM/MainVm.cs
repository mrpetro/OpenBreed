using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RendererTest.Wpf.App.VM
{
    public class MainVm : BaseViewModel
    {
        public MainVm(IServiceProvider serviceProvider, LoggerVM logger, Func<IServiceProvider, RendererVm> viewFactory)
        {
            Logger = logger;
            View = viewFactory.Invoke(serviceProvider);
        }

        public RendererVm View { get; }
        public LoggerVM Logger { get; }
    }
}
