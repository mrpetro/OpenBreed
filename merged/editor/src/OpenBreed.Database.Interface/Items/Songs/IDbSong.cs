using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBreed.Database.Interface.Items.Songs
{
    public interface IDbSong : IDbEntry
    {
        string DataRef { get; }
    }
}
