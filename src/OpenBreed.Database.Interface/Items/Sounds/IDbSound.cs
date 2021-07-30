using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Sounds
{
    public interface IDbSound : IDbEntry
    {
        string DataRef { get; }
    }
}
