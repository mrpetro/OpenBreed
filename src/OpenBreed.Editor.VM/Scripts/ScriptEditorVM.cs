using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Scripts;
using System;
using System.Collections.Generic;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptEditorVM : ParentEntryEditor<IDbScript>
    {
        #region Public Constructors

        public ScriptEditorVM(DbEntrySubEditorFactory subEditorFactory, IWorkspaceMan workspaceMan, IDialogProvider dialogProvider) : base(subEditorFactory, workspaceMan, dialogProvider, "Script Editor")
        {
        }

        #endregion Public Constructors
    }
}