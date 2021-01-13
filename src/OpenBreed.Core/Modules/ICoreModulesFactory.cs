using OpenBreed.Core.Modules.Rendering;

namespace OpenBreed.Core.Modules
{
    public interface ICoreModulesFactory
    {
        #region Public Methods

        IInputModule CreateInputModule(ICore core);

        IScriptingModule CreateScriptingModule(ICore core);

        #endregion Public Methods
    }
}