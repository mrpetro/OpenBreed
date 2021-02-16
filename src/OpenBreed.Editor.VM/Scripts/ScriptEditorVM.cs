using OpenBreed.Common;
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
            RegisterSubeditor<IScriptEmbeddedEntry>((workspaceMan, dataProvider, dialogProvider) => new ScriptEmbeddedEditorVM(dataProvider.Scripts));
            RegisterSubeditor<IScriptFromFileEntry>((workspaceMan, dataProvider, dialogProvider) => new ScriptFromFileEditorVM(workspaceMan, dataProvider.Scripts, dataProvider));
        }

        public ScriptEditorVM(IManagerCollection managerCollection, IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(managerCollection, workspaceMan, dataProvider, dialogProvider, "Script Editor")
        {
        }

        #endregion Public Constructors
    }
}