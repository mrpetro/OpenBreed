using System;
using System.Reflection;

namespace OpenBreed.Scripting.Interface
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

        void Expose(string apiName, object apiObj);
        void ExposeMethod(object apiObj, string methodName, MethodInfo methodInfo);

        #endregion Public Methods
    }
}