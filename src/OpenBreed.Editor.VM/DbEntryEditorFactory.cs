using OpenBreed.Common;
using OpenBreed.Database.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Editor.VM
{
    public class DbEntryEditorFactory
    {
        #region Private Fields

        private readonly Dictionary<Type, Type> creators = new Dictionary<Type, Type>();
        private readonly IManagerCollection managerCollection;

        #endregion Private Fields

        #region Public Constructors

        public DbEntryEditorFactory(IManagerCollection managerCollection)
        {
            this.managerCollection = managerCollection;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Register<E, C>() where C : EntryEditorVM
        {
            var entryType = typeof(E);

            if (creators.ContainsKey(entryType))
                throw new InvalidOperationException($"Factory already has type '{entryType}' registered.");

            creators.Add(typeof(E), typeof(C));
        }

        #endregion Public Methods

        #region Internal Methods

        internal EntryEditorVM Create(IRepository repository)
        {
            var type = GetEditorType(repository);
            return (EntryEditorVM)managerCollection.GetManager(type);
        }

        #endregion Internal Methods

        #region Private Methods

        private Type GetEditorType(IRepository repository)
        {
            var entryType = repository.GetType().GetInterfaces().FirstOrDefault(item => item.IsGenericType && item.GetInterfaces().Contains(typeof(IRepository)));

            foreach (var item in creators)
            {
                if (entryType.IsSubclassOf(item.Key) || entryType.Equals(item.Key))
                    return item.Value;
            }

            throw new InvalidOperationException($"{entryType}' type not registered.");
        }

        #endregion Private Methods
    }
}