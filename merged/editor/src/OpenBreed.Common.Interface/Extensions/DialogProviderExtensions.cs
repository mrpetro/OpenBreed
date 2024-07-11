using OpenBreed.Common.Interface.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Extensions
{
    public static class DialogProviderExtensions
    {
        #region Public Methods

        public static void ShowNotImplementedMessage(this IDialogProvider dialogProvider, string featureName)
        {
            dialogProvider.ShowMessage($"The feature '{featureName}' is not implemented yet. Please don't use it yet.", "Feature not implemented yet.");
        }

        #endregion Public Methods
    }
}