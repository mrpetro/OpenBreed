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
        #region Public Properties

        string DataRef { get; set; }

        int SampleRate { get; set; }

        int BitsPerSample { get; set; }

        int Channels { get; set; }

        #endregion Public Properties
    }
}