using CommunityToolkit.Mvvm.Input;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface.Items.EntityClasses;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.EntityClasses
{
    public class EntityClassNodeVM : BaseViewModel
    {
        #region Private Fields

        private readonly EntityClassesTreeEditorVM parentEditor;
        private bool isExpanded;
        private EntityClassNodeVM parent;
        private bool isLoaded;
        private string id;
        private bool isVisible;

        #endregion Private Fields

        #region Public Constructors

        public EntityClassNodeVM(
            EntityClassesTreeEditorVM parentEditor,
            IDbEntityClass model,
            EntityClassNodeVM parent,
            bool hasDummyChild = false)
        {
            this.parentEditor = parentEditor ?? throw new ArgumentNullException(nameof(parentEditor));
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Parent = parent;

            Id = model?.Id;

            if (hasDummyChild)
            {
                // placeholder so expand arrow appears
                Children.Add(null);
            }

            NewCommand = new RelayCommand(New);
            DeleteCommand = new RelayCommand(Delete);
        }

        #endregion Public Constructors

        #region Public Properties

        public EntityClassNodeVM Parent
        {
            get { return parent; }
            set { SetProperty(ref parent, value); }
        }

        public ObservableCollection<EntityClassNodeVM> Children { get; } = new ObservableCollection<EntityClassNodeVM>();
        public ICommand NewCommand { get; }
        public ICommand DeleteCommand { get; }

        public string Id
        {
            get => Model.Id;
            set => SetProperty(Model, x => x.Id, value);
        }

        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                SetProperty(ref isExpanded, value);

                if (isExpanded)
                {
                    LoadChildren();
                }
            }
        }

        #endregion Public Properties

        #region Internal Properties

        internal IDbEntityClass Model { get; }

        #endregion Internal Properties

        #region Private Methods

        private void Delete()
        {
            parentEditor.DeleteItem(Model);
            Parent?.Children.Remove(this);
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(Parent):
                    Model.ParentId = Parent.Id;
                    break;
                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        private string GetDefaultNewClassId()
        {
            return $"ChildOf{Id}-{Children.Count}";
        }

        private void New()
        {
            LoadChildren();

            var newClassId = GetDefaultNewClassId();
            var newClass = parentEditor.NewClass(newClassId);
            newClass.ParentId = Model.Id;
            var newClassVm = new EntityClassNodeVM(parentEditor, newClass, this, hasDummyChild: true);
            Children.Add(newClassVm);

            parentEditor.Edit(newClassVm);
        }

        private void LoadChildren()
        {
            if (isLoaded) return;

            Children.Clear();

            var childClasses = parentEditor.FindChilds(Model);

            foreach (var childClass in childClasses)
            {
                Children.Add(new EntityClassNodeVM(parentEditor, childClass, this, hasDummyChild: true));
            }

            isLoaded = true;
        }

        #endregion Private Methods
    }
}