using OpenBreed.Common.Model.Maps;
using OpenBreed.Common.Model.Maps.Blocks;
using OpenBreed.Common.Model.Scripts;
using OpenBreed.Common.Builders.Scripts;
using OpenBreed.Common.Model.Texts;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Interface.Items.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    internal class ScriptsDataHelper
    {

        public static ScriptModel FromBinary(DataProvider provider, IScriptEmbeddedEntry scriptEntry)
        {
            var builder = ScriptBuilder.NewScriptModel();
            builder.SetScript(scriptEntry.Script);
            return builder.Build();
        }
    }
}
