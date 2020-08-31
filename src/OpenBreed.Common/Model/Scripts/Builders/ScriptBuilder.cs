﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Model.Scripts.Builders
{
    public class ScriptBuilder
    {
        internal string Script;

        public static ScriptBuilder NewScriptModel()
        {
            return new ScriptBuilder();
        }
        public ScriptModel Build()
        {
            return new ScriptModel(this);
        }

        public void SetScript(string script)
        {
            Script = script;
        }
    }
}
