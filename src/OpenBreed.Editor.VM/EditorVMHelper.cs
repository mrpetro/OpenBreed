using OpenBreed.Common;
using OpenBreed.Common.Database;
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

        /// <summary>
        /// This checks if database is opened already,
        /// If it is then it asks of it can be closed
        /// </summary>
        /// <returns>True if no database was opened or if previous one was closed, false otherwise</returns>
        internal static bool CheckCloseCurrentDatabase(EditorVM editor, string newDatabaseFilePath)
        {
            if (editor.Database != null)
            {
                if (Tools.GetNormalizedPath(newDatabaseFilePath) == Tools.GetNormalizedPath(editor.Database.FilePath))
                {
                    //Root.Logger.Warning("Database already opened.");
                    return false;
                }

                var answer = editor.DialogProvider.ShowMessageWithQuestion($"Another database ({editor.Database.Name}) is already opened. Do you want to close it?",
                                                                "Close current database?",
                                                                QuestionDialogButtons.OKCancel);
                if (answer != DialogAnswer.OK)
                    return false;

                if (!editor.TryCloseDatabase())
                    return false;
            }

            return true;
        }

        internal static bool TryCloseDatabase(EditorVM editor)
        {
            if (editor.Database == null)
                throw new InvalidOperationException("Expected opened database");

            if (editor.Database.IsModified)
            {
                var answer = editor.DialogProvider.ShowMessageWithQuestion("Current database has been modified. Do you want to save it before closing?",
                                                                           "Save database before closing?", QuestionDialogButtons.YesNoCancel);

                if (answer == DialogAnswer.Cancel)
                    return false;
                else if(answer == DialogAnswer.Yes)
                    editor.Database.Save();
            }

            editor.Database = null;
            editor.UnitOfWork = null;

            return true;
        }

        internal static bool TryExit(EditorVM editor)
        {
            if (editor.Database != null)
            {
                if (editor.Database.IsModified)
                {
                    var answer = editor.DialogProvider.ShowMessageWithQuestion("Current database has been modified. Do you want to save it before exiting?",
                                                                               "Save database before exiting?", QuestionDialogButtons.YesNoCancel);

                    if (answer == DialogAnswer.Cancel)
                        return false;
                    else if (answer == DialogAnswer.Yes)
                        editor.Database.Save();
                }
            }

            return true;
        }

        internal static bool TryOpenDatabase(EditorVM editor)
        {
            var openFileDialog = editor.DialogProvider.OpenFileDialog();
            openFileDialog.Title = "Select an Open Breed Editor Database file to open...";
            openFileDialog.Filter = "Open Breed Editor Database files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = DatabaseDef.DefaultDirectoryPath;

            openFileDialog.Multiselect = false;
            var answer = openFileDialog.Show();

            if (answer != DialogAnswer.OK)
                return false;

            string databaseFilePath = openFileDialog.FileName;

            if (!CheckCloseCurrentDatabase(editor, databaseFilePath))
                return false;

            editor.Database = editor.OpenXmlDatabase(databaseFilePath);

            return true;
        }

        internal static bool TrySaveDatabase(EditorVM editor)
        {
            throw new NotImplementedException();
        }

        #endregion Internal Methods

    }
}
