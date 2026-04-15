using OpenBreed.Common.Interface;
using OpenBreed.Database.Interface.Items.Sounds;

namespace OpenBreed.Audio.Abstractions.Data
{
    public interface ISoundSampleDataLoader : IDataLoader<int>
    {
        #region Public Methods

        int Load(IDbSound dbSound);

        #endregion Public Methods
    }
}