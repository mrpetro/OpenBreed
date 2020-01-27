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

        bool TryInvokeFunction(string funcName, out object funcResult, params object[] funcArgs);

        bool TryInvokeFunction(string funcName, params object[] funcArgs);

        void SetObject(string objectName, object value);

        #endregion Public Methods
    }
}