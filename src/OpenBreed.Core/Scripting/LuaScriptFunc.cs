using NLua;

namespace OpenBreed.Core.Scripting
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
            return function.Call(args);
        }

        #endregion Public Methods
    }
}