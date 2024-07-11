using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Editor.VM
{
    public class DbEntryEditorFactory
    {
        #region Private Fields

        private readonly Dictionary<Type, Type> creators = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Type> creatorsEx = new Dictionary<Type, Type>();
        private readonly IServiceProvider managerCollection;

        #endregion Private Fields

        #region Public Constructors

        public DbEntryEditorFactory(IServiceProvider managerCollection)
        {
            this.managerCollection = managerCollection;
        }

        #endregion Public Constructors

        #region Public Methods

        public void RegisterEditor<TDbEntry>() where TDbEntry : IDbEntry
        {
            var entryType = typeof(TDbEntry);

            if (creatorsEx.ContainsKey(entryType))
            {
                throw new InvalidOperationException($"Factory already has type '{entryType}' registered.");
            }

            creatorsEx.Add(entryType, typeof(EntryEditorVM<TDbEntry>));
        }

        public void Register<E, C>() where C : EntrySpecificEditorVM where E : IDbEntry 
        {
            var entryType = typeof(E);

            if (creators.ContainsKey(entryType))
            {
                throw new InvalidOperationException($"Factory already has type '{entryType}' registered.");
            }

            creators.Add(typeof(E), typeof(C));
        }

        #endregion Public Methods

        #region Internal Methods

        internal EntryEditorVM Create(IDbEntry dbEntry)
        {
            var type = GetEditorType(dbEntry);

            return (EntryEditorVM)managerCollection.GetRequiredService(type);
        }

        internal EntrySpecificEditorVM CreateSpecific(IDbEntry dbEntry)
        {
            var type = GetSpecificEditorType(dbEntry);

            return (EntrySpecificEditorVM)managerCollection.GetService(type);
        }

        #endregion Internal Methods

        #region Private Methods

        private Type GetSpecificEditorType(IDbEntry dbEntry)
        {
            var entryType = dbEntry.GetType();

            foreach (var item in creators)
            {
                if (entryType.GetInterfaces().Contains(item.Key) || entryType.Equals(item.Key))
                {
                    return item.Value;
                }
            }

            return null;
        }

        private Type GetEditorType(IDbEntry dbEntry)
        {
            var entryType = dbEntry.GetType();

            foreach (var item in creatorsEx)
            {
                if (entryType.GetInterfaces().Contains(item.Key) || entryType.Equals(item.Key))
                {
                    return item.Value;
                }
            }

            return null;
        }

        #endregion Private Methods
    }
}