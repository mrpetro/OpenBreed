using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Audio.Helpers
{
    public class SoundMan : ISoundMan
    {
        public ISound GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ISound Load(string filePath, string id = null)
        {
            throw new NotImplementedException();
        }

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }
    }
}
