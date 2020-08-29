using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM
{

    internal interface IEntryEditorCreator
    {
        EntryEditorVM Create(IRepository repository);
    }

    internal class EntryEditorCreator<T> : IEntryEditorCreator where T: EntryEditorVM
    {
        public EntryEditorVM Create(IRepository repository)
        {
            return Activator.CreateInstance(typeof(T), repository) as T;
        }
    }

    public class DbEntryEditorFactory
    {
        private Dictionary<Type, IEntryEditorCreator> _creators = new Dictionary<Type, IEntryEditorCreator>();

        public void Register<E,C>() where C : EntryEditorVM
        {
            var entryType = typeof(E);

            if (_creators.ContainsKey(entryType))
                throw new InvalidOperationException($"Factory already has type '{entryType}' registered.");

            _creators.Add(typeof(E), new EntryEditorCreator<C>());
        }

        public EntryEditorVM CreateEditor(IRepository repository)
        {
            var entryType = repository.GetType().GetInterfaces().FirstOrDefault();

            //IEntryEditorCreator creator = null;

            foreach (var item in _creators)
            {
                if (entryType.IsSubclassOf(item.Key) || entryType.Equals(item.Key))
                {
                    return item.Value.Create(repository);
                }
            }

            throw new InvalidOperationException($"{entryType}' type not registered.");

            //if (_creators.TryGetValue(entryType, out creator))
            //    return creator.Create(repository);
            //else
            //    throw new InvalidOperationException($"{entryType}' type not registered.");

        }
    }
}
