namespace OpenBreed.Core.Managers
{
    public delegate void ActionPerformer(object[] args);

    public interface ITriggerMan
    {
        #region Public Methods

        int Register(string actionName);

        void Fire(int actionId, params object[] args);

        void OnEachAction(int actionId, ActionPerformer performer);

        void OnSingleAction(int actionId, ActionPerformer performer);

        void Fire(string actionName, params object[] args);

        void OnEachAction(string actionName, ActionPerformer performer);

        void OnSingleAction(string actionName, ActionPerformer performer);

        #endregion Public Methods
    }
}