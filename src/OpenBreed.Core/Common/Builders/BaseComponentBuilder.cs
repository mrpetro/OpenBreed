﻿using NLua;
using OpenBreed.Core.Common.Systems.Components;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Common.Builders
{
    /// <summary>
    /// Base entity component builder abstract class
    /// </summary>
    public abstract class BaseComponentBuilder : IComponentBuilder
    {
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

        public abstract IEntityComponent Build();

        public abstract void SetProperty(object key, object value);

        #endregion Public Methods
    }
}