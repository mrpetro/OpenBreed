using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM
{

    public interface IEntryEditorCreator
    {

        #region Public Methods

        EntryEditorVM Create(EditorApplication application, IDataProvider dataProvider, IUnitOfWork unitOfWork);

        #endregion Public Methods

    }

    public class DbEntryEditorFactory : IApplicationInterface
    {

        #region Private Fields

        private EditorApplication application;

        private Dictionary<Type, IEntryEditorCreator> _creators = new Dictionary<Type, IEntryEditorCreator>();

        #endregion Private Fields

        #region Public Constructors

        public DbEntryEditorFactory(EditorApplication application)
        {
            this.application = application;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Register<E, C>() where C : EntryEditorVM
        {
            var entryType = typeof(E);

            if (_creators.ContainsKey(entryType))
                throw new InvalidOperationException($"Factory already has type '{entryType}' registered.");

            _creators.Add(typeof(E), new EntryEditorCreator<C>());
        }

        public IEntryEditorCreator GetCreator(IRepository repository)
        {
            var entryType = repository.GetType().GetInterfaces().FirstOrDefault();

            foreach (var item in _creators)
            {
                if (entryType.IsSubclassOf(item.Key) || entryType.Equals(item.Key))
                {
                    return item.Value;
                }
            }

            throw new InvalidOperationException($"{entryType}' type not registered.");
        }

        #endregion Public Methods

    }

    internal class EntryEditorCreator<T> : IEntryEditorCreator where T : EntryEditorVM
    {

        #region Public Methods

        public EntryEditorVM Create(EditorApplication application, IDataProvider dataProvider, IUnitOfWork unitOfWork)
        {
            return Activator.CreateInstance(typeof(T), application, dataProvider, unitOfWork) as T;
        }

        #endregion Public Methods

    }
}
