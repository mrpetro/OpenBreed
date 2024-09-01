using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Database.Interface.Items.Sounds;

namespace OpenBreed.Audio.Interface.Data
{
    public interface ISoundStreamDataLoader : IDataLoader<int>
    {
        int Load(IDbSound dbSound);
    }
}