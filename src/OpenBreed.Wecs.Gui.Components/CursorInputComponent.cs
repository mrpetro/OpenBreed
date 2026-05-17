using OpenBreed.Input.Abstractions;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Gui.Components
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
}