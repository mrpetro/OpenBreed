
namespace OpenBreed.Input.Interface
{
    public interface IInputHandler
    {
        #region Public Properties

        string InputType { get; }

        #endregion Public Properties

        #region Public Indexers

        #endregion Public Indexers

        #region Public Methods

        void HandleKeyDown(IPlayer player, float value, string actionName);
        void HandleKeyUp(IPlayer player, float value, string actionName);
        void HandleKeyPressed(IPlayer player, string controlAction);

        #endregion Public Methods
    }

    public class KeyBinding
    {
        #region Internal Constructors

        public KeyBinding(IPlayer player, IInputHandler controlHandler, string controlAction)
        {
            Player = player;
            ControlHandler = controlHandler;
            ControlAction = controlAction;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal IPlayer Player { get; }
        internal string ControlAction { get; }
        internal IInputHandler ControlHandler { get; }

        #endregion Internal Properties

        #region Internal Methods

        public void OnKeyPressed()
        {
            ControlHandler.HandleKeyPressed(Player, ControlAction);
        }

        public void OnKeyDown(float value)
        {
            ControlHandler.HandleKeyDown(Player, value, ControlAction);
        }

        public void OnKeyUp(float value)
        {
            ControlHandler.HandleKeyUp(Player, value, ControlAction);
        }

        #endregion Internal Methods
    }
}