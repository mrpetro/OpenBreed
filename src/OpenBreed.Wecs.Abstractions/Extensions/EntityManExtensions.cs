using OpenBreed.Wecs.Abstractions.Services;
using System;

using System.Linq;


namespace OpenBreed.Wecs.Abstractions.Extensions
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
