using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Sprites
{
    public interface ISpriteSetFromSprEntry : ISpriteSetEntry
    {
        string DataRef { get; }
    }
}
