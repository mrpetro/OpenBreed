using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Extensions
{
    public static class EntityManExtensions
    {
        public static IEntity FindOrCreate(this IEntityMan entityMan, string tag, Action<IEntity> create)
        {
            var foundEntity = entityMan.GetByTag(tag).FirstOrDefault();

            if (foundEntity is null)
            {
                foundEntity = entityMan.Create(tag);
                create.Invoke(foundEntity);
            }

            return foundEntity;
        }
    }
}
