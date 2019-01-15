using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM
{

    internal interface IEntryEditorCreator
    {
        EntryEditorVM Create();
    }

    internal class EntryEditorCreator<T> : IEntryEditorCreator where T: EntryEditorVM, new()
    {
        public EntryEditorVM Create()
        {
            return new T();
        }
    }

    public class EntryEditorFactory
    {
        private Dictionary<Type, IEntryEditorCreator> _creators = new Dictionary<Type, IEntryEditorCreator>();

        public void Register<E,C>() where C : EntryEditorVM, new()
        {
            var entryType = typeof(E);

            if (_creators.ContainsKey(entryType))
                throw new InvalidOperationException($"Factory already has type '{entryType}' registered.");

            _creators.Add(typeof(E), new EntryEditorCreator<C>());
        }

        public EntryEditorVM CreateEditor<E>()
        {
            return CreateEditor(typeof(E));
        }

        public EntryEditorVM CreateEditor(Type entryType)
        {
            IEntryEditorCreator creator = null;

            if (_creators.TryGetValue(entryType, out creator))
                return creator.Create();
            else
                throw new InvalidOperationException($"{entryType}' type not registered.");

        }
    }
}
