using OpenBreed.Core.Scripting;

namespace OpenBreed.Core.Managers
{
    /// <summary>
    /// Script manager interface
    /// </summary>
    public interface IScriptMan
    {
        #region Public Methods

        IScriptFunc CompileString(string script, string name);

        IScriptFunc CompileFile(string filePath);

        object RunString(string script);

        object RunFile(string filePath);

        object GetObject(string objectName);

        void SetObject(string objectName, object value);

        #endregion Public Methods
    }
}