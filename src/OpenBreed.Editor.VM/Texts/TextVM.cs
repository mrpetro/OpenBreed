using OpenBreed.Editor.VM.Base;
using OpenBreed.Common.Palettes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;
using OpenBreed.Common.Texts;

namespace OpenBreed.Editor.VM.Texts
{
    public class TextVM : EditableEntryVM
    {

        #region Private Fields

        private string _text;
        private string _dataRef;

        #endregion Private Fields

        #region Public Constructors

        public TextVM()
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

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void FromModel(TextModel model)
        {
            Text = model.Text;
        }

        #endregion Public Methods

        #region Internal Methods

        internal override void FromEntry(IEntry entry)
        {
            base.FromEntry(entry);
            FromEntry((ITextEntry)entry);
        }

        internal virtual void FromEntry(ITextEntry entry)
        {
        }

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((ITextEntry)entry);
        }
        internal virtual void ToEntry(ITextEntry entry)
        {
        }

        #endregion Internal Methods

    }
}
