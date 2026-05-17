using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Scripting.Abstractions;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Scripting.Components.Xml
{
    [XmlRoot("Script")]
    public class XmlScriptComponent : XmlComponentTemplate, IScriptComponentTemplate
    {
        #region Public Properties

        [XmlElement("ScriptId")]
        public string ScriptId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            var dataLoaderFactory = serviceProvider.GetRequiredService<IDataLoaderFactory>();
            var scriptMan = serviceProvider.GetRequiredService<IScriptMan>();
            var scriptLoader = dataLoaderFactory.GetLoader<IScriptDataLoader>();

            var systemHooks = Array.Empty<ScriptHook>();

            if (!string.IsNullOrEmpty(ScriptId))
            {
                var scriptFunc = scriptLoader.Load(ScriptId);
                var result = scriptFunc.Invoke();

                if (result is object[] array)
                {
                    if (array[0] is Dictionary<object, object> entityHooks)
                    {
                        foreach (var hook in entityHooks)
                        {
                            if (hook.Key is string hookName)
                            {
                                if (hookName == "systemHooks")
                                {
                                    if (hook.Value is Dictionary<object, object> engineHooks)
                                        systemHooks = GetSystemHooks(scriptMan, ScriptId, engineHooks);
                                }
                            }
                        }
                    }
                }
            }

            return new ScriptComponent(
                ScriptId,
                systemHooks);
        }

        #endregion Public Methods

        #region Private Methods

        private ScriptHook[] GetSystemHooks(IScriptMan scriptMan, string scriptId, Dictionary<object, object> engineHooks)
        {
            var systemHooks = new List<ScriptHook>();

            foreach (var hook in engineHooks)
            {
                if (hook.Key is not string hookName)
                    continue;

                if (hook.Value is not IScriptFunc hookFunc)
                    continue;

                var functionId = $"{scriptId}.{hook.Key}";

                if (!scriptMan.FunctionExists(functionId))
                    scriptMan.RegisterFunction(functionId, hookFunc);

                systemHooks.Add(new ScriptHook(hookName, functionId));
            }

            return systemHooks.ToArray();
        }

        #endregion Private Methods
    }
}