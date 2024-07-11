using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Model.Anims
{
    public class AnimBuilder
    {
        internal string Script;

        public static AnimBuilder NewAnimModel()
        {
            return new AnimBuilder();
        }
        public AnimModel Build()
        {
            return new AnimModel(this);
        }

        public void SetScript(string script)
        {
            Script = script;
        }
    }
}
