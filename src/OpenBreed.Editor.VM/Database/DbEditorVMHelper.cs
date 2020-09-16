using OpenBreed.Common;
using OpenBreed.Database.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    internal static class DbEditorVMHelper
    {
        internal static bool TryCloseDatabase(DbEditorVM dbEditor)
        {
            if (dbEditor.Editable == null)
                throw new InvalidOperationException("Expected current database");

            if (dbEditor.Editable.IsModified)
            {
                var answer = ServiceLocator.Instance.GetService<IDialogProvider>().ShowMessageWithQuestion("Current database has been modified. Do you want to save it before closing?",
                                                                           "Save database before closing?", QuestionDialogButtons.YesNoCancel);

                if (answer == DialogAnswer.Cancel)
                    return false;
                else if (answer == DialogAnswer.Yes)
                    dbEditor.Save();
            }

            dbEditor.Editable.Dispose();
            dbEditor.Editable = null;

            return true;
        }

        internal static bool TrySaveDatabase(DbEditorVM dbEditor)
        {
            if (dbEditor.Editable == null)
                throw new InvalidOperationException("Expected current database");

            dbEditor.Save();
            return true;
        }
    }
}
