using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Physics.Interface.Managers
{
    public interface IShapeMan
    {
         int Count { get; }

        IShape GetByTag(string tag);
        IShape GetById(int id);
        void Register(string tag, IShape shape, bool overwrite = false);
    }
}
