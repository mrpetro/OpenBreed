using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;

namespace OpenBreed.Editor.VM
{
    public abstract class ParentEntryEditor<E> : EntryEditorBaseVM<E> where E : IEntry
    {
        #region Private Fields

        private static Dictionary<Type, Func<ParentEntryEditor<E>, IEntryEditor<E>>> creators = new Dictionary<Type, Func<ParentEntryEditor<E>, IEntryEditor<E>>>();
        private IEntryEditor<E> subeditor;

        #endregion Private Fields

        #region Public Constructors

        public ParentEntryEditor(IRepository repository, string editorName) : base(repository)
        {
            EditorName = editorName;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntryEditor<E> Subeditor
        {
            get { return subeditor; }
            private set { SetProperty(ref subeditor, value); }
        }

        public override string EditorName { get; }

        #endregion Public Properties

        #region Public Methods

        public static void RegisterSubeditor<SE>(Func<ParentEntryEditor<E>, IEntryEditor<E>> subeditorCreator)
        {
            creators.Add(typeof(SE), subeditorCreator);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateVM(E source)
        {
            Subeditor = CreateSubeditor(source);

            base.UpdateVM(source);
            Subeditor.UpdateVM(source);
        }

        protected override void UpdateEntry(E target)
        {
            base.UpdateEntry(target);
            Subeditor.UpdateEntry(target);
        }

        #endregion Protected Methods

        #region Private Methods

        private IEntryEditor<E> CreateSubeditor(E source)
        {
            Func<ParentEntryEditor<E>, IEntryEditor<E>> editorCreator = null;

            var type = source.GetType();

            foreach (var interfaceType in type.GetInterfaces())
            {
                if (creators.TryGetValue(interfaceType, out editorCreator))
                    return editorCreator.Invoke(this);
            }

            throw new Exception("Editor not registered.");
        }

        #endregion Private Methods
    }
}