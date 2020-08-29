using OpenBreed.Common;
using OpenBreed.Common.Helpers;
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
        /// <summary>
        /// This checks if database is opened already,
        /// If it is then it asks of it can be closed
        /// </summary>
        /// <returns>True if no database was opened or if previous one was closed, false otherwise</returns>
        internal static bool CheckCloseCurrentDatabase(DbEditorVM dbEditor, string newDatabaseFilePath)
        {
            if (dbEditor.Editable != null)
            {
                if (IOHelper.GetNormalizedPath(newDatabaseFilePath) == IOHelper.GetNormalizedPath(dbEditor.Editable.Name))
                {
                    //Root.Logger.Warning("Database already opened.");
                    return false;
                }

                var answer = ServiceLocator.Instance.GetService<IDialogProvider>().ShowMessageWithQuestion($"Another database ({dbEditor.Editable.Name}) is already opened. Do you want to close it?",
                                                                "Close current database?",
                                                                QuestionDialogButtons.OKCancel);
                if (answer != DialogAnswer.OK)
                    return false;

                if (!dbEditor.TryCloseDatabase())
                    return false;
            }

            return true;
        }

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

        internal static bool TryOpenXmlDatabase(DbEditorVM dbEditor)
        {
            var openFileDialog = ServiceLocator.Instance.GetService<IDialogProvider>().OpenFileDialog();
            openFileDialog.Title = "Select an Open Breed Editor Database file to open...";
            openFileDialog.Filter = "Open Breed Editor Database files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = XmlDatabase.DefaultDirectoryPath;
            openFileDialog.Multiselect = false;

            var answer = openFileDialog.Show();

            if (answer != DialogAnswer.OK)
                return false;

            string databaseFilePath = openFileDialog.FileName;

            if (!CheckCloseCurrentDatabase(dbEditor, databaseFilePath))
                return false;

            var unitOfWork = XmlDatabaseMan.Open(databaseFilePath).CreateUnitOfWork();
            dbEditor.EditModel(unitOfWork);
            return true;
        }

    }
}
