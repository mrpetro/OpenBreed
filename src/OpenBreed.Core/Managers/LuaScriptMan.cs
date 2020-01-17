using NLua;
using OpenBreed.Core.Scripting;
using System;

namespace OpenBreed.Core.Managers
{
    /// <summary>
    /// LUA implementation of Script manager
    /// </summary>
    public class LuaScriptMan : IScriptMan
    {
        #region Private Fields

        private readonly Lua luaState;

        #endregion Private Fields

        #region Public Constructors

        public LuaScriptMan(ICore core)
        {
            Core = core;

            luaState = new Lua();
            luaState.LoadCLRPackage();
            luaState["Core"] = Core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public object GetObject(string objectName)
        {
            return luaState[objectName];
        }

        public object RunString(string script)
        {
            return luaState.DoString(script);
        }

        public object RunFile(string filePath)
        {
            return luaState.DoFile(filePath);
        }

        public void SetObject(string objectName, object value)
        {
            luaState[objectName] = value;
        }

        public IScriptFunc CompileString(string script, string name)
        {
            return new LuaScriptFunc(luaState.LoadString(script, name));
        }

        public IScriptFunc CompileFile(string filePath)
        {
            return new LuaScriptFunc(luaState.LoadFile(filePath));
        }

        #endregion Public Methods
    }
}