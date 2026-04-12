using OpenBreed.Wecs.Abstractions.Services;
using System;

using System.Linq;


namespace OpenBreed.Wecs.Abstractions.Extensions
{
    public static class EntityManExtensions
    {
        public static IEntity FindOrCreate(this IEntityMan entityMan, string tag, Func<IEntityBuilder, IEntity> initializer)
        {
            var foundEntity = entityMan.GetByTag(tag).FirstOrDefault();

            if (foundEntity is null)
            {
                var builder = entityMan.Create()
                    .SetTag(tag);
                foundEntity = initializer.Invoke(builder);
            }

            return foundEntity;
        }
    }
}
