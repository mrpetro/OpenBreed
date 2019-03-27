using OpenBreed.Common;
using OpenBreed.Common.Sprites;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Sprites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetEditorVM : EntryEditorBaseVM<ISpriteSetEntry, SpriteSetVM>
    {

        #region Public Constructors

        public SpriteSetEditorVM(IRepository repository) : base(repository)
        {
            SpriteSetViewer = new SpriteSetViewerVM();

            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName { get { return "Sprite Set Editor"; } }
        public int SelectedIndex { get; private set; }
        public SpriteSetViewerVM SpriteSetViewer { get; }

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateEntry(SpriteSetVM source, ISpriteSetEntry target)
        {
            base.UpdateEntry(source, target);
        }

        protected override void UpdateVM(ISpriteSetEntry source, SpriteSetVM target)
        {
            var model = DataProvider.SpriteSets.GetSpriteSet(source.Id);

            if (model != null)
                target.SetupSprites(model.Sprites);

            base.UpdateVM(source, target);
        }

        #endregion Protected Methods

        #region Private Methods

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Editable):
                    SpriteSetViewer.CurrentSpriteSet = Editable;
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods

    }
}
