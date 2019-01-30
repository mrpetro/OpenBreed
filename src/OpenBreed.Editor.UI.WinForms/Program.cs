using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenBreed.Common.Sounds;
using System.IO;
using OpenABEd.Modules;
using OpenABEdCfg;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.UI.WinForms;
using OpenBreed.Editor.UI.WinForms.Forms;
using OpenBreed.Common;
using OpenBreed.Editor.VM.Database;

namespace OpenBreed.Editor.UI.WinForms
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new GLTestForm());

            ServiceLocator.Instance.RegisterService<IDialogProvider>(new DialogProvider());

            using (var editor = new EditorVM())
            {
                editor.Run();
            }
        }
    }
}
