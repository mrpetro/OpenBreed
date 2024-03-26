using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using System.Windows.Forms;
using System.Windows;

namespace OpenBreed.Editor.UI.WinForms.Helpers
{
    public static class WpfHelper
    {
        public static ElementHost CreateWpfControl<TControl>(object viewModel) where TControl : FrameworkElement, new()
        {
            var elementHost = new ElementHost();
            var control = new TControl();
            control.DataContext = viewModel;
            elementHost.Child = control;

            return elementHost;
        }
    }
}
