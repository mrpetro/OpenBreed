using OpenTK.Input;

namespace OpenBreed.Core.Inputs
{
    public interface IControlHandler
    {
        #region Public Properties

        string ControlType { get; }

        #endregion Public Properties

        #region Public Indexers

        #endregion Public Indexers

        #region Public Methods

        void HandleKeyDown(Player player, float value, string actionName);
        void HandleKeyUp(Player player, float value, string actionName);
        void HandleKeyPressed(Player player, string controlAction);

        #endregion Public Methods
    }

    internal class KeyBinding
    {
        #region Internal Constructors

        internal KeyBinding(Player player, IControlHandler controlHandler, string controlAction)
        {
            Player = player;
            ControlHandler = controlHandler;
            ControlAction = controlAction;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal Player Player { get; }
        internal string ControlAction { get; }
        internal IControlHandler ControlHandler { get; }

        #endregion Internal Properties

        #region Internal Methods

        internal void OnKeyPressed()
        {
            ControlHandler.HandleKeyPressed(Player, ControlAction);
        }

        internal void OnKeyDown(float value)
        {
            ControlHandler.HandleKeyDown(Player, value, ControlAction);
        }

        internal void OnKeyUp(float value)
        {
            ControlHandler.HandleKeyUp(Player, value, ControlAction);
        }

        #endregion Internal Methods
    }
}