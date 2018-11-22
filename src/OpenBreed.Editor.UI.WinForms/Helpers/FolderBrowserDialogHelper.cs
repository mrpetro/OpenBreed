using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenBreed.Editor.UI.WinForms.Helpers
{
    public class FolderBrowserDialogHelper
    {
        public static void PrepareToSelectABTAGameFolder(FolderBrowserDialog dialog, string initFolderPath)
        {
            dialog.Reset();
            dialog.Description = "Select Alien Breed: Tower Assault game folder...";
            dialog.SelectedPath = initFolderPath;
            dialog.ShowNewFolderButton = false;
        }
    }
}
