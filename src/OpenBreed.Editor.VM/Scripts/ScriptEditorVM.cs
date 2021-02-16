using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Scripts;
using System;
using System.Collections.Generic;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptEditorVM : ParentEntryEditor<IScriptEntry>
    {
        #region Public Constructors

        static ScriptEditorVM()
        {
            RegisterSubeditor<IScriptEmbeddedEntry>((parent) => new ScriptEmbeddedEditorVM(parent.DataProvider.Scripts));
            RegisterSubeditor<IScriptFromFileEntry>((parent) => new ScriptFromFileEditorVM(parent.WorkspaceMan, parent.DataProvider.Scripts, parent.DataProvider));
        }

        public ScriptEditorVM(IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(workspaceMan, dataProvider, dialogProvider, "Script Editor")
        {
        }

        #endregion Public Constructors
    }
}