//using OpenBreed.Common;
//using OpenBreed.Editor.VM.Base;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OpenBreed.Editor.VM.Sprites
//{
//    public class SpriteViewerVM : BaseViewModel
//    {

//        #region Private Fields

//        private int _currentIndex = -1;
//        private SpriteVM _currentItem = null;

//        #endregion Private Fields

//        #region Public Constructors

//        public SpriteViewerVM(EditorVM root)
//        {
//            Root = root;

//            Items = new BindingList<SpriteVM>();
//            Items.ListChanged += (s, a) => OnPropertyChanged(nameof(Items));

//            PropertyChanged += SpriteViewerVM_PropertyChanged;
//        }

//        public void Connect()
//        {
//            Root.LevelEditor.SpriteSetViewer.PropertyChanged += SpriteSetSelector_PropertyChanged;
//        }

//        #endregion Public Constructors

//        #region Public Properties

//        public int CurrentIndex
//        {
//            get { return _currentIndex; }
//            set { SetProperty(ref _currentIndex, value); }
//        }

//        public SpriteVM CurrentItem
//        {
//            get { return _currentItem; }
//            set { SetProperty(ref _currentItem, value); }
//        }

//        public BindingList<SpriteVM> Items { get; private set; }

//        public EditorVM Root { get; private set; }

//        #endregion Public Properties

//        #region Private Methods

//        private void SpriteSetSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            var spriteSetSelector = sender as SpriteSetSelectorVM;

//            switch (e.PropertyName)
//            {
//                case nameof(spriteSetSelector.CurrentItem):
//                    UpdateWithSpriteSet(spriteSetSelector.CurrentItem);
//                    break;
//                default:
//                    break;
//            }
//        }

//        private void UpdateWithSpriteSet(SpriteSetVM spriteSet)
//        {
//            try
//            {
//                Items.RaiseListChangedEvents = false;
//                Items.Clear();

//                if (spriteSet == null)
//                    return;

//                spriteSet.Items.ForEach(item => Items.Add(item));
//            }
//            finally
//            {
//                Items.RaiseListChangedEvents = true;
//                Items.ResetBindings();
//            }
//        }

//        private void SpriteViewerVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            switch (e.PropertyName)
//            {
//                case nameof(CurrentIndex):
//                    UpdateCurrentItem();
//                    break;
//                case nameof(CurrentItem):
//                    UpdateCurrentIndex();
//                    break;
//                case nameof(Items):
//                    CurrentItem = Items.FirstOrDefault();
//                    break;
//                default:
//                    break;
//            }
//        }

//        private void UpdateCurrentIndex()
//        {
//            if (Root.LevelEditor.SpriteSetViewer.CurrentItem == null)
//                CurrentIndex = -1;
//            else
//                CurrentIndex = Items.IndexOf(CurrentItem);
//        }

//        private void UpdateCurrentItem()
//        {
//            if (CurrentIndex == -1)
//                CurrentItem = null;
//            else
//                CurrentItem = Items[CurrentIndex];
//        }

//        #endregion Private Methods

//    }
//}
