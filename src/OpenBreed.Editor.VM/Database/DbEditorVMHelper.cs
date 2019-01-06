using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    internal static class DbEditorVMHelper
    {
        /// <summary>
        /// This checks if database is opened already,
        /// If it is then it asks of it can be closed
        /// </summary>
        /// <returns>True if no database was opened or if previous one was closed, false otherwise</returns>
        internal static bool CheckCloseCurrentDatabase(DbEditorVM dbEditor, string newDatabaseFilePath)
        {
            if (dbEditor.CurrentDb != null)
            {
                if (Tools.GetNormalizedPath(newDatabaseFilePath) == Tools.GetNormalizedPath(dbEditor.CurrentDb.FilePath))
                {
                    //Root.Logger.Warning("Database already opened.");
                    return false;
                }

                var answer = dbEditor.Root.DialogProvider.ShowMessageWithQuestion($"Another database ({dbEditor.CurrentDb.Name}) is already opened. Do you want to close it?",
                                                                "Close current database?",
                                                                QuestionDialogButtons.OKCancel);
                if (answer != DialogAnswer.OK)
                    return false;

                if (!dbEditor.Root.DbEditor.TryCloseDatabase())
                    return false;
            }

            return true;
        }

        internal static bool TryCloseDatabase(DbEditorVM dbEditor)
        {
            if (dbEditor.CurrentDb == null)
                throw new InvalidOperationException("Expected opened database");

            if (dbEditor.CurrentDb.IsModified)
            {
                var answer = dbEditor.Root.DialogProvider.ShowMessageWithQuestion("Current database has been modified. Do you want to save it before closing?",
                                                                           "Save database before closing?", QuestionDialogButtons.YesNoCancel);

                if (answer == DialogAnswer.Cancel)
                    return false;
                else if (answer == DialogAnswer.Yes)
                    dbEditor.CurrentDb.Save();
            }

            dbEditor.CurrentDb = null;
            dbEditor.Root.UnitOfWork = null;

            return true;
        }

        internal static bool TryOpenDatabase(DbEditorVM dbEditor)
        {
            var openFileDialog = dbEditor.Root.DialogProvider.OpenFileDialog();
            openFileDialog.Title = "Select an Open Breed Editor Database file to open...";
            openFileDialog.Filter = "Open Breed Editor Database files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = DatabaseDef.DefaultDirectoryPath;

            openFileDialog.Multiselect = false;
            var answer = openFileDialog.Show();

            if (answer != DialogAnswer.OK)
                return false;

            string databaseFilePath = openFileDialog.FileName;

            if (!CheckCloseCurrentDatabase(dbEditor, databaseFilePath))
                return false;

            dbEditor.CurrentDb = dbEditor.OpenXmlDatabase(databaseFilePath);

            return true;
        }

    }
}
