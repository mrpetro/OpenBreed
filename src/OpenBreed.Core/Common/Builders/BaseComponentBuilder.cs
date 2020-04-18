using NLua;
using OpenBreed.Core.Common.Systems.Components;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Common.Builders
{
    /// <summary>
    /// Base entity component builder abstract class
    /// </summary>
    public abstract class BaseComponentBuilder<T> : IComponentBuilder where T : IComponentBuilder
    {
        #region Private Fields

        private static Dictionary<string, Action<T, object>> setters = new Dictionary<string, Action<T, object>>();

        #endregion Private Fields

        #region Protected Constructors

        protected BaseComponentBuilder(ICore core)
        {
            Core = core;
        }

        #endregion Protected Constructors

        #region Protected Properties

        protected ICore Core { get; }

        #endregion Protected Properties

        #region Public Methods

        public static void ForEachInTable(object value, Func<object, object, object> action)
        {
            var table = (LuaTable)value;

            foreach (KeyValuePair<object, object> pair in table)
            {
                try
                {
                    action(pair.Key, pair.Value);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Invalid property with key '{pair.Key}'.", ex);
                }
            }
        }

        public static List<int> ToInt32Array(object value)
        {
            var luaTable = (LuaTable)value;

            var list = new List<int>();

            foreach (KeyValuePair<object, object> pair in luaTable)
            {
                try
                {
                    list.Add(Convert.ToInt32(pair.Value));
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Invalid component property with key '{pair.Key}'.", ex);
                }
            }

            return list;
        }

        public static Vector2 ToVector2(object value)
        {
            if (value is Vector2)
                return (Vector2)value;

            var luaTable = (LuaTable)value;

            return new Vector2(Convert.ToSingle(luaTable[1]),
                               Convert.ToSingle(luaTable[2]));
        }

        public static Color4 ToColor4(object value)
        {
            if (value is Color4)
                return (Color4)value;

            var luaTable = (LuaTable)value;

            return new Color4(Convert.ToByte(luaTable[1]),
                              Convert.ToByte(luaTable[2]),
                              Convert.ToByte(luaTable[3]),
                              Convert.ToByte(luaTable[4]));
        }

        public static List<string> ToStringArray(object value)
        {
            var luaTable = (LuaTable)value;

            var list = new List<string>();

            foreach (KeyValuePair<object, object> pair in luaTable)
            {
                try
                {
                    list.Add(Convert.ToString(pair.Value));
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Invalid component property with key '{pair.Key}'.", ex);
                }
            }

            return list;
        }

        public int ToFontId(object value)
        {
            if (value is int)
                return (int)value;

            var luaTable = (LuaTable)value;

            var fontName = Convert.ToString(luaTable[1]);
            var fontSize = Convert.ToInt32(luaTable[2]);

            return Core.Rendering.Fonts.Create(fontName, fontSize).Id;
        }

        public abstract IEntityComponent Build();

        public virtual void SetProperty(object key, object value)
        {
            var propertyName = Convert.ToString(key);
            setters[propertyName].Invoke((T)(IComponentBuilder)this, value);
        }

        #endregion Public Methods

        #region Protected Methods

        protected static void RegisterSetter(string name, Action<T, object> setter)
        {
            setters.Add(name, setter);
        }

        #endregion Protected Methods
    }
}