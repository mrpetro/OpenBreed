using OpenBreed.Editor.UI.WinForms.Views;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM
{

    internal interface IViewCreator
    {
        EntryEditorBaseView Create();
    }

    internal class ViewCreator<T> : IViewCreator where T: EntryEditorBaseView, new()
    {
        public EntryEditorBaseView Create()
        {
            return new T();
        }
    }

    public class ViewFactory
    {
        private Dictionary<Type, IViewCreator> _creators = new Dictionary<Type, IViewCreator>();

        public void Register<VM,V>() where VM : EntryEditorVM where V: EntryEditorBaseView, new()
        {
            var entryType = typeof(VM);

            if (_creators.ContainsKey(entryType))
                throw new InvalidOperationException($"Factory already has type '{entryType}' registered.");

            _creators.Add(typeof(VM), new ViewCreator<V>());
        }

        public EntryEditorBaseView CreateEditor<VM>()
        {
            return CreateView(typeof(VM));
        }

        public EntryEditorBaseView CreateView(Type entryType)
        {
            IViewCreator creator = null;

            if (_creators.TryGetValue(entryType, out creator))
                return creator.Create();
            else
                throw new InvalidOperationException($"{entryType}' type not registered.");

        }
    }
}
