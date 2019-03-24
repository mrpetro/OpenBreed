using OpenBreed.Common;
using OpenBreed.Common.Texts;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Texts
{
    public class TextEditorVM : EntryEditorBaseVM<ITextEntry, TextVM>
    {

        #region Public Constructors

        public TextEditorVM(IRepository repository) : base(repository)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName { get { return "Text Editor"; } }

        #endregion Public Properties

        #region Protected Methods

        protected override TextVM CreateVM(ITextEntry entry)
        {
            if (entry is ITextEmbeddedEntry)
                return new TextEmbeddedVM();
            else if (entry is ITextFromMapEntry)
                return new TextFromMapVM();
            else
                throw new NotImplementedException();
        }

        protected override void UpdateEntry(TextVM source, ITextEntry target)
        {
            var model = DataProvider.Texts.GetText(target.Id);

            model.Text = source.Text;

            base.UpdateEntry(source, target);
        }

        #endregion Protected Methods

    }
}
