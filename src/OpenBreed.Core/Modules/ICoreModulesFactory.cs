namespace OpenBreed.Core.Modules
{
    public interface ICoreModulesFactory
    {
        #region Public Methods

        IScriptingModule CreateScriptingModule(ICore core);

        #endregion Public Methods
    }
}