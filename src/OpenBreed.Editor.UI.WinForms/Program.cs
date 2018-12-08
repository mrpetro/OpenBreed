using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenBreed.Common.Sound;
using System.IO;
using OpenABEd.Modules;
using OpenABEdCfg;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.UI.WinForms;
using OpenBreed.Editor.UI.WinForms.Forms;

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
            var dialogProvider = new DialogProvider();
            //dialogProvider.Register(
            //dialogProvider.ShowDialog("", 

            using (var editor = new EditorVM(dialogProvider))
            {
                editor.Initialize();
                editor.Run();
                InitUI(editor);
            }

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());

            //for (int i = 1; i <= 4; i++)
            //{
            //    string pcmSampleFileName = string.Format(@"D:\Games\Alien Breed Tower Assault Enhanced (1994)(Psygnosis Team 17)\extract\ALIEN{0:D1}", i);

            //    PCMPlayer pcmPlayer = new PCMPlayer(pcmSampleFileName, 11025, 8, 1);
            //    pcmPlayer.PlaySync();

            //}

            //for (int i = 1; i <= 27; i++)
            //{
            //    string pcmSampleFileName = string.Format(@"D:\Games\Alien Breed Tower Assault Enhanced (1994)(Psygnosis Team 17)\extract\SPEECH{0:D2}", i);

            //    PCMPlayer pcmPlayer = new PCMPlayer(pcmSampleFileName, 10000, 8, 1);
            //    pcmPlayer.PlaySync();

            //}
        }

        private static void InitUI(EditorVM editor)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new MainForm();
            mainForm.Initialize(editor);
            Application.Run(mainForm);
        }
    }
}
