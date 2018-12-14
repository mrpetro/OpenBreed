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

        #endregion Public Methods


    }
}
