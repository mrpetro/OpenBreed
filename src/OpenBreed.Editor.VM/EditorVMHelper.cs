using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Editor.VM.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM
{
    public static class EditorVMHelper
    {

        #region Internal Methods

        internal static bool TryExit(EditorVM editor)
        {
            if (editor.DbEditor.Editable != null)
            {
                if (editor.DbEditor.Editable.IsModified)
                {
                    var answer = editor.DialogProvider.ShowMessageWithQuestion("Current database has been modified. Do you want to save it before exiting?",
                                                                               "Save database before exiting?", QuestionDialogButtons.YesNoCancel);

                    if (answer == DialogAnswer.Cancel)
                        return false;
                    else if (answer == DialogAnswer.Yes)
                        editor.DbEditor.Save();
                }
            }

            return true;
        }



        internal static bool TrySaveDatabase(EditorVM editor)
        {
            throw new NotImplementedException();
        }

        #endregion Internal Methods

    }
}
