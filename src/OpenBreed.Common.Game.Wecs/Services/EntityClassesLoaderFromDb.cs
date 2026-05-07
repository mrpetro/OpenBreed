using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Services
{
    internal class EntityClassesLoaderFromDb : IEntityClassesLoader
    {
        private readonly IRepositoryProvider repositoryProvider;
        private readonly IRepository<IDbEntityClass> entityClassRepository;

        public EntityClassesLoaderFromDb(IRepositoryProvider repositoryProvider)
        {
            this.repositoryProvider = repositoryProvider ?? throw new ArgumentNullException(nameof(repositoryProvider));
            this.entityClassRepository = repositoryProvider.GetRepository<IDbEntityClass>();
        }

        public void Load(EntityClassLoadCallback callback)
        {
            foreach (var entryClass in entityClassRepository.Entries.OfType<IDbEntityClass>().Where(item => item.ParentId is null))
            {
                LoadEntityClass(entryClass, callback);
            }
        }

        private void LoadEntityClass(IDbEntityClass entityClass, EntityClassLoadCallback callback)
        {
            callback.Invoke(entityClass.Id, entityClass.ParentId);

            var members = entityClassRepository.Entries.OfType<IDbEntityClass>().Where(item => item.ParentId == entityClass.Id);

            foreach (var item in members)
            {
                LoadEntityClass(item, callback);
            }
        }
    }
}
