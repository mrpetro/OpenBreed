using OpenBreed.Components.Common;
using OpenBreed.Ecsw.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Ecsw
{
    public interface IEntityMan
    {
        IEnumerable<Entity> GetByTag(object tag);

        IEnumerable<Entity> Where(Func<Entity, bool> predicate);

        Entity GetById(int id);

        Entity Create(List<IEntityComponent> initialComponents = null);

        void Destroy(Entity entity);
    }
}
