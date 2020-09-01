using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Common.Model.Scripts;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptVM : EditableEntryVM
    {

        #region Private Fields

        private string _script;
        private string _dataRef;

        #endregion Private Fields

        #region Public Constructors

        public ScriptVM()
        {
            Initialize();
        }

        private void Initialize()
        {

        }

        #endregion Public Constructors

        #region Public Properties

        public string DataRef
        {
            get { return _dataRef; }
            set { SetProperty(ref _dataRef, value); }
        }

        public string Script
        {
            get { return _script; }
            set { SetProperty(ref _script, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void FromModel(ScriptModel model)
        {
            Script = model.Script;
        }

        #endregion Public Methods

        #region Internal Methods

        internal override void FromEntry(IEntry entry)
        {
            base.FromEntry(entry);
            FromEntry((IScriptEntry)entry);
        }

        internal virtual void FromEntry(IScriptEntry entry)
        {
        }

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((IScriptEntry)entry);
        }
        internal virtual void ToEntry(IScriptEntry entry)
        {
        }

        #endregion Internal Methods

    }
}
