using OpenBreed.Editor.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Wpf
{
    public class ControlFactory : IControlFactory
    {
        #region Private Fields

        private readonly Dictionary<Type, Func<object>> _initializers = new Dictionary<Type, Func<object>>();

        #endregion Private Fields

        #region Public Methods

        public void Register<TViewModel, TView>() where TViewModel : EntrySpecificEditorVM where TView : new()
        {
            var entryType = typeof(TViewModel);
            var viewType = typeof(TView);

            if (_initializers.ContainsKey(entryType))
            {
                throw new InvalidOperationException($"Factory already has type '{entryType}' registered.");
            }

            _initializers.Add(typeof(TViewModel), () => new TView());
        }

        public bool SupportsWpf(Type entryType)
        {
            return _initializers.ContainsKey(entryType);
        }

        public object Create(Type entryType)
        {
            if (_initializers.TryGetValue(entryType, out Func<object> initializer))
            {
                return initializer.Invoke();
            }

            throw new InvalidOperationException($"{entryType}' type not registered.");
        }

        #endregion Public Methods
    }
}