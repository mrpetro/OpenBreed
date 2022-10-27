using NLua;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Scripting.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenBreed.Scripting.Lua
{
    /// <summary>
    /// LUA implementation of Script manager
    /// </summary>
    public class LuaScriptMan : IScriptMan
    {
        #region Private Fields

        private readonly NLua.Lua luaState;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public LuaScriptMan(ILogger logger)
        {
            this.logger = logger;

            luaState = new NLua.Lua();
            luaState.LoadCLRPackage();

            //Define table placeholder for templates namespace
            luaState.NewTable("Templates");
            //Define table placeholder for entity templates namespace
            luaState.NewTable("Templates.Entities");
            //Define table placeholder for viewport templates namespace
            luaState.NewTable("Templates.Viewports");

            //Allow C# Action delgates to be called from Lua
            luaState.RegisterLuaDelegateType(typeof(Action), typeof(LuaActionHandler));
        }

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties

        #region Public Methods

        public void RegisterDelegateType(Type delegateType, Type scriptDelegateType )
        {
            luaState.RegisterLuaDelegateType(delegateType, scriptDelegateType);
        }

        public void Expose(string apiName, object apiObj)
        {
            luaState[apiName] = apiObj;
        }

        public void ExposeMethod(object apiObj, string methodName, MethodInfo methodInfo)
        {
            luaState.RegisterFunction(methodName, apiObj, methodInfo);
        }

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

        private Dictionary<string, IScriptFunc> functionLookup = new Dictionary<string, IScriptFunc>();

        public void RegisterFunction(string functionName, IScriptFunc func)
        {
            if (functionLookup.ContainsKey(functionName))
                throw new InvalidOperationException($"Function '{functionName}' already registered.");

            functionLookup.Add(functionName, func);
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

        public IScriptFunc GetTableFunction(string funcName)
        {
            var t = luaState[funcName];

            if (t is not LuaFunction)
                return null;

            return new LuaScriptFunc((LuaFunction)t);
        }

        public IScriptFunc GetFunction(string funcName)
        {
            functionLookup.TryGetValue(funcName, out IScriptFunc func);
            return func;
        }

        public bool FunctionExists(string funcName)
        {
            return functionLookup.ContainsKey(funcName);
        }

        public bool TryInvokeFunction(string funcName, out object funcResult, params object[] funcArgs)
        {
            var func = (LuaFunction)luaState[funcName] as LuaFunction;

            if (func == null)
            {
                funcResult = null;

                #if DEBUG
                logger.Warning($"'{funcName}' not existing.");
                #endif
                return false;
            }

            funcResult = func.Call(funcArgs);
            return true;
        }

        #endregion Public Methods
    }

    internal class LuaActionHandler : NLua.Method.LuaDelegate
    {
        void CallFunction()
        {
            object[] args = new object[] { };
            object[] inArgs = new object[] { };
            int[] outArgs = new int[] { };
            base.CallFunction(args, inArgs, outArgs);
        }
    }
}