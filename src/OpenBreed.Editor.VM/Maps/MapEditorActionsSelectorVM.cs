using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorActionsSelectorVM : BaseViewModel
    {

        #region Public Constructors

        public MapEditorActionsSelectorVM(MapEditorActionsToolVM parent)
        {
            Parent = parent;

            Items = new BindingList<ActionVM>();
            Items.ListChanged += (s, a) => OnPropertyChanged(nameof(Items));
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<ActionVM> Items { get; }

        public MapEditorActionsToolVM Parent { get; }
        public int SelectedIndex { get; set; }

        #endregion Public Properties

    }
}
