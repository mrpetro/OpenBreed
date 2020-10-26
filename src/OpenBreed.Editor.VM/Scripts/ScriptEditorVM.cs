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
            RegisterSubeditor<IScriptEmbeddedEntry>((parent) => new ScriptEmbeddedEditorVM(parent));
            RegisterSubeditor<IScriptFromFileEntry>((parent) => new ScriptFromFileEditorVM(parent));
        }

        public ScriptEditorVM(IRepository repository) : base(repository, "Script Editor")
        {
        }

        #endregion Public Constructors
    }
}