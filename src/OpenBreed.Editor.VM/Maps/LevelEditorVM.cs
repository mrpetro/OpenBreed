using OpenBreed.Common.Database.Items.Images;
using OpenBreed.Common.Database.Items.Levels;
using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Images
{
    public class LevelEditorVM : BaseViewModel
    {
        #region Public Constructors

        public LevelEditorVM(EditorVM root)
        {
            Root = root;
        }

        #endregion Public Constructors

        #region Public Properties

        public EditorVM Root { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void TryClose()
        {

        }

        public void TryLoad(LevelDef levelDef)
        {
            Root.TileSets.Clear();
            Root.AddTileSet(levelDef.TileSetResourceRef);

            Root.TileSetSelector.CurrentItem = Root.TileSets.FirstOrDefault();

            Root.SpriteSets.Clear();
            foreach (var spriteSetSourceRef in levelDef.SpriteSetResourceRefs)
                Root.AddSpriteSet(spriteSetSourceRef);

            Root.SpriteSetViewer.CurrentItem = Root.SpriteSets.FirstOrDefault();
            if (Root.SpriteSetViewer.CurrentItem != null)
                Root.SpriteViewer.CurrentItem = Root.SpriteSetViewer.CurrentItem.Items.FirstOrDefault();

            if (levelDef.PropertySetRef != null)
                Root.LoadPropSet(levelDef.PropertySetRef);

            var mapSourceDef = Root.Database.GetSourceDef(levelDef.MapResourceRef);
            if (mapSourceDef != null)
                Root.Map.Load(mapSourceDef);
        }

        #endregion Public Methods


    }
}
