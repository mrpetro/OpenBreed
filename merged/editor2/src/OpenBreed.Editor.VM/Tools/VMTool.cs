using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Editor.VM.Tools
{
    public class VMTool : IEditorTool
    {
        #region Private Fields

        #endregion Private Fields

        #region Protected Constructors

        protected VMTool(string name, IToolView view)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (view == null)
                throw new ArgumentNullException(nameof(view));

            Name = name;
            View = view;
        }

        #endregion Protected Constructors

        #region Public Properties

        public string Name { get; }

        #endregion Public Properties

        #region Protected Properties

        protected IToolView View { get; }

        #endregion Protected Properties

        #region Public Methods

        public virtual void Activate()
        {
            throw new NotImplementedException();
        }

        public virtual void Deactivate()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}
