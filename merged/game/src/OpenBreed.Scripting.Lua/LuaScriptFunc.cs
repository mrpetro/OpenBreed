using NLua;
using OpenBreed.Scripting.Interface;
using System.Collections.Generic;

namespace OpenBreed.Scripting.Lua
{
    /// <summary>
    /// LUA implementation of script function
    /// </summary>
    internal class LuaScriptFunc : IScriptFunc
    {
        #region Private Fields

        private readonly LuaFunction function;

        #endregion Private Fields

        #region Internal Constructors

        internal LuaScriptFunc(LuaFunction function)
        {
            this.function = function;
        }

        #endregion Internal Constructors

        #region Public Methods

        public object Invoke(params object[] args)
        {
            return Convert(function.Call(args));
        }

        #endregion Public Methods

        #region Private Methods

        private object Convert(object inputObj)
        {
            if (inputObj is object[])
                return ConvertArray((object[])inputObj);
            else if (inputObj is LuaTable)
                return ConvertLuaTable((LuaTable)inputObj);
            else if (inputObj is LuaFunction)
                return ConvertLuaFunction((LuaFunction)inputObj);
            else
                return inputObj;
        }

        private object[] ConvertArray(object[] inArray)
        {
            var outArray = new object[inArray.Length];

            for (int i = 0; i < outArray.Length; i++)
                outArray[i] = Convert(inArray[i]);

            return outArray;
        }

        private object ConvertLuaFunction(LuaFunction infunction)
        {
            return new LuaScriptFunc(infunction);
        }

        private object ConvertLuaTable(LuaTable inTable)
        {
            var outDict = new Dictionary<object, object>();

            var enumerator = inTable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                outDict.Add(enumerator.Key, Convert(enumerator.Value));
            }

            return outDict;
        }

        #endregion Private Methods
    }
}