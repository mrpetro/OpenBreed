using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OpenBreed.Editor.VM
{
    public abstract class ParentEntryEditor<E> : EntryEditorBaseVM<E> where E : IEntry
    {
        #region Private Fields

        private static Dictionary<Type, Type> creators = new Dictionary<Type, Type>();

        private IEntryEditor<E> subeditor;
        private readonly IManagerCollection managerCollection;

        #endregion Private Fields

        #region Public Constructors

        public ParentEntryEditor(IManagerCollection managerCollection, IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider, string editorName) : base(workspaceMan, dataProvider, dialogProvider)
        {
            this.managerCollection = managerCollection;
            EditorName = editorName;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntryEditor<E> Subeditor
        {
            get { return subeditor; }
            private set { SetProperty(ref subeditor, value); }
        }

        //protected override void OnPropertyChanged(string name)
        //{
        //    switch (name)
        //    {
        //        case nameof(Subeditor):

        //            break;
        //        default:
        //            break;
        //    }

        //    base.OnPropertyChanged(name);
        //}

        public override string EditorName { get; }

        #endregion Public Properties

        #region Public Methods

        public static void RegisterSubeditor<SE,E>()
        {
            creators.Add(typeof(SE), typeof(IEntryEditor<SE>));
        }

        #endregion Public Methods

        #region Protected Methods



        protected override void UpdateVM(E source)
        {
            void SubmodelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                OnPropertyChanged(e.PropertyName);
            }

            if (Subeditor != null)
                Subeditor.PropertyChanged -= SubmodelPropertyChanged;

            Subeditor = CreateSubeditor(source);

            base.UpdateVM(source);
            Subeditor.UpdateVM(source);

            Subeditor.PropertyChanged += SubmodelPropertyChanged;
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
            var type = source.GetType();

            foreach (var interfaceType in type.GetInterfaces())
            {
                if (creators.TryGetValue(interfaceType, out Type subEditorType))
                    return (IEntryEditor<E>)managerCollection.GetManager(subEditorType);
            }

            throw new Exception("Editor not registered.");
        }

        #endregion Private Methods
    }
}