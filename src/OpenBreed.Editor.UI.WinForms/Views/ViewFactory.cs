using OpenBreed.Editor.UI.WinForms.Controls;
using OpenBreed.Editor.UI.WinForms.Views;
using OpenBreed.Editor.UI.Wpf;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OpenBreed.Editor.VM
{
    public class ViewFactory
    {
        #region Private Fields

        private readonly Dictionary<Type, Func<EntryEditorBaseView>> _initializers = new Dictionary<Type, Func<EntryEditorBaseView>>();

        private readonly IControlFactory controlFactory;

        #endregion Private Fields

        #region Public Constructors

        public ViewFactory(IControlFactory controlFactory)
        {
            this.controlFactory = controlFactory;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Register<VM, V>() where VM : EntryEditorVM where V : EntryEditorBaseView, new()
        {
            var entryType = typeof(VM);

            if (_initializers.ContainsKey(entryType))
            {
                throw new InvalidOperationException($"Factory already has type '{entryType}' registered.");
            }

            _initializers.Add(typeof(VM), () => new V());
        }

        public EntryEditorBaseView CreateView(Type entryType)
        {
            if (_initializers.TryGetValue(entryType, out Func<EntryEditorBaseView> initializer))
            {
                return initializer.Invoke();
            }

            throw new InvalidOperationException($"{entryType}' type not registered.");
        }

        public EntryEditorViewWpf CreateViewWpf(Type entryType)
        {
            if (!controlFactory.SupportsWpf(entryType))
            {
                return null;
            }

            return new EntryEditorViewWpf();
        }

        #endregion Public Methods
    }
}