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
            RegisterSubeditor<IScriptEmbeddedEntry, IScriptEntry>();
            RegisterSubeditor<IScriptFromFileEntry, IScriptEntry>();
        }

        public ScriptEditorVM(IManagerCollection managerCollection, IWorkspaceMan workspaceMan, IDialogProvider dialogProvider) : base(managerCollection, workspaceMan, dialogProvider, "Script Editor")
        {
        }

        #endregion Public Constructors
    }
}