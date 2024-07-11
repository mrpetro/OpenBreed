using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Actions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorActionsSelectorVM : BaseViewModel
    {
        #region Private Fields

        private int _selectedIndex;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorActionsSelectorVM(
            MapEditorActionsToolVM parent,
            IDrawingFactory drawingFactory,
            IDrawingContextProvider drawingContextProvider)
        {
            Parent = parent;
            this.drawingFactory = drawingFactory;
            this.drawingContextProvider = drawingContextProvider;
            Items = new ObservableCollection<ActionVM>();
            Items.CollectionChanged += (s, a) => OnPropertyChanged(nameof(Items));

            Parent.PropertyChanged += Parent_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public ObservableCollection<ActionVM> Items { get; }
        public MapEditorActionsToolVM Parent { get; }

        #endregion Public Properties

        #region Internal Properties

        #endregion Internal Properties

        #region Private Methods

        private int _currentItemIndex;
        private readonly IDrawingFactory drawingFactory;
        private readonly IDrawingContextProvider drawingContextProvider;

        public int CurrentItemIndex
        {
            get { return _currentItemIndex; }
            set { SetProperty(ref _currentItemIndex, value); }
        }

        private void OnActionSetChanged()
        {
            Items.Clear();

            if (Parent.Parent.ActionSet is null)
            {
                return;
            }

            foreach (var actionModel in Parent.Parent.ActionSet.Items)
            {
                Items.Add(new ActionVM(drawingFactory, drawingContextProvider, actionModel, null));
            }
        }

        private void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Parent.CurrentActionSetRef):
                    OnActionSetChanged();
                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}