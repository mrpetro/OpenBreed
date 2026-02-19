using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Core.Components
{
    public interface IClassComponentTemplate : IComponentTemplate
    {
        string Name { get; }
    }

    [ComponentName("Class")]
    public class ClassComponent : IEntityComponent
    {
        public ClassComponent(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    public sealed class ClassComponentFactory : ComponentFactoryBase<IClassComponentTemplate>
    {
        private readonly IEntityClassMan entityClassMan;

        public ClassComponentFactory(IEntityClassMan entityClassMan)
        {
            this.entityClassMan = entityClassMan;
        }

        protected override IEntityComponent Create(IClassComponentTemplate template)
        {
            var entityClass = entityClassMan.GetByName(template.Name);
            return new ClassComponent(entityClass.Id);
        }
    }
}
