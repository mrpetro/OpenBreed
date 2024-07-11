using OpenBreed.Input.Interface;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Components.Gui
{
    public interface ICursorAction
    {
        #region Public Properties

        string Name { get; }
        string Type { get; }

        #endregion Public Properties
    }

    public interface ICursorInputComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        //ICursorAction[] Actions { get; }

        #endregion Public Properties
    }

    public class CursorInputComponent : IEntityComponent
    {
        #region Public Constructors

        public CursorInputComponent(List<int> actions)
        {
            //Actions = actions;
        }

        #endregion Public Constructors

        #region Public Properties

        //public List<int> Actions { get; }

        #endregion Public Properties
    }

    public sealed class CursorInputComponentFactory : ComponentFactoryBase<ICursorInputComponentTemplate>
    {
        #region Private Fields

        private readonly IActionCodeProvider actionCodeProvider;

        #endregion Private Fields

        #region Public Constructors

        public CursorInputComponentFactory(IActionCodeProvider actionCodeProvider)
        {
            this.actionCodeProvider = actionCodeProvider;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(ICursorInputComponentTemplate template)
        {
            var actions = new List<int>();

            //foreach (var action in template.Actions)
            //{
            //    if (actionCodeProvider.TryGetCode(action.Type, action.Name, out int code))
            //        actions.Add(code);
            //}

            return new CursorInputComponent(actions);
        }

        #endregion Protected Methods
    }
}