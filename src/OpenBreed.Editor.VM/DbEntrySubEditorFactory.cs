using OpenBreed.Common;
using System;
using System.Collections.Generic;

namespace OpenBreed.Editor.VM
{
    public class DbEntrySubEditorFactory
    {
        #region Private Fields

        private readonly Dictionary<Type, Dictionary<Type, Type>> creators = new Dictionary<Type, Dictionary<Type, Type>>();

        private readonly IServiceProvider managerCollection;

        #endregion Private Fields

        #region Public Constructors

        public DbEntrySubEditorFactory(IServiceProvider managerCollection)
        {
            this.managerCollection = managerCollection;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Register<SE, E>()
        {
            if (!creators.TryGetValue(typeof(E), out Dictionary<Type, Type> subEditorTypes))
            {
                subEditorTypes = new Dictionary<Type, Type>();
                creators.Add(typeof(E), subEditorTypes);
            }

            subEditorTypes.Add(typeof(SE), typeof(IEntryEditor<SE>));
        }

        public IEntryEditor<E> Create<E>(E source)
        {
            var type = typeof(E);
            var subType = source.GetType();

            if (!creators.TryGetValue(type, out Dictionary<Type, Type> subEditorTypes))
                return null;

            foreach (var interfaceType in subType.GetInterfaces())
            {
                if (subEditorTypes.TryGetValue(interfaceType, out Type subEditorType))
                    return (IEntryEditor<E>)managerCollection.GetService(subEditorType);
            }

            throw new Exception("Editor not registered.");
        }

        #endregion Public Methods
    }
}