using CommunityToolkit.Mvvm.Input;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.EntityClasses;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Maps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace OpenBreed.Editor.VM.EntityClasses
{
    public class EntityClassesTreeEditorVM : BaseViewModel
    {
        #region Private Fields

        private readonly IRepository<IDbEntityClass> entityClassRepository;

        private EntityClassEditorVM classEditor;
        private EntityClassNodeVM selectedClass;

        #endregion Private Fields

        #region Public Constructors

        public EntityClassesTreeEditorVM(IRepositoryProvider repositoryProvider)
        {
            ArgumentNullException.ThrowIfNull(repositoryProvider);

            this.entityClassRepository = repositoryProvider.GetRepository<IDbEntityClass>();

            var rootEntityClass = FindRoot();

            if (rootEntityClass is not null)
            {
                Items.Add(new EntityClassNodeVM(this, rootEntityClass, null, true));
            }

            MoveNodeCommand = new RelayCommand<(EntityClassNodeVM dragged, EntityClassNodeVM target)>(MoveNode);
        }

        #endregion Public Constructors

        #region Public Properties

        public ObservableCollection<EntityClassNodeVM> Items { get; } = new ObservableCollection<EntityClassNodeVM>();

        public EntityClassEditorVM ClassEditor
        {
            get { return classEditor; }
            private set { SetProperty(ref classEditor, value); }
        }

        public EntityClassNodeVM SelectedClass
        {
            get { return selectedClass; }
            set { SetProperty(ref selectedClass, value); }
        }

        public ICommand MoveNodeCommand { get; }

        #endregion Public Properties

        #region Internal Methods

        internal bool DeleteItem(IDbEntityClass item)
        {
            return entityClassRepository.Remove(item);
        }

        internal void Edit(EntityClassNodeVM model)
        {
            ClassEditor = new EntityClassEditorVM(this, model);
        }

        internal IEnumerable<IDbEntityClass> FindChilds(IDbEntityClass parent)
        {
            return entityClassRepository.Entries.OfType<IDbEntityClass>().Where(item => item.ParentId == parent.Id);
        }

        internal bool IsUnique(string newId)
        {
            return entityClassRepository.Find(newId) is null;
        }

        internal IDbEntityClass NewClass(string id)
        {
            return (IDbEntityClass)entityClassRepository.New(id);
        }

        internal void UpdateClass(IDbEntityClass entityClass)
        {
            entityClassRepository.Update(entityClass);
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(SelectedClass):
                    Edit(SelectedClass);
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void MoveNode((EntityClassNodeVM dragged, EntityClassNodeVM target) param)
        {
            var (dragged, target) = param;

            if (dragged == null || target == null || dragged == target)
                return;

            if (IsDescendant(dragged, target))
                return;

            // Remove from old parent
            if (dragged.Parent != null)
            {
                dragged.Parent.Children.Remove(dragged);
            }
            else
            {
                Items.Remove(dragged);
            }

            // Add to new parent
            target.Children.Add(dragged);
            dragged.Parent = target;
        }

        private bool IsDescendant(EntityClassNodeVM source, EntityClassNodeVM target)
        {
            if (source.Children.Contains(target))
                return true;

            foreach (var child in source.Children)
            {
                if (child is null)
                {
                    continue;
                }

                if (IsDescendant(child, target))
                    return true;
            }

            return false;
        }

        private IDbEntityClass FindRoot()
        {
            return entityClassRepository.Entries.OfType<IDbEntityClass>().FirstOrDefault();
        }

        #endregion Private Methods
    }
}