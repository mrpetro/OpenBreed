using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBreed.Common.UI.WinForms.Extensions
{
    public static class SynchronizeInvokeExtensions
    {
        public static void InvokeIfRequired(this ISynchronizeInvoke obj,
                                                 MethodInvoker action)
        {
            if (obj.InvokeRequired)
            {
                var args = new object[0];
                obj.Invoke(action, args);
            }
            else
            {
                action();
            }
        }
    }
}
