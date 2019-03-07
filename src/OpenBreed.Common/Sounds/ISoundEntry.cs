using OpenBreed.Common.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Sounds
{
    public interface ISoundEntry : IEntry
    {
        string DataRef { get; }
    }
}
