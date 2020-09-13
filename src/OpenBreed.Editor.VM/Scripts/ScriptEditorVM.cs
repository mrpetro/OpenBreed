using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Model.Scripts;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Editor.VM.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptEditorVM : EntryEditorBaseVM<IScriptEntry, ScriptVM>
    {

        #region Public Constructors

        public ScriptEditorVM(IRepository repository) : base(repository)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName { get { return "Script Editor"; } }

        #endregion Public Properties

        #region Protected Methods

        protected override ScriptVM CreateVM(IScriptEntry entry)
        {
            if (entry is IScriptEmbeddedEntry)
                return new ScriptEmbeddedVM();
            else if (entry is IScriptFromFileEntry)
                return new ScriptFromFileVM();
            else
                throw new NotImplementedException();
        }

        protected override void UpdateEntry(ScriptVM source, IScriptEntry target)
        {
            var model = DataProvider.Scripts.GetScript(target.Id);

            model.Script = source.Script;

            base.UpdateEntry(source, target);
        }

        #endregion Protected Methods

    }
}
