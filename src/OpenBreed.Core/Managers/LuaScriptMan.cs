using NLua;
using OpenBreed.Core.Scripting;

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

            //Define table placeholder for templates namespace
            luaState.NewTable("Templates");
            //Define table placeholder for entity templates namespace
            luaState.NewTable("Templates.Entities");
            //Define table placeholder for viewport templates namespace
            luaState.NewTable("Templates.Viewports");

        }


        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public object GetObject(string objectName)
        {
            var value = luaState[objectName];
            if (value is LuaFunction)
                return new LuaScriptFunc((LuaFunction)value);
            else
                return value;
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

        public bool TryInvokeFunction(string funcName, params object[] funcArgs)
        {
            var func = (LuaFunction)luaState[funcName] as LuaFunction;

            if (func == null)
                return false;

            func.Call(funcArgs);
            return true;
        }

        public bool TryInvokeFunction(string funcName, out object funcResult, params object[] funcArgs)
        {
            var func = (LuaFunction)luaState[funcName] as LuaFunction;

            if (func == null)
            {
                funcResult = null;
#if DEBUG
                Core.Logging.Warning($"'{funcName}' not existing.");
#endif
                return false;
            }

            funcResult = func.Call(funcArgs);
            return true;
        }

        #endregion Public Methods
    }
}