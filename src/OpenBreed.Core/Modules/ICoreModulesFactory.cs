using OpenBreed.Core.Modules.Audio;
using OpenBreed.Core.Modules.Rendering;

namespace OpenBreed.Core.Modules
{
    public interface ICoreModulesFactory
    {
        #region Public Methods

        IAudioModule CreateAudioModule(ICore core);

        IInputModule CreateInputModule(ICore core);

        IPhysicsModule CreatePhysicsModule(ICore core);

        IScriptingModule CreateScriptingModule(ICore core);

        #endregion Public Methods
    }
}