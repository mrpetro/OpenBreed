using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Data;
using OpenBreed.Common.Model.Scripts;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Xml.Items.Assets;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptEmbeddedVM : ScriptVM
    {

        #region Private Fields

        #endregion Private Fields

        #region Public Properties


        #endregion Public Properties

        #region Internal Methods

        internal override void FromEntry(IScriptEntry entry)
        {
            base.FromEntry(entry);
            FromEntry((IScriptEmbeddedEntry)entry);
        }

        internal override void ToEntry(IScriptEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((IScriptEmbeddedEntry)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        private void FromEntry(IScriptEmbeddedEntry entry)
        {
            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var model = dataProvider.Scripts.GetScript(entry.Id);

            if (model != null)
                Script = model.Script;

            DataRef = entry.DataRef;
        }

        private void ToEntry(IScriptEmbeddedEntry entry)
        {
            entry.DataRef = DataRef;
        }

        #endregion Private Methods

    }
}
