using OpenBreed.Model.Maps;
using OpenBreed.Model.Maps.Blocks;
using OpenBreed.Model.Scripts;
using OpenBreed.Model.Texts;
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

        public static ScriptModel FromBinary(IModelsProvider dataProvider, IDbScriptEmbedded scriptEntry)
        {
            var builder = ScriptBuilder.NewScriptModel();
            builder.SetScript(scriptEntry.Script);
            return builder.Build();
        }

        internal static ScriptModel FromText(IModelsProvider dataProvider, IDbScriptFromFile entry)
        {
            if (entry.DataRef == null)
                return null;

            var data = dataProvider.GetModel<TextModel>(entry.DataRef);

            if (data == null)
                return null;

            var builder = ScriptBuilder.NewScriptModel();
            builder.SetScript(data.Text);
            return builder.Build();
        }
    }
}
