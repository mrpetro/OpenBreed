using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RendererTest.Wpf.App.VM
{
    public class MainVm : BaseViewModel
    {
        public MainVm(IServiceProvider serviceProvider, Func<IServiceProvider, RendererVm> viewFactory)
        {
            ViewLeft = viewFactory.Invoke(serviceProvider);
            ViewRight = viewFactory.Invoke(serviceProvider);
        }

        public RendererVm ViewLeft { get; }
        public RendererVm ViewRight { get; }
    }
}
