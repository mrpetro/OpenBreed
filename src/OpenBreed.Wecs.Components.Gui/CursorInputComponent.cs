using System.Collections.Generic;

namespace OpenBreed.Wecs.Components.Gui
{
    public interface ICursorInputComponentTemplate : IComponentTemplate
    {
    }

    public class CursorInputComponent : IEntityComponent
    {
        #region Private Fields

        private readonly Dictionary<int, int> bindings = new Dictionary<int, int>();

        #endregion Private Fields

        #region Public Constructors

        public CursorInputComponent()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void RemoveBinding(int id)
        {
            bindings.Remove(id);
        }

        public void SetBinding(int id, int keyId)
        {
            bindings[id] = keyId;
        }

        #endregion Public Methods
    }

    public sealed class CursorInputComponentFactory : ComponentFactoryBase<ICursorInputComponentTemplate>
    {
        #region Public Constructors

        public CursorInputComponentFactory()
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(ICursorInputComponentTemplate template)
        {
            return new CursorInputComponent();
        }

        #endregion Protected Methods
    }
}