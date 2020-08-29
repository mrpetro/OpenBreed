using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Data;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Texts;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Database.Xml.Items.Assets;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Texts
{
    public class TextEmbeddedVM : TextVM
    {

        #region Private Fields

        #endregion Private Fields

        #region Public Properties


        #endregion Public Properties

        #region Internal Methods

        internal override void FromEntry(ITextEntry entry)
        {
            base.FromEntry(entry);
            FromEntry((ITextEmbeddedEntry)entry);
        }

        internal override void ToEntry(ITextEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((ITextFromMapEntry)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        private void FromEntry(ITextEmbeddedEntry entry)
        {
            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var model = dataProvider.Texts.GetText(entry.Id);

            if (model != null)
                Text = model.Text;

            DataRef = entry.DataRef;
        }

        private void ToEntry(ITextEmbeddedEntry entry)
        {
            entry.DataRef = DataRef;
        }

        #endregion Private Methods

    }
}
